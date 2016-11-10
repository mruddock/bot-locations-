"use strict";
var common = require('../common');
function register(library) {
    library.dialog('confirm-dialog', createDialog());
}
exports.register = register;
function createDialog() {
    return common.createBaseDialog()
        .onBegin(function (session, args) {
        session.dialogData.locations = args.locations;
        session.send("SingleResultFound");
    })
        .onDefault(function (session) {
        var message = parseBoolean(session.message.text);
        if (typeof message == 'boolean') {
            var place = message == true ? common.processLocation(session.dialogData.locations[0]) : null;
            session.endDialogWithResult({ response: { place: place } });
            return;
        }
        session.send("InvalidLocationResponse");
    });
}
function parseBoolean(input) {
    input = input.trim();
    var yesExp = /^(y|yes|yep|sure|ok|true)/i;
    var noExp = /^(n|no|nope|not|false)/i;
    if (yesExp.test(input)) {
        return true;
    }
    else if (noExp.test(input)) {
        return false;
    }
    return undefined;
}
