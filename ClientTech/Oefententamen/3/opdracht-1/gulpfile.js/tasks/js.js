const {src, dest} = require('gulp');
const concat = require('gulp-concat');
const order = require('gulp-order');
const uglify = require('gulp-uglify');

    const fn = function (jsfiles, jsorder) {
        console.log(jsfiles)
        return function () {
            return src(jsfiles)
                .pipe(order(jsorder))

//student uitwerking

                .pipe(dest('./dist'));
        }
};

exports.js = fn;
