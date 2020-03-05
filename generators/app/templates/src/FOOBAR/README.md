# FOOBAR (Angular)

## Prerequisites
This project runs on
* [dotnet core 2.2](https://www.microsoft.com/net/download/core)
* [nodejs v12.11.0](https://nodejs.org/en/download/)  (or higher)

The angular and typescript version are handled by the npm dependencies specified in `Package.json`:
* Angular core 9.0.0
* Typescript 3.7.5
* @acpaas-ui/ngx-components 3.3.0

## TL;DR;

 1. From command line in the project source folder as admin: `npm install` 
 2. If you want to do everything in Visual Studio:
	i. run `npm run build-dev`
	ii.  Start Visual Studio 
3. If you want to use Visual studio code:
    i. run `npm run build && dotnet run` from project root
    ii. Open visual studio code.
    iii. for the angular part: `npm start` or `ng serve` when angular CLI is installed globally on your system.

## Build

> All commands in this section need to be run from the src folder of the webproject.
>in a terminal/command prompt with administrator privileges.

first make sure the npm dependencies are installed.
If running from Visual Studio 2017 or up this should be done automatically but otherwise run
`npm install`

Now building the angular part of the project can be done by using the following commands; which are defined in the package.json file:

 - `npm run build-dev` will build in development mode
 - `npm run build` will build for production (with aot)

> It is important to run the production build before deploying as build-dev is more forgiving than build!  Otherwise a build might failed on the server.

Both build commands will build into the wwwroot and remove the index.html file from there.
This will force the serving of the MVC index.html and will force us to pass through authorization.  otherwise the static index file would be returned and we would bypass authorization.

## Run

There are multiple ways to run the project:

Running in visual studio:

>This requires the angular part to be build already and be present in the wwwroot folder. So make sure one of the above build commands has been executed.

Simply press the run button in Visual Studio  using the `Web` profile.
the application will run on localhost:5000



There are multiple ways to run the project:
*  Once the angular project is build (see above) press the run button in Visual Studio  using the `Web` profile.
		This will start your project as well as the client app.		
* To test only the angular client app, run `ng serve` in a terminal.
		This will also allow for live changes to the the app.
* You can also combine the two
	* Press the run button in visual studio or in a terminal run dotnet run`    
	 * In another terminal run `ng serve` or `npm start` for short.  The proxy will redirect api calls from the usual 4200 port to the backend.

>when running `ng serve` or `npm start` an npm build is not required

* A nice but optional feature is to use `dotnet watch run` instead of `dontnet run`. This allows for live editing of the back-end API.

## Configuration

>Be sure to check out the `_config` folder.
make sure appsettings, authentication and logging are correct. 
have a look at the TODO comments to know what to look for.

## Documentation

This project uses compodoc (https://compodoc.app/) to automatically generate documentation based on the projects components, services, ...
To generate your docs run `npm run compodoc`. The generated documentation lives in the documentation directory, just open the index.html file and you're good to go.

>Note: Angular 9 is still pretty new (don't worry it's stable) CompoDoc isn't supported just yet but will be in the near future.

## Swagger Generate

Chances are you're using swagger for your API docs. You can generate typescript models based on your swagger.json file.

1. Rename your swagger file to spec.json
2. Move the file to the swagger directory in the src directory.
3. run `npm run swagger-generate` this will generate typescript models inside shared/models/api/defs/ ready for use in your Angular app.

## Authentication and authorization

### On start up
To force the user to authenticate and check if that user is authorized an  `Authorize attribute` has been placed above the `HomeConntroller Index method`.
It is in  comments because it will not work until configuration has been done (see above) is done. **Comment it out** for it to work.

> The HomeController is located under project root > Mvc > Controllers

### Dev permissions

To make the life of the developer easier, dev permissions can be set to test guards and views depending on permissions. This will only work locally.
in  `auth.json`  make sure `usedevpermissions` is set to `true` and eddit the persmisions array.
when `usedevpermissions` is `false` the developer's real permission will be used.

> The auth.json is located under project root > _config

## AppConfig

Due to the use of Docker the use of AppConfig variables is slightly different.
AppConfig requires a `variables` root tag.
This tag is **required** when the project is enabled in AppConfig, even if it is just empty.

The AppConfig variables are given to the Docker as environment variables.
an empty AppConfig configuration will now look like this :
```
{
	"variables" : {
			//insert all your config at this level
	}
}
```
## TODO

This project has been made with the best of intentions but is far from perfect.
Due to time constraints the following still needs to be done:

 * The project structure could be a bit better: putting all the angular parts in the ClientApp folder, using the new Microsoft Angular template as a base.	 
	 The problem with this for the moment is security when serving static files.
	 
* Logging should be looked at.  especially the `serilog.sink` package which might contains bugs. A version without those bugs exists but is not compatible with this project due to dependency conflicts.

* **Anything you can think of** to make this a better starters project. Please feel free to create your own branch and create pull requests...
