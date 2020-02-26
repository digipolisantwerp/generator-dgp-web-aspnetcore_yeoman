'use strict';
const Generator = require('yeoman-generator');
const chalk = require('chalk');
const yosay = require('yosay');
const del = require('del');
const nd = require('node-dir');
const {v4: uuidv4} = require('uuid');
const updateNotifier = require('update-notifier');
const pkg = require('./../../package.json');

module.exports = class extends Generator {

  constructor(args, opts) {
    super(args, opts);
  }

  prompting() {
    let done = this.async();

    // greet the user
    this.log(yosay('Welcome to the fantastic Yeoman ' + chalk.green('dgp-web-aspnetcore') + ' ' + chalk.blue('(' + pkg.version + ')') + ' generator!'));

    let notifier = updateNotifier({
      pkg,
      updateCheckInterval: 1000 * 60 * 5 // check every 5 minutes.
    });
    notifier.notify();

    if (notifier.update !== undefined) return;

    // ask project parameters
    let prompts = [{
      type: 'confirm',
      name: 'deleteContent',
      message: 'Delete the contents of this directory before generation (.git will be preserved)?:',
      default: true
    },
      {
        type: 'input',
        name: 'projectName',
        message: "Enter a name for your new project (preferably in PascalCase):"
      },
      {
        type: 'confirm',
        name: 'hasAppConfig',
        message: 'Does the project have an AppConfig configuration?',
        default: false
      },
      {
        type: 'input',
        name: 'appConfigGuid',
        message: 'Please enter the unique application key from AppConfig:',
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
        message: 'Please enter the UME application name:',
        when: function (response) {
          return response.hasUMEName;
        }
      }

    ];

    this.prompt(prompts).then((props) => {
      this.props = props;
      done();
    });
  }

  writing() {
    let done = this.async();

    /**
     * Empty the target directory first if the user wished to do so.
     * */
    if (this.props.deleteContent) {
      this.log('Emptying target directory... Starting fresh.');
      del.sync(['**/*', '!.git', '!.git/**/*'], {force: true, dot: true});
    }

    let projectName = this.props.projectName;
    let lowerProjectName = projectName.toLowerCase();
    let probableUrlName = lowerProjectName.endsWith('web') ? lowerProjectName.substr(0, lowerProjectName.length - 3) : lowerProjectName;
    let probableUmeName = probableUrlName.toUpperCase();

    let solutionItemsGuid = uuidv4();
    let srcGuid = uuidv4();
    let testGuid = uuidv4();
    let starterKitGuid = uuidv4();
    let integrationGuid = uuidv4();
    let unitGuid = uuidv4();

    let applicationGuid = this.props.hasAppConfig ? this.props.appConfigGuid.trim() : uuidv4();

    let applicationUmeName = this.props.hasUMEName ? this.props.appUMName.toUpperCase() : probableUmeName;

    let copyOptions = {
      process: function (contents) {
        let str = contents.toString();
        return str
          .replace(/FOOBAR/g, projectName)
          .replace(/foobar/g, lowerProjectName)
          .replace(/153C20F8-826E-4AE7-3087-BE77A1D2A988/g, solutionItemsGuid.toUpperCase())
          .replace(/B41FC458-86EC-FF29-1691-C526F2000F0E/g, srcGuid.toUpperCase())
          .replace(/4E6B5AAF-3ED4-7688-C19F-B32717889E89/g, testGuid.toUpperCase())
          .replace(/6B9C8153-26AA-83A5-4B93-119E48875A86/g, starterKitGuid.toUpperCase())
          .replace(/43A0EA6E-20EC-C3DE-C04C-D0E086D03DDA/g, integrationGuid.toUpperCase())
          .replace(/48B7DFB0-4E5F-FDFC-CFFD-33B79F8C342F/g, unitGuid.toUpperCase())
          .replace(/AA8563B1-CC02-4F01-9FAC-E93764F81C9B/g, applicationGuid)
          .replace(/UMEFOO/g, applicationUmeName)
          .replace(/TITLEFOO/g, applicationUmeName);
      }
    };

    let source = this.sourceRoot();
    let dest = this.destinationRoot();
    let fs = this.fs;

    // copy files and rename starterkit to projectName

    this.log('Creating project skeleton...');

    nd.promiseFiles(source).then((files) => {
      files.map((file, index) => {
        let filename = file
          .replace(/FOOBAR/g, projectName)
          .replace(/foobar/g, lowerProjectName)
          .replace(".npmignore", ".gitignore")
          .replace(source, dest);

        fs.copy(file, filename, copyOptions);

        if (index === (files.length - 1)) {
          done();
        }
      });
    });
  }

  install() {
    const packageJsonDir = `${process.cwd()}/src/${this.props.projectName}`;
    process.chdir(packageJsonDir);

    this.installDependencies({
      bower: false,
      npm: true
    });
  }

  end() {
    // Say goodbye
    this.log(yosay('All done, Enjoy!'));
  }
};
