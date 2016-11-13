"use strict";
var path = require('path');
var botbuilder_1 = require('botbuilder');
var defaultLocationDialog = require('./dialogs/default-location-dialog');
var facebookLocationDialog = require('./dialogs/facebook-location-dialog');
var requiredFieldsDialog = require('./dialogs/required-fields-dialog');
exports.LocationRequiredFields = requiredFieldsDialog.LocationRequiredFields;
var lib = new botbuilder_1.Library('botbuilder-location');
requiredFieldsDialog.register(lib);
defaultLocationDialog.register(lib);
facebookLocationDialog.register(lib);
lib.dialog('locationPickerPrompt', getLocationPickerPrompt());
lib.localePath(path.join(__dirname, 'locale/'));
exports.create = function (bot) {
    bot.library(lib);
};
exports.getLocation = function (session, options) {
    session.beginDialog('botbuilder-location:locationPickerPrompt', options);
};
function getLocationPickerPrompt() {
    return [
        function (session, args) {
            session.dialogData.args = args;
            if (args.useNativeControl && session.message.address.channelId == 'facebook') {
                session.beginDialog('facebook-location-dialog', args);
            }
            else {
                session.beginDialog('default-location-dialog', args);
            }
        },
        function (session, results, next) {
            if (results.response && results.response.place) {
                session.beginDialog('required-fields-dialog', { place: results.response.place, requiredFields: session.dialogData.args.requiredFields });
            }
            else {
                next(results);
            }
        },
        function (session, results, next) {
            if (results.response && results.response.reset) {
                session.replaceDialog('locationPickerPrompt', session.dialogData.args);
            }
            else {
                next({ response: results.response.place });
            }
        }
    ];
}
