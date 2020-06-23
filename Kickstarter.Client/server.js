var fs = require("fs");
var path = require("path");
var https = require("https");
var express = require("express");

var privateKey = fs.readFileSync(__dirname + "/key.pem", "utf8");
var certificate = fs.readFileSync(__dirname + "/cert.pem", "utf8");

var credentials = { key: privateKey, cert: certificate };
var app = express();

app.set("port", process.env.PORT || 3000);

app.use(function(req, res, next) {
    if (path.extname(req.path).length > 0) {
        next();
    } 
    else if (path.dirname(req.path).indexOf("silent_renew") > -1) {
        req.url = "/silent_renew.html";
        next();
    } 
    else {
        req.url = "/index.html";
        next();
    }
});

app.use(express.static(__dirname + "/dist"))
    .get("/", function(req, res) {
        res.sendFile("index.html", {
        root: __dirname + "/dist"
        });
    })
    .get("/silent_renew.html", function(req, res) {
        res.sendFile("silent_renew.html", {
        root: __dirname + "/dist"
        });
    });

var httpsServer = https.createServer(credentials, app);

httpsServer.listen(app.get("port"), function() {
    let port = app.get('port');
    let url = `https://localhost:${port}/`;
    console.log("The server is listening on port", port, url);
});
