# generator-dgp-web-aspnetcore

Yeoman generator for a new ASP.NET Core 2.0 Web Client project.

## Installation

Make sure you have installed a recent version of node.js. You can download it here : https://nodejs.org/en/. 

Install Yeoman :

``` bash
npm install yo -g
``` 

The _**-g**_ flag installs the generator globally so you can run yeoman from anywhere.

Install the generator :

It is recommended to first uninstall any current installed version before installing the latest version to avoid remains of previous versions in the npm cache.

``` bash
npm uninstall generator-dgp-web-aspnetcore -g
```

Install the current version.

``` bash
npm install generator-dgp-web-aspnetcore -g
```

## Generate a new ASP.NET Core project

In a command prompt, navigate to the directory where you want to create the new project and type :

``` bash
yo dgp-web-aspnetcore
```

Answer the questions :-)

## The ASP.NET Core solution

### Startup

Enter your application Id, which you can find in AppConfig, in _config\app.json. It will be used in the StartUp class -> ConfigureServices -> services.AddApplicationServices

### Logging

Almost everything is preset for logging to Kibana. Enter the name of your logging index in _config\logging.json -> "indexFormat": "logstash-{tenant}-{your logging index goes here}-{0:yyyy.MM.dd}" .
The maximum length of tenant (application- or system-) and logging index name is 30 characters !