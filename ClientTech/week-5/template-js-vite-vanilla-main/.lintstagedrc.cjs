module.exports = {
  "*.js": ["prettier -w --list-different", "eslint --fix"],
  "!*.js": "prettier -w --list-different",
};
