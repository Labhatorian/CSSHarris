const {series, parallel, watch} = require('gulp');
const browserSync = require('browser-sync').create();
const config = require('./config');

const css = require('./tasks/css').css(config.files.css);
css.displayName = 'css';

const html = require('./tasks/html').js(config.files.html);
html.displayName = 'html';

const image = require('./tasks/image').image(config.files.image);
image.displayName = 'image';

const js = require('./tasks/js').js(config.files.js, config.files.jsOrder);
js.displayName = 'js';

const json = require('./tasks/json').json(config.files.json);
json.displayName = 'json';

exports.css = css;
exports.html = html;
exports.js = js;
exports.build = parallel(html, js, css, image, json);
