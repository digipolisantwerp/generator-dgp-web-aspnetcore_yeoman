/// <binding />
module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: "wwwroot/lib",
                    layout: "byComponent",
                    cleanTargetDir: false,
                    cleanBowerDir: false,
                    verbose: true
                }
            }
        },
        copy: {
            scripts: {
                expand: true,
                cwd: "WebUI/Scripts/",
                src: ["app/**/*", "lib/**/*", "views/**/*"],
                dest: "wwwroot/"
            },
            styles: {
                expand: true,
                cwd: "WebUI/Styles/",
                src: ["**/*"],
                dest: "wwwroot/css/"
            },
            templates: {
            	expand: true,
            	cwd: "WebUI/Templates/",
            	src: ["**/*"],
            	dest: "wwwroot/templates/"
            }
        },
        includeSource: {
            options: {
                basePath: "wwwroot/",
                baseUrl: "~/",
            },
            myTarget: {
                files: {
                    "WebUI/Views/Home/Index.cshtml": "WebUI/Views/Home/IndexTemplate.cshtml"
                }
            }
        },
        sass: {
            dist: {
                options: {
                    outputStyle: 'compressed',
                    sourceComments: false,
                    sourceMap: false
                },
                files: {
                    'WebUI/Styles/site.css': 'WebUI/Styles/scss/site.scss'
                }
            },
            dev: {
                options: {
                    outputStyle: 'expanded',    // nested ,  expanded ,  compact ,  compressed 
                    sourceComments: true,
                    sourceMap: true
                },
                files: {
                    'WebUI/Styles/site.css': 'WebUI/Styles/scss/site.scss'
                }
            }
        },
        watch: {
            scripts: {
                files: ["WebUI/Scripts/**/*.js", "WebUI/Scripts/**/*.html", "WebUI/Templates/**/*.html"],
                tasks: ["clean", "copy:scripts", "copy:styles", "copy:templates", "includeSource"],
                options: {
                    spawn: false,
                },
            },
            css: {
                files: ["WebUI/Styles/scss/*.scss"],
                tasks: ["sass:dev"],
                options: {
                    spawn: false,
                },
            }
        },
    	jasmine: {
    		main: {
    			src: [
    				"wwwroot/lib/jquery/js/*.min.js",
    				"wwwroot/lib/jquery-validation/js/jquery.validate.min.js",
    				"wwwroot/lib/jquery-validation-unobtrusive/js/*.min.js",
    				"wwwroot/lib/angular/js/*.min.js",
    				"wwwroot/lib/angular-*/js/*.min.js",
    				"wwwroot/lib/angular-mocks/js/angular-mocks.js",
    				"wwwroot/lib/bootstrap/js/bootstrap.min.js",
    				"wwwroot/lib/ui-bootstrap/js/ui-bootstrap-tpls.min.js",
    				"wwwroot/lib/respond/js/*.min.js",
    				"wwwroot/lib/detect-element-resize/js/*.js",
    				"wwwroot/lib/underscore/js/*.js",
    				"wwwroot/app/app.js",
    				"wwwroot/app/shared/**/*.js",
    				"wwwroot/app/**/*.js"
    			],
    			options: {
    				specs: "WebUI/Scripts/specs/*spec.js",
					keepRunner: true
    			}
    		}
    	},
    	clean: {
    		app: ["wwwroot/app/**/*", "wwwroot/views/**/*"],
    		lib: ["wwwroot/lib/**/*", "wwwroot/css/**/*", "wwwroot/templates/**/*"],
    		jasmine: ["_specRunner.html", ".grunt"],
    		nodemodules: [
				"node_modules/grunt-contrib-jasmine/node_modules/grunt-lib-phantomjs",
				"node_modules/grunt-bower-task/node_modules/bower/node_modules/bower-registry-client",
				"node_modules/grunt-bower-task/node_modules/bower/node_modules/insight",
				"node_modules/grunt-bower-task/node_modules/bower/node_modules/update-notifier"
    		]
    	}
    });

    grunt.registerTask("default", ["clean:nodemodules", "clean:app", "clean:lib", "bower:install", "sass:dev", "copy:scripts", "copy:styles", "copy:templates", "includeSource"]);

    grunt.registerTask("cleanmodules", ["clean:nodemodules"]);

    grunt.registerTask("test", ["jasmine:main:build", "jasmine:main", "clean:jasmine"]);

	// Deze task wordt gebruikt door het build systeem
    grunt.registerTask("package", ["clean:app", "clean:lib", "clean:jasmine", "bower:install", "sass:dist", "copy:scripts", "copy:styles", "copy:templates", "includeSource"]);

    // This line needs to be at the end of this file.    
    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks("grunt-include-source");
    grunt.loadNpmTasks("grunt-contrib-copy");
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks("grunt-contrib-jasmine");
    grunt.loadNpmTasks("grunt-bower-task");
    
};