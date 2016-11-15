"use strict";
var common = require('../common');
var consts_1 = require('../consts');
var botbuilder_1 = require('botbuilder');
(function (LocationRequiredFields) {
    LocationRequiredFields[LocationRequiredFields["none"] = 0] = "none";
    LocationRequiredFields[LocationRequiredFields["streetAddress"] = 1] = "streetAddress";
    LocationRequiredFields[LocationRequiredFields["locality"] = 2] = "locality";
    LocationRequiredFields[LocationRequiredFields["region"] = 4] = "region";
    LocationRequiredFields[LocationRequiredFields["country"] = 8] = "country";
    LocationRequiredFields[LocationRequiredFields["postalCode"] = 16] = "postalCode";
})(exports.LocationRequiredFields || (exports.LocationRequiredFields = {}));
var LocationRequiredFields = exports.LocationRequiredFields;
function register(library) {
    library.dialog('required-fields-dialog', createDialog());
}
exports.register = register;
var fields = [
    { name: "streetAddress", prompt: consts_1.Strings.AskForStreetAddress, flag: LocationRequiredFields.streetAddress },
    { name: "locality", prompt: consts_1.Strings.AskForLocality, flag: LocationRequiredFields.locality },
    { name: "region", prompt: consts_1.Strings.AskForRegion, flag: LocationRequiredFields.region },
    { name: "country", prompt: consts_1.Strings.AskForCountry, flag: LocationRequiredFields.country },
    { name: "postalCode", prompt: consts_1.Strings.AskForPostalCode, flag: LocationRequiredFields.postalCode },
];
function createDialog() {
    return common.createBaseDialog({ recognizeMode: botbuilder_1.RecognizeMode.onBegin })
        .onBegin(function (session, args, next) {
        if (args.requiredFields) {
            session.dialogData.place = args.place;
            session.dialogData.index = -1;
            session.dialogData.requiredFieldsFlag = args.requiredFields;
            next();
        }
        else {
            session.endDialogWithResult({ response: args.place });
        }
    })
        .onDefault(function (session) {
        var index = session.dialogData.index;
        if (index >= 0) {
            if (!session.message.text) {
                return;
            }
            session.dialogData.place[fields[index].name] = session.message.text;
        }
        index++;
        while (index < fields.length) {
            if (completeFieldIfMissing(session, fields[index])) {
                break;
            }
            index++;
        }
        session.dialogData.index = index;
        if (index >= fields.length) {
            session.endDialogWithResult({ response: { place: session.dialogData.place } });
        }
    });
}
function completeFieldIfMissing(session, field) {
    if ((field.flag & session.dialogData.requiredFieldsFlag) && !session.dialogData.place[field.name]) {
        session.send(field.prompt);
        return true;
    }
    return false;
}
