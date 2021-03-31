'use strict'
var gulp = require('gulp');
var injectPartials = require('gulp-inject-partials');
var inject = require('gulp-inject');
var rename = require('gulp-rename');
var prettify = require('gulp-prettify');
var merge = require('merge-stream');
var replace = require('gulp-replace');



/* inject partials like sidebar and navbar */
gulp.task('injectPartial', function () {
    return gulp.src("./**/*.html", {
            base: "./"
        })
        .pipe(injectPartials())
        .pipe(gulp.dest("."));
});



/* inject Js and CCS assets into HTML */
gulp.task('injectAssets', function () {
    return gulp.src('./**/*.html')
        .pipe(inject(gulp.src([
            './src/assets/vendors/mdi/css/materialdesignicons.min.css',
            './src/assets/vendors/flag-icon-css/css/flag-icon.min.css',
            './src/assets/vendors/ti-icons/css/themify-icons.css',
            './src/assets/vendors/typicons/typicons.css',
            './src/assets/vendors/css/vendor.bundle.base.css',
            './src/assets/vendors/js/vendor.bundle.base.js',
        ], {
            read: false
        }), {
            name: 'plugins',
            relative: true
        }))
        .pipe(inject(gulp.src([
            '!css/horizontal-layouts.css', // <== !
            '!css/horizontal-layouts-2.css', // <== !
            '!css/sidebar-layouts.css', // <== !
            './src/assets/css/*.css',
            './src/assets/js/shared/off-canvas.js',
            './src/assets/js/shared/hoverable-collapse.js',
            './src/assets/js/shared/misc.js',
            './src/assets/js/shared/settings.js',
            './src/assets/js/shared/todolist.js'
        ], {
            read: false
        }), {
            relative: true
        }))
        .pipe(gulp.dest('.'));
});



/*replace image path and linking after injection*/
gulp.task('replacePath', function () {
    var replacePath1 = gulp.src('src/*/*.html', {
            base: "./"
        })
        .pipe(replace('src="assets/images/', 'src="../assets/images/'))
        .pipe(replace('href="pages/', 'href="pages/'))
        .pipe(replace('href="docs/', 'href="../docs/'))
        .pipe(replace('href="demo_1/', 'href="../demo_1/'))
        .pipe(replace('href="demo_2/', 'href="../demo_2/'))
        .pipe(replace('href="demo_3/', 'href="../demo_3/'))
        .pipe(replace('href="demo_4/', 'href="../demo_4/'))
        .pipe(replace('href="demo_5/', 'href="../demo_5/'))
        .pipe(replace('href="demo_6/', 'href="../demo_6/'))
        .pipe(replace('href="demo_7/', 'href="../demo_7/'))
        .pipe(replace('href="demo_8/', 'href="../demo_8/'))
        .pipe(replace('href="demo_9/', 'href="../demo_9/'))
        .pipe(replace('href="demo_10/', 'href="../demo_10/'))
        .pipe(replace('href="demo_11/', 'href="../demo_11/'))
        .pipe(replace('href="index.html"', 'href="index.html"'))
        .pipe(gulp.dest('.'));
    var replacePath2 = gulp.src('src/*/pages/*.html', {
            base: "./"
        })
        .pipe(replace('src="assets/images/', 'src="../../assets/images/'))
        .pipe(replace('href="pages/', 'href="../pages/'))
        .pipe(replace('href="pages/', 'href="../../pages/'))
        .pipe(replace('href="docs/', 'href="../../docs/'))
        .pipe(replace('href="demo_1/', 'href="../../demo_1/'))
        .pipe(replace('href="demo_2/', 'href="../../demo_2/'))
        .pipe(replace('href="demo_3/', 'href="../../demo_3/'))
        .pipe(replace('href="demo_4/', 'href="../../demo_4/'))
        .pipe(replace('href="demo_5/', 'href="../../demo_5/'))
        .pipe(replace('href="demo_6/', 'href="../../demo_6/'))
        .pipe(replace('href="demo_7/', 'href="../../demo_7/'))
        .pipe(replace('href="demo_8/', 'href="../../demo_8/'))
        .pipe(replace('href="demo_9/', 'href="../../demo_9/'))
        .pipe(replace('href="demo_10/', 'href="../../demo_10/'))
        .pipe(replace('href="demo_11/', 'href="../../demo_11/'))
        .pipe(replace('href="index.html"', 'href="../index.html"'))
        .pipe(gulp.dest('.'));
    var replacePath3 = gulp.src('src/*/pages/*/*.html', {
            base: "./"
        })
        .pipe(replace('src="assets/images/', 'src="../../../assets/images/'))
        .pipe(replace('href="pages/', 'href="../../pages/'))
        .pipe(replace('href="docs/', 'href="../../../docs/'))
        .pipe(replace('href="demo_1/', 'href="../../../demo_1/'))
        .pipe(replace('href="demo_2/', 'href="../../../demo_2/'))
        .pipe(replace('href="demo_3/', 'href="../../../demo_3/'))
        .pipe(replace('href="demo_4/', 'href="../../../demo_4/'))
        .pipe(replace('href="demo_5/', 'href="../../../demo_5/'))
        .pipe(replace('href="demo_6/', 'href="../../../demo_6/'))
        .pipe(replace('href="demo_7/', 'href="../../../demo_7/'))
        .pipe(replace('href="demo_8/', 'href="../../../demo_8/'))
        .pipe(replace('href="demo_9/', 'href="../../../demo_9/'))
        .pipe(replace('href="demo_10/', 'href="../../../demo_10/'))
        .pipe(replace('href="demo_11/', 'href="../../../demo_11/'))
        .pipe(replace('href="index.html"', 'href="../../index.html"'))
        .pipe(gulp.dest('.'));
    return merge(replacePath1, replacePath2, replacePath3);
});



gulp.task('html-beautify', function () {
    return gulp.src('src/*/**/*.html')
        .pipe(prettify({
            unformatted: ['pre', 'code', 'textarea']
        }))
        .pipe(gulp.dest(function (file) {
            return file.base;
        }));
});

/*sequence for injecting partials and replacing paths*/
gulp.task('inject', gulp.series('injectPartial', 'injectAssets', 'html-beautify', 'replacePath'));