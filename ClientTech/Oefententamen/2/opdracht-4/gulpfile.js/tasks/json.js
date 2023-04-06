const {src, dest} = require('gulp');

const json = function (filesJSON) {
    return function () {

//student uitwerking
        //json taak werkend maken
        return src(filesJSON)

//student uitwerking
                //javascript taak werkend maken

                .pipe(dest('./dist/assets/memes/'));
    }
};

exports.json = json;
