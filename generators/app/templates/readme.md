Manueel proces (dit kan geautomatiseerd met de starterkit_generator)

	* nieuwe repo in stash maken voor de nieuwe solution : projectnaam_aspnet5 (mss best alles lowercase)
	* git clone van de nieuwe repo
	* git clone / checkout van repo PLAT/starterkit_aspnet5
	* folders en files kopiëren van repo starterkit_aspnet5 naar de nieuwe repo (niet de .vs en .git folders en ook niet deze readme.md)
	* replace van StarterKit naar ProjectNaam in .gitignore
	* rename van de solution/project files/folders :
		* StarterKit.sln
		* folder src/StarterKit
		* src/StarterKit/StarterKit.kproj
		* folder test/StarterKit.IntegrationTests
		* folder test/StarterKit.UnitTests
		* test/StarterKit.IntegrationTests/StarterKit.IntegrationTests.kproj
		* test/StarterKit.UnitTests/StarterKit.UnitTests.kproj
	* replace van StarterKit in .sln en .kproj
		* ProjectNaam.sln
		* src/ProjectNaam/ProjectNaam.kproj
		* test/ProjectNaam.IntegrationTests/ProjectNaam.IntegrationTests.kproj
		* test/ProjectNaam.UnitTests/ProjectNaam.UnitTests.kproj
	* replace van de guids in .sln en .kproj files
		* ProjectNaam.sln
		* src/ProjectNaam/ProjectNaam.kproj
		* test/ProjectNaam.IntegrationTests/ProjectNaam.IntegrationTests.kproj
		* test/ProjectNaam.UnitTests/ProjectNaam.UnitTests.kproj
	* web poorten    (10 verhogen tov vorig project)
		* in src\ProjectNaam\project.json voor web en kestrel
		* in src\ProjectNaam\ProjectNaam.kproj voor IIS Express
		* in src\ProjectNaam\WebUI\Scripts\app\shared\settings.json dezelfde als die voor IIS Express
	* solution openen en global replace (in alle files) van
		* StarterKit naar ProjectNaam (casing optie aanzetten)
		* starterkit naar projectnaam (casing optie aanzetten)
	* nog eens global search op starterkit (casing afzetten) en aanpassen waar er nog zijn overgebleven (indien project.json.lock ==> deze mogen verwijderd worden)
	* connectionstring in src\ProjectNaam\Configs\dbconfig.json

	* rebuild
	* npm packages restoren
	* grunt default task uitvoeren
	* testen via F5
	* files stagen in SourceTree (oppassen dat wwwroot\*, node_modules en bower_modules niet mee gaan)
	* pushen naar master branch
	* development branch aanmaken en als default instellen



