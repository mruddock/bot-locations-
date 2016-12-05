import * as common from '../common';
import { Strings } from '../consts';
import { Session, RecognizeMode, Library } from 'botbuilder';

export enum LocationRequiredFields {
    none = 0,
    streetAddress = 1 << 0,
    locality = 1 << 1,
    region = 1 << 2,
    country = 1 << 3,
    postalCode = 1 << 4
}

export function register(library: Library): void {
    library.dialog('required-fields-dialog', createDialog());
}

const fields: Array<any> = [
    { name: "streetAddress", prompt: Strings.AskForStreetAddress, flag: LocationRequiredFields.streetAddress },
    { name: "locality", prompt: Strings.AskForLocality, flag: LocationRequiredFields.locality },
    { name: "region", prompt: Strings.AskForRegion, flag: LocationRequiredFields.region },
    { name: "country", prompt: Strings.AskForCountry, flag: LocationRequiredFields.country },
    { name: "postalCode", prompt: Strings.AskForPostalCode, flag: LocationRequiredFields.postalCode },
];

function createDialog() {
    return common.createBaseDialog({ recognizeMode: RecognizeMode.onBegin })
        .onBegin((session, args, next) => {
            if (args.requiredFields) {
                session.dialogData.place = args.place;
                session.dialogData.index = -1;
                session.dialogData.requiredFieldsFlag = args.requiredFields;
                next();
            } else {
                session.endDialogWithResult({ response: args.place });
            }
        })
        .onDefault((session) => {
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
            } else {
                session.sendBatch();
            }
        });
}

function completeFieldIfMissing(session: Session, field: any) {
    if ((field.flag & session.dialogData.requiredFieldsFlag) && !session.dialogData.place[field.name]) {
        session.send(field.prompt);
        return true;
    }

    return false;
}