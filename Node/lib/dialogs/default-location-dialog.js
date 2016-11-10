"use strict";
var common = require('../common');
var botbuilder_1 = require('botbuilder');
var locationService = require('../services/bing-geospatial-service');
var confirmDialog = require('./confirm-dialog');
var choiceDialog = require('./choice-dialog');
function register(library) {
    confirmDialog.register(library);
    choiceDialog.register(library);
    library.dialog('default-location-dialog', createDialog());
    library.dialog('location-resolve-dialog', createLocationResolveDialog());
}
exports.register = register;
function createDialog() {
    return [
        function (session, args) {
            session.beginDialog('location-resolve-dialog', { prompt: args.prompt });
        },
        function (session, results, next) {
            session.dialogData.response = results.response;
            if (results.response && results.response.locations) {
                var locations = results.response.locations;
                if (locations.length == 1) {
                    session.beginDialog('confirm-dialog', { locations: locations });
                }
                else {
                    session.beginDialog('choice-dialog', { locations: locations });
                }
            }
            else {
                next(results);
            }
        }
    ];
}
var MAX_CARD_COUNT = 5;
function createLocationResolveDialog() {
    return common.createBaseDialog()
        .onBegin(function (session, args) {
        session.send(args.prompt);
    }).onDefault(function (session) {
        locationService.getLocationByQuery(session.message.text)
            .then(function (locations) {
            if (locations.length == 0) {
                session.send("LocationNotFound");
                return;
            }
            var locationCount = Math.min(MAX_CARD_COUNT, locations.length);
            locations = locations.slice(0, locationCount);
            var reply = createLocationsCard(session, locations);
            session.send(reply);
            session.endDialogWithResult({ response: { locations: locations } });
        });
    });
}
function createLocationsCard(session, locations) {
    var cards = new Array();
    for (var i = 0; i < locations.length; i++) {
        cards.push(constructCard(session, locations, i));
    }
    return new botbuilder_1.Message(session)
        .attachmentLayout(botbuilder_1.AttachmentLayout.carousel)
        .attachments(cards);
}
function constructCard(session, locations, index) {
    var location = locations[index];
    var indexText = locations.length > 1 ? (index + 1) + ". " : "";
    var text = indexText + location.address.formattedAddress;
    var card = new botbuilder_1.HeroCard(session)
        .subtitle(text);
    if (location.point) {
        card.images([botbuilder_1.CardImage.create(session, locationService.GetLocationMapImageUrl(location, index))]);
    }
    return card;
}
