"use strict";

var gulp = require('gulp'),
    del = require('del'),
    autoprefixer = require('gulp-autoprefixer'),
    sass = require('gulp-sass'),
    sassdoc = require('gulp-sassdoc'),
    sourcemaps = require('gulp-sourcemaps'),
    inject = require('gulp-inject'),
    angularFileSort = require('gulp-angular-filesort');

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
    scssFolder: 'scss/',
    viewsFolder: 'views/',
    nodeFolder: 'node_modules/'
};

var sourcePaths = {
    scripts: paths.webClientRoot + paths.scriptsFolder + paths.appFolder + '**/*.js',
    styles: paths.webClientRoot + paths.stylesFolder + paths.scssFolder + '*.scss',
    lib: paths.webClientRoot + paths.scriptsFolder + paths.libFolder + '**/*',
    images: paths.webClientRoot + paths.imagesFolder + '**/*',
    views: paths.webClientRoot + paths.scriptsFolder + paths.viewsFolder + '**/*'
};

var nodeSourcePaths = [
    paths.nodeFolder + 'angular/angular.js',
    paths.nodeFolder + 'angular-*/angular-*.js',
    paths.nodeFolder + 'angular-ui-router/release/angular-ui-router.js'
];

var targetPaths = {
    scripts: paths.webRoot + paths.appFolder,
    styles: paths.webRoot + paths.stylesFolder,
    lib: paths.webRoot + paths.libFolder,
    images: paths.webRoot + paths.imagesFolder,
    views: paths.webRoot + paths.viewsFolder
};

// Clean tasks

gulp.task('clean:scripts', function() {
    return del.sync(targetPaths.scripts);
});

gulp.task('clean:styles', function() {
    return del.sync(targetPaths.styles);
});

gulp.task('clean:lib', function() {
    return del.sync(targetPaths.lib);
});

gulp.task('clean:images', function() {
    return del.sync(targetPaths.images);
});

gulp.task('clean:views', function() {
    return del.sync(targetPaths.views);
});

gulp.task('clean:all', ['clean:scripts', 'clean:styles', 'clean:lib', 'clean:images', 'clean:views']);

// Sass tasks

gulp.task('sass:dev', function() {
    var sassOptions = {
        errLogToConsole: true,
        outputStyle: 'expanded'
    };
    return gulp.src(sourcePaths.styles)
               .pipe(sourcemaps.init())
               .pipe(sass(sassOptions).on('error', sass.logError))
               .pipe(sourcemaps.write('.'))
               //.pipe(autoprefixer())
               .pipe(gulp.dest(targetPaths.styles));
});

gulp.task('sass:prod', function() {
    var sassOptions = {
        errLogToConsole: true,
        outputStyle: 'compressed'
    };
    return gulp.src(sourcePaths.styles)
               .pipe(sass(sassOptions).on('error', sass.logError))
               //.pipe(autoprefixer())
               .pipe(gulp.dest(targetPaths.styles));
});

gulp.task('sassdoc', function() {
    var sassDocOptions = {
        dest: '../../docs/sassdocs'
    };
    return gulp.src(sourcePaths.styles)
               .pipe(sassdoc(sassDocOptions))
               .resume();
});

// Copy tasks

gulp.task('copy:scripts', function() {
    return gulp.src(sourcePaths.scripts).pipe(gulp.dest(targetPaths.scripts));
});

gulp.task('copy:lib', function() {
    return gulp.src(sourcePaths.lib).pipe(gulp.dest(targetPaths.lib));
});

gulp.task('copy:nodemodules', function() {
    return gulp.src(nodeSourcePaths).pipe(gulp.dest(targetPaths.lib));
});

gulp.task('copy:images', function() {
    return gulp.src(sourcePaths.images).pipe(gulp.dest(targetPaths.images));
});

gulp.task('copy:views', function() {
    return gulp.src(sourcePaths.views).pipe(gulp.dest(targetPaths.views));
});


gulp.task('copy:all', ['copy:scripts', 'copy:lib', 'copy:nodemodules', 'copy:images', 'copy:views']);

// Inject task

gulp.task('inject-index', function() {
    var target = gulp.src(paths.mvcRoot + 'Views/Home/Template/Index.cshtml');

    // It's not necessary to read the files (will speed up things), we're only after their paths: 
    var sources = gulp.src([targetPaths.lib + '**/*.js', targetPaths.scripts + '**/*.js', targetPaths.styles + '**/*.css', targetPaths.lib + '**/*.css'], {read: false});

    return target.pipe(inject(sources, { ignorePath: '/wwwroot' }))
      //.pipe(angularFileSort())
      .pipe(gulp.dest(paths.mvcRoot + 'Views/Home'));
});

// Default tasks

gulp.task('default', ['clean:all', 'copy:all', 'sass:dev', 'sassdoc', 'inject-index']);
gulp.task('dev', ['clean:scripts', 'clean:views', 'clean:styles', 'copy:scripts', 'copy:views', 'sass:dev', 'inject-index']);
gulp.task('prod', ['clean:all', 'copy:all', 'sass:prod', 'sassdoc', 'inject-index']);

// watch tasks

gulp.task('watch:sass', function() {
    return gulp.watch(sourcePaths.styles, ['clean:styles', 'sass:dev', 'inject-index'])
               .on('change', function (event) {
                   console.log('File ' + event.path + ' was ' + event.type + ', running sass tasks...');
               });
});

gulp.task('watch:scripts', function() {
    return gulp.watch(sourcePaths.scripts, ['clean:scripts', 'copy:scripts', 'inject-index'])
               .on('change', function (event) {
                   console.log('File ' + event.path + ' was ' + event.type + ', running script tasks...');
               });
});

gulp.task('watch:dev', ['watch:scripts', 'watch:sass']);
 
// Browser-sync tasks

gulp.task('browser-sync', function() {
    browsersync.init({
        proxy: "localhost:2230"
    });
});

gulp.task('serve:dev', function() {
    browsersync.init({
        proxy: "localhost:2230"
    });
    gulp.watch(sourcePaths.scripts, ['reload:scripts'])
               .on('change', function (event) {
                   console.log('File ' + event.path + ' was ' + event.type + ', running script tasks...');
               });
    gulp.watch(sourcePaths.styles, ['reload:styles'])
               .on('change', function (event) {
                   console.log('File ' + event.path + ' was ' + event.type + ', running sass tasks...');
               });
    gulp.watch(sourcePaths.views, ['reload:views'])
               .on('change', function (event) {
                   console.log('File ' + event.path + ' was ' + event.type + ', running view tasks...');
               });
});

gulp.task('reload:scripts', ['clean:scripts', 'copy:scripts'], function() {
    browsersync.reload();
});

gulp.task('reload:styles', ['clean:styles', 'sass:dev'], function() {
    browsersync.reload();
});

gulp.task('reload:views', ['clean:views', 'copy:views'], function() {
    browsersync.reload();
});
