"use strict";

var gulp = require('gulp'),
    del = require('del'),
    autoprefixer = require('gulp-autoprefixer'),
    sass = require('gulp-sass'),
    sassdoc = require('gulp-sassdoc'),
    sourcemaps = require('gulp-sourcemaps'),
    inject = require('gulp-inject'),
    bower = require('gulp-bower'),
    bowerfiles = require('main-bower-files'),
    flatten = require('gulp-flatten'),
    es = require('event-stream');

var browsersync = require('browser-sync').create();

var paths = {
    webRoot: './wwwroot/',
    configRoot: './_config/',
    apiRoot: './Api/',
    mvcRoot: './Mvc/',
    serviceAgentsRoot: './ServiceAgents/',
    sharedRoot: './Shared/',
    startupRoot: './Startup/',
    webClientRoot: './WebClient/',
    scriptsFolder: 'scripts/',
    appFolder: 'app/',
    libFolder: 'lib/',
    stylesFolder: 'styles/',
    imagesFolder: 'img/',
    sassFolder: 'scss/',
    specsFolder: 'specs/',
    nodeFolder: 'node_modules/'
};

var sourcePaths = {
    app: paths.webClientRoot + paths.scriptsFolder + paths.appFolder,
    styles: paths.webClientRoot + paths.stylesFolder,
    sass: paths.webClientRoot + paths.stylesFolder + paths.sassFolder,
    lib: paths.webClientRoot + paths.scriptsFolder + paths.libFolder,
    images: paths.webClientRoot + paths.imagesFolder,
    scripts: paths.webClientRoot + paths.scriptsFolder,
    specs: paths.webClientRoot + paths.specsFolder
};

var targetPaths = {
    app: paths.webRoot + paths.scriptsFolder + paths.appFolder,
    styles: paths.webRoot + paths.stylesFolder,
    lib: paths.webRoot + paths.libFolder,
    images: paths.webRoot + paths.imagesFolder,
    scripts: paths.webRoot + paths.scriptsFolder
};

// Clean tasks

gulp.task('clean:wwwRoot', function () {
    return del.sync([targetPaths.scripts, targetPaths.styles, targetPaths.images, '!.gitkeep']);
});

// Copy tasks
gulp.task('copy:webClient', ['clean:wwwRoot'], function () {
    return es.concat(
            gulp.src(sourcePaths.images + '**/*').pipe(gulp.dest(targetPaths.images)),
            gulp.src(sourcePaths.app + '**/*').pipe(gulp.dest(targetPaths.app)),
            gulp.src(sourcePaths.lib + '**/*').pipe(gulp.dest(targetPaths.lib))
        );
});

gulp.task('copy:all:dev', ['copy:webClient']);
gulp.task('copy:all:prd', ['copy:webClient']);


//bower
gulp.task('bower', function () {
    return bower();
});

gulp.task('bower-files:dev', function () {
    return gulp.src(bowerfiles(), { base: 'bower_components' })
        .pipe(gulp.dest(paths.webRoot + 'lib/'));
});

gulp.task('bower-files:prd', function () {
    return gulp.src(bowerfiles(), { base: 'bower_components' })
        .pipe(gulp.dest(paths.webRoot + 'lib/'));
});

// Sass tasks

gulp.task('sass:dev', ['copy:all:dev'], function () {
    var sassOptions = {
        errLogToConsole: true,
        outputStyle: 'expanded'
    };
    return gulp.src(sourcePaths.sass + '**/*.scss')
        .pipe(sourcemaps.init())
        .pipe(sass(sassOptions).on('error', sass.logError))
        .pipe(sourcemaps.write('.'))
        //.pipe(autoprefixer())
        .pipe(gulp.dest(targetPaths.styles));
});

gulp.task('sass:prd', ['copy:all:prd'], function () {
    var sassOptions = {
        errLogToConsole: true,
        outputStyle: 'compressed'
    };

    var sassDocOptions = {
        dest: '../../docs/sassdocs'
    };

    return gulp.src(sourcePaths.sass + '**/*.scss')
        .pipe(sassdoc(sassDocOptions))
        .pipe(sass(sassOptions).on('error', sass.logError))
        //.pipe(autoprefixer())
        .pipe(gulp.dest(targetPaths.styles));
});

// Inject task

gulp.task('inject-index', function () {
    var target = gulp.src(paths.mvcRoot + 'Views/Home/Template/Index.cshtml');

    //// It's not necessary to read the files (will speed up things), we're only after their paths: 
    var sources = gulp.src([targetPaths.lib + 'jquery/**/*.js', targetPaths.lib + 'angular/**/*.js', targetPaths.lib + 'angular-*/**/*.js', targetPaths.lib + '**/*.js', targetPaths.app + 'app.js', targetPaths.app + '**/*.js', targetPaths.app + '**/**/*.js',
        targetPaths.lib + 'dgp-bootstrap-sass/dist/css/main.css', targetPaths.lib + '**/*.css', targetPaths.styles + '**/*.css'], { read: false });

    return target.pipe(inject(sources, { ignorePath: '/wwwroot' }))
        //  //.pipe(angularFileSort())
        .pipe(gulp.dest(paths.mvcRoot + 'Views/Home'));
});

// Default tasks


gulp.task('dev', ['sass:dev', 'bower-files:dev'], function () {
    return gulp.start('inject-index');
});
gulp.task('prd', ['sass:prd', 'bower-files:prd'], function () {
    return gulp.start('inject-index');
});
gulp.task('default', ['dev']);

// watch tasks

gulp.task('watch', function () {
    return gulp.watch([sourcePaths.scripts + '**/*', sourcePaths.sass + '*/*.scss'], ['dev'])
        .on('change', function (event) {
            console.log('File ' + event.path + ' was ' + event.type + ', running dev tasks...');
        });
});

// Browser-sync tasks

gulp.task('browser-sync', function () {
    browsersync.init({
        proxy: "localhost:2230"
    });
});

gulp.task('serve', function () {
    browsersync.init({
        proxy: "localhost:2230"
    });
    gulp.watch([sourcePaths.scripts + '**/*', sourcePaths.sass + '*/*.scss'], ['reload'])
        .on('change', function (event) {
            console.log('File ' + event.path + ' was ' + event.type + ', running dev tasks...');
        });
});

gulp.task('reload', ['dev'], function () {
    browsersync.reload();
});

