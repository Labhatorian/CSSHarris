const {src, dest} = require('gulp');

    const image = function (imageFiles) {
        return function () {
            return src(imageFiles)
                .pipe(dest('./dist/img'));
        }
};

exports.image = image;
