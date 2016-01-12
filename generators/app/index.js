'use strict';
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var del = require('del');
var nd = require('node-dir');
var Guid = require('guid');

module.exports = yeoman.generators.Base.extend({
  
  prompting: function () {
    var done = this.async();

    // Zeg hi to the user
    this.log(yosay('Welcome to the fantastic Yeoman ' + chalk.red('dgp-web-aspnet5') + ' generator!'));

    // Ask project parameters
    var prompts = [{
      type: 'input',
      name: 'projectName',
      message: "Enter the name of the new project (don't forget the Pascal-casing):"
    }, {
      type: 'input',
      name: 'port',
      message: 'Enter the port for the new project:'
    }];

    this.prompt(prompts, function (props) {
      this.props = props;     // To access props later use this.props.someOption;
      done();
    }.bind(this));
  },

  writing: function () {
  
    // empty the directory
    console.log('Emptying target directory...');
    del.sync('**/*', { force: true, dot: true });
    
    var projectName = this.props.projectName;
    var lowerProjectName = projectName.toLowerCase(); 
    
    var solutionItemsGuid = Guid.create();
    var srcGuid = Guid.create();
    var testGuid = Guid.create();
    var starterKitGuid = Guid.create();
    var integrationGuid = Guid.create();
    var unitGuid = Guid.create();
    
    var copyOptions = { 
      process: function(contents) {
        var str = contents.toString();
        var result = str.replace(/StarterKit/g, projectName)
                        .replace(/starterkit/g, lowerProjectName)
                        .replace(/C3E0690A-0044-402C-90D2-2DC0FF14980F/g, solutionItemsGuid.value.toUpperCase())
                        .replace(/05A3A5CE-4659-4E00-A4BB-4129AEBEE7D0/g, srcGuid.value.toUpperCase())
                        .replace(/079636FA-0D93-4251-921A-013355153BF5/g, testGuid.value.toUpperCase())
                        .replace(/BD79C050-331F-4733-87DE-F650976253B5/g, starterKitGuid.value.toUpperCase())
                        .replace(/948E75FD-C478-4001-AFBE-4D87181E1BEC/g, integrationGuid.value.toUpperCase())
                        .replace(/0A3016FD-A06C-4AA1-A843-DEA6A2F01696/g, unitGuid.value.toUpperCase());
        return result;
      }
    };
      
    //  this.fs.copy(
    //     this.templatePath('**/**.*'),
    //     this.destinationPath(),
    //     copyOptions
    //  );
     
     //this.fs.move(this.destinationPath('StarterKit.sln'), this.destinationPath(this.props.projectName + '.sln'));
     
     var source = this.sourceRoot();
     var dest = this.destinationRoot();
     var fs = this.fs;
     
     //console.log('source: ' + source);
     //console.log('dest: ' + dest);
     
     // copy the files and rename starterkit to projectName
     
     console.log('Creation project skeleton...');
     
     nd.files(source, function (err, files) {
      for ( var i = 0; i < files.length; i++ ) {
        var filename = files[i].replace(/StarterKit/g, projectName).replace(/starterkit/g, lowerProjectName).replace(source, dest);
        //console.log(files[i] + ' --> ' + filename);
        fs.copy(files[i], filename, copyOptions);
      }
    });
  },

  install: function () {
    // this.installDependencies();
    //this.log('----');
    //this.log('Project naam :' + this.props.projectName);
    //this.log('Poort :' + this.props.port);
    //this.log('templatePath :' + this.templatePath());
    //this.log('destinationPath :' + this.destinationPath());
  }
});
