import {Session, IntentDialog} from 'botbuilder';
import {Place, Geo} from './place';

export function createBaseDialog(options?: any): IntentDialog {
    return new IntentDialog(options)
        .matches(/help/i, function(session: Session) {
            session.send("help message");
        })
        .matches(/^cancel$/i, function(session: Session) {
            session.endDialogWithResult({ response: {cancel: true} });
            return;
        })
        .matches(/^reset$/i, function(session: Session) {
            session.endDialogWithResult({ response: {reset: true} });
            return;
        });
}

export function processLocation(location: any): Place {
    var place: Place = new Place();
    place.type = location.entityType;
    place.name = location.name;

    if (location.address) {
        place.formattedAddress = location.address.formattedAddress;
        place.country = location.address.countryRegion;
        place.locality = location.address.locality;
        place.postalCode = location.address.postalCode;
        place.region = location.address.adminDistrict;
        place.streetAddress = location.address.addressLine;
    }

    if (location.point && location.point.coordinates && location.point.coordinates.length == 2) {
        place.geo = new Geo();
        place.geo.latitude = location.point.coordinates[0];
        place.geo.longitude = location.point.coordinates[1];
    }

    return place;
}