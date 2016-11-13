import * as common from '../common';
import { Session, IDialogResult, Library, AttachmentLayout, HeroCard, CardImage, Message } from 'botbuilder';
import { Place } from '../Place';

export function register(library: Library): void {
    library.dialog('facebook-location-dialog', createDialog());
    library.dialog('facebook-location-resolve-dialog', createLocationResolveDialog());
}

function createDialog() {
    return [
        (session: Session, args: any) => {
            session.beginDialog('facebook-location-resolve-dialog', { prompt: args.prompt });
        }
    ];
}

function createLocationResolveDialog() {
    return common.createBaseDialog()
        .onBegin(function(session, args) {
            session.dialogData.args = args;
            sendLocationPrompt(session);
        }).onDefault((session) => {
            var entities = session.message.entities;
            for (var i = 0; i < entities.length; i++) {
                if (entities[i].type == "Place" && entities[i].geo && entities[i].geo.latitude && entities[i].geo.longitude) {
                    session.endDialogWithResult({ response: { place: common.buildPlaceFromGeo(entities[i].geo.latitude, entities[i].geo.longitude) } });
                    return;
                }
            }

            sendLocationPrompt(session);
        });
}

function sendLocationPrompt(session: Session) {
    var message = new Message(session).text(session.dialogData.args.prompt).sourceEvent({
        facebook: {
            quick_replies: [
                {
                    content_type: "location"
                }
            ]
        }
    });

    session.send(message);
}