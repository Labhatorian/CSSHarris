const app = require("./index.jsx");
const port = process.env.PORT || 8080;

typeof app.listen === 'function' && app.listen(port, () => {
  console.log("Listening on " + port);
});
