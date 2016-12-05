import * as path from 'path';
import { Library, Session, IDialogResult } from 'botbuilder';
import * as common from './common';
import * as consts from './consts';
import { Place } from './place';
import * as defaultLocationDialog from './dialogs/default-location-dialog';
import * as facebookLocationDialog from './dialogs/facebook-location-dialog'
import * as requiredFieldsDialog from './dialogs/required-fields-dialog';

export interface ILocationPromptOptions {
    prompt: string;
    requiredFields?: requiredFieldsDialog.LocationRequiredFields;
    useNativeControl?: boolean,
    reverseGeocode?: boolean
}

exports.LocationRequiredFields = requiredFieldsDialog.LocationRequiredFields

//=========================================================
// Library creation
//=========================================================


exports.createLibrary = (apiKey: string) => {
    if(apiKey === undefined) {
        throw "'apiKey' parameter missing";
    }

    var lib = new Library(consts.LibraryName);

    requiredFieldsDialog.register(lib);
    defaultLocationDialog.register(lib, apiKey);
    facebookLocationDialog.register(lib, apiKey);
    lib.localePath(path.join(__dirname, 'locale/'));

    lib.dialog('locationPickerPrompt', getLocationPickerPrompt())
        .cancelAction('cancel', null, {
            matches: /^cancel$/i,
        });

    return lib;
}

//=========================================================
// Location Picker Prompt
//=========================================================

exports.getLocation = function (session: Session, options: ILocationPromptOptions) {
    session.beginDialog(consts.LibraryName + ':locationPickerPrompt', options);
};

function getLocationPickerPrompt() {
    return [
        (session: Session, args: ILocationPromptOptions) => {
            session.dialogData.args = args;
            if (args.useNativeControl && session.message.address.channelId == 'facebook') {
                session.beginDialog('facebook-location-dialog', args);
            }
            else {
                session.beginDialog('default-location-dialog', args);
            }
        },
        (session: Session, results: IDialogResult<any>, next: (results?: IDialogResult<any>) => void) => {
            if (results.response && results.response.place) {
                session.beginDialog('required-fields-dialog', {
                    place: results.response.place,
                    requiredFields: session.dialogData.args.requiredFields
                })
            } else {
                next(results);
            }
        },
        (session: Session, results: IDialogResult<any>, next: (results?: IDialogResult<any>) => void) => {
            if (results.response && results.response.reset) {
                session.replaceDialog('locationPickerPrompt', session.dialogData.args);
            } else {
                next({ response: results.response.place });
            }
        }
    ];
}