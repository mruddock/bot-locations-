import * as rp from 'request-promise';
import { sprintf } from 'sprintf-js';

const apiKey = process.env.BING_MAPS_API_KEY;
const findLocationByQueryUrl = "https://dev.virtualearth.net/REST/v1/Locations?key=" + apiKey + "&q=";
const findLocationByPointUrl = "https://dev.virtualearth.net/REST/v1/Locations/%1$s,%2$s?key=" + apiKey + "&q=";
const findImageByPointUrl = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/%1$s,%2$s/15?mapSize=500,500&pp=%1$s,%2$s;1;%3$s&dpi=1&key=" + apiKey;
const findImageByBBoxUrl = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road?mapArea=%1$s,%2$s,%3$s,%4$s&mapSize=500,500&pp=%5$s,%6$s;1;%7$s&dpi=1&key=" + apiKey;

export function getLocationByQuery(address: string): Promise<Array<any>> {
    var url = findLocationByQueryUrl + encodeURIComponent(address);
    return getLocation(url);
}

export function getLocationByPoint(latitude: string, longitude: string): Promise<Array<any>> {
    var url: string = sprintf(findLocationByPointUrl, latitude, longitude);
    return getLocation(url);
}

export function GetLocationMapImageUrl(location: any, index: number) {
    if (location && location.point && location.point.coordinates && location.point.coordinates.length == 2) {

        var point = location.point;
        var url: string;

        if (location.bbox && location.bbox.length == 4) {
            url = sprintf(findImageByBBoxUrl, location.bbox[0], location.bbox[1], location.bbox[2], location.bbox[3], point.coordinates[0], point.coordinates[1], index)
        }
        else {
            url = sprintf(findImageByPointUrl, point.coordinates[0], point.coordinates[1], index)
        }

        return url;
    }

    throw location;
}

function getLocation(url: string): Promise<Array<any>> {
    const requestData = {
        url: url,
        json: true
    };

    return rp(requestData)
        .then(body => {
            if (body && body.resourceSets && body.resourceSets[0] && body.resourceSets[0].resources) {
                return body.resourceSets[0].resources;
            } else {
                throw ("Invalid Api Response");
            }
        });
}