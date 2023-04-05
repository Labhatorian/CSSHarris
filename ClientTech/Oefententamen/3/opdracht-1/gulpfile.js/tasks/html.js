const {src, dest} = require('gulp');


    const fn = function (htmlFiles) {
        return function () {
            return src(htmlFiles)
                .pipe(dest('./dist'))
        }

};

exports.js = fn;
