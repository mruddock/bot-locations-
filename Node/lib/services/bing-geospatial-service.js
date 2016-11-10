"use strict";
var rp = require('request-promise');
var sprintf_js_1 = require('sprintf-js');
var apiKey = process.env.BING_MAPS_API_KEY;
var findLocationByQueryUrl = "https://dev.virtualearth.net/REST/v1/Locations?key=" + apiKey + "&q=";
var findImageByPointUrl = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/%1$s,%2$s/15?mapSize=500,500&pp=%1$s,%2$s;1;%3$s&dpi=1&key=" + apiKey;
var findImageByBBoxUrl = "https://dev.virtualearth.net/REST/V1/Imagery/Map/Road?mapArea=%1$s,%2$s,%3$s,%4$s&mapSize=500,500&pp=%5$s,%6$s;1;%7$s&dpi=1&key=" + apiKey;
function getLocationByQuery(address) {
    var requestData = {
        url: findLocationByQueryUrl + encodeURIComponent(address),
        json: true
    };
    return rp(requestData)
        .then(function (body) {
        if (body && body.resourceSets && body.resourceSets[0] && body.resourceSets[0].resources) {
            return body.resourceSets[0].resources;
        }
        else {
            throw ("Invalid Api Response");
        }
    });
}
exports.getLocationByQuery = getLocationByQuery;
function GetLocationMapImageUrl(location, index) {
    if (location && location.point && location.point.coordinates && location.point.coordinates.length == 2) {
        var point = location.point;
        var url;
        if (location.bbox && location.bbox.length == 4) {
            url = sprintf_js_1.sprintf(findImageByBBoxUrl, location.bbox[0], location.bbox[1], location.bbox[2], location.bbox[3], point.coordinates[0], point.coordinates[1], index);
        }
        else {
            url = sprintf_js_1.sprintf(findImageByPointUrl, point.coordinates[0], point.coordinates[1], index);
        }
        return url;
    }
    throw location;
}
exports.GetLocationMapImageUrl = GetLocationMapImageUrl;
