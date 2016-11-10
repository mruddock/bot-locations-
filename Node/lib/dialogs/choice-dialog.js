"use strict";
var common = require('../common');
function register(library) {
    library.dialog('choice-dialog', createDialog());
}
exports.register = register;
function createDialog() {
    return common.createBaseDialog()
        .onBegin(function (session, args) {
        session.dialogData.locations = args.locations;
        session.send("MultipleResultsFound");
    })
        .onDefault(function (session) {
        var numberExp = /[+-]?(?:\d+\.?\d*|\d*\.?\d+)/;
        var match = numberExp.exec(session.message.text);
        if (match) {
            var currentNumber = Number(match[0]);
            if (currentNumber > 0 && currentNumber <= session.dialogData.locations.length) {
                var place = common.processLocation(session.dialogData.locations[currentNumber - 1]);
                session.endDialogWithResult({ response: { place: place } });
                return;
            }
        }
        session.send("InvalidLocationResponse");
    });
}
