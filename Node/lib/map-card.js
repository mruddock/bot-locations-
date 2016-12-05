"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var botbuilder_1 = require('botbuilder');
var locationService = require('./services/bing-geospatial-service');
var MapCard = (function (_super) {
    __extends(MapCard, _super);
    function MapCard(session) {
        _super.call(this, session);
    }
    MapCard.prototype.location = function (location, index) {
        var indexText = "";
        if (index !== undefined) {
            indexText = (index + 1) + ". ";
        }
        this.text(indexText + location.address.formattedAddress);
        if (location.point) {
            this.images([botbuilder_1.CardImage.create(null, locationService.GetLocationMapImageUrl(location, index))]);
        }
        return this;
    };
    return MapCard;
}(botbuilder_1.HeroCard));
exports.MapCard = MapCard;
