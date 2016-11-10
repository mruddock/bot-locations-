
// This loads the environment variables from the .env file
require('dotenv-extended').load();

var builder = require('botbuilder');
var restify = require('restify');
var ld = require('./lib/location-dialog');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    console.log('%s listening to %s', server.name, server.url);
});

var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});
var bot = new builder.UniversalBot(connector);
server.post('/api/messages', connector.listen());

ld.create(bot);

bot.dialog("/", [
    function (session) {
        ld.getLocation(session, {
            prompt: "Hi, where would you like me to ship to your widget?",
            requiredFields: 
                ld.LocationRequiredFields.streetAddress |
                ld.LocationRequiredFields.locality |
                ld.LocationRequiredFields.region |
                ld.LocationRequiredFields.country |
                ld.LocationRequiredFields.postalCode
        });
    },
    function (session, results) {
        if (results.response) {
            console.log(results.response);
            session.send(JSON.stringify(results.response));
        }
        else {
            console.log("OK, I won't be shipping it");
            session.send("OK, I won't be shipping it");
        }
    }
]);
