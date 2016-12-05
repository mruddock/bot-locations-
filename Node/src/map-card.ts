import { Session, HeroCard, CardImage } from 'botbuilder';
import { Place, Geo } from './place';
import * as locationService from './services/bing-geospatial-service';

export class MapCard extends HeroCard {
    constructor(session?: Session) {
        super(session);
    }

    public location(location: any, index?: number): this {
        var indexText = "";
        if(index !== undefined) {
            indexText = (index + 1) + ". ";
        }

        this.text(indexText + location.address.formattedAddress)

        // Todo: pass this.session as a first parameter. https://github.com/Microsoft/BotBuilder/pull/1790
        if (location.point) {
            this.images([CardImage.create(null, locationService.GetLocationMapImageUrl(location, index))]);
        }
        return this;
    }
}