'use strict';
const yeoman = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const del = require('del');
const nd = require('node-dir');
const Guid = require('guid');
const updateNotifier = require('update-notifier');
const pkg = require('./../../package.json');

module.exports = yeoman.generators.Base.extend({

  prompting: function () {
    let done = this.async();

    // greet the user
    this.log(yosay('Welcome to the fantastic Yeoman ' + chalk.green('dgp-web-aspnetcore') + ' ' + chalk.blue('(' + pkg.version + ')') + ' generator!'));

    let notifier = updateNotifier({
      pkg,
      updateCheckInterval: 1000 * 60 * 5      // check every 5 minutes.
    });
    notifier.notify();

    if (notifier.update !== undefined) return;

    // ask project parameters
    let prompts = [{
      type: 'confirm',
      name: 'deleteContent',
      message: 'Delete the contents of this directory before generation (.git will be preserved) ?:',
      default: true
    },
      {
        type: 'input',
        name: 'projectName',
        message: "Enter the name of the new project (don't forget the Pascal-casing):"
      },
      {
        type: 'confirm',
        name: 'hasAppConfig',
        message: 'Does the project have an Appconfig configuration ?',
        default: false
      },
      {
        type: 'input',
        name: 'appConfigGuid',
        message: 'Please enter the application unique key from Appconfig:',
        when: function (response) {
          return response.hasAppConfig;
        }
      },
      {
        type: 'confirm',
        name: 'hasUMEName',
        message: 'Does the project already have a UME application name?',
        default: false
      },
      {
        type: 'input',
        name: 'appUMName',
        message: 'Please enter the the UME application name:',
        when: function (response) {
          return response.hasUMEName;
        }
      }

    ];

    this.prompt(prompts, function (props) {
      this.props = props;     // To access props later use this.props.someOption;
      done();
    }.bind(this));
  },

  writing: function () {

    // empty target directory
    console.log('Emptying target directory...');
    if (this.props.deleteContent === true) {
      del.sync(['**/*', '!.git', '!.git/**/*'], {force: true, dot: true});
    }

    let projectName = this.props.projectName;
    let lowerProjectName = projectName.toLowerCase();
    let probableUrlName = lowerProjectName.endsWith("web") ? lowerProjectName.substr(0, lowerProjectName.length - 3) : lowerProjectName;
    let probableUmeName = probableUrlName.toUpperCase();

    let solutionItemsGuid = Guid.create();
    let srcGuid = Guid.create();
    let testGuid = Guid.create();
    let starterKitGuid = Guid.create();
    let integrationGuid = Guid.create();
    let unitGuid = Guid.create();

    let applicationGuid = this.props.hasAppConfig ? this.props.appConfigGuid.trim() : Guid.create().value;

    let applicationUmeName = this.props.hasUMEName ? this.props.appUMName.toUpperCase() : probableUmeName;

    //console.log("APP UME NAME = " + applicationUmeName)

    let copyOptions = {
      process: function (contents) {
        let str = contents.toString();
        let result = str.replace(/FOOBAR/g, projectName)
          .replace(/foobar/g, lowerProjectName)
          .replace(/153C20F8-826E-4AE7-3087-BE77A1D2A988/g, solutionItemsGuid.value.toUpperCase())
          .replace(/B41FC458-86EC-FF29-1691-C526F2000F0E/g, srcGuid.value.toUpperCase())
          .replace(/4E6B5AAF-3ED4-7688-C19F-B32717889E89/g, testGuid.value.toUpperCase())
          .replace(/6B9C8153-26AA-83A5-4B93-119E48875A86/g, starterKitGuid.value.toUpperCase())
          .replace(/43A0EA6E-20EC-C3DE-C04C-D0E086D03DDA/g, integrationGuid.value.toUpperCase())
          .replace(/48B7DFB0-4E5F-FDFC-CFFD-33B79F8C342F/g, unitGuid.value.toUpperCase())
          .replace(/AA8563B1-CC02-4F01-9FAC-E93764F81C9B/g, applicationGuid)
          .replace(/UMEFOO/g, applicationUmeName)
          .replace(/TITLEFOO/g, applicationUmeName)
        return result;
      }
    };

    let source = this.sourceRoot();
    let dest = this.destinationRoot();
    let fs = this.fs;

    // copy files and rename starterkit to projectName

    console.log('Creation project skeleton...');

    nd.files(source, function (err, files) {
      for (let i = 0; i < files.length; i++) {
        let filename = files[i].replace(/FOOBAR/g, projectName)
          .replace(/foobar/g, lowerProjectName)
          .replace(".npmignore", ".gitignore")
          .replace(source, dest);
        //console.log(files[i] + ' --> ' + filename);
        fs.copy(files[i], filename, copyOptions);
      }
    });
  },

  install: function () {
    // this.installDependencies();
  }
});
