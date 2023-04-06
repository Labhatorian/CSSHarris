const {series, parallel, watch} = require("gulp");
const {src, dest} = require("gulp");
const browserSync = require("browser-sync").create();

const watchFiles = (opdrachtPath) => {
    return () => {
        browserSync.init({server:{baseDir: "./" + opdrachtPath}});

        watch("./**/*.html").on("change", browserSync.reload);
        watch("./**/*.js").on("change", browserSync.reload);
        watch("./**/*.css").on("change", browserSync.reload);
    }
}

exports.watch_opdracht_1 = watchFiles('opdracht-1');
exports.watch_opdracht_2 = watchFiles('opdracht-2');
exports.watch_opdracht_3 = watchFiles('opdracht-3');
exports.watch_opdracht_proef = watchFiles('opdracht-proef');
exports.watch_nakijken = watchFiles('./');

