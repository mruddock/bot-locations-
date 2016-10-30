using Microsoft.Bot.Builder.Location.Bing;

namespace Microsoft.Bot.Builder.Location
{
    using System.Collections.Generic;
    using System.Linq;
    using Connector;
    using ConnectorEx;

    internal static class AddressCard
    {
        internal static List<Attachment> CreateLocationsCard(IList<Bing.Location> locations)
        {
            var attachments = new List<Attachment>();

            int i = 1;

            foreach (var location in locations)
            {
                string address = locations.Count > 1 ? $"{i}. {location.Address.FormattedAddress}" : location.Address.FormattedAddress;

                var heroCard = new HeroCard
                {
                    Subtitle = address
                };

                if (location.Point != null)
                {
                    var image =
                        new CardImage(
                            url: new BingGeoSpatialService().GetLocationMapImageUrl(location, i));

                    heroCard.Images = new[] { image };
                }

                attachments.Add(heroCard.ToAttachment());

                i++;
            }

            return attachments;
        }

        internal static List<Attachment> CreateLocationsKeyboardCard(IEnumerable<Bing.Location> resources, string selectText)
        {
            int i = 1;
            var keyboardCard = new KeyboardCard(
                "Please select one",
                resources.Select(a => new CardAction
                {
                    Type = "imBack",
                    Title = i.ToString(),
                    Value = (i++).ToString()
                }).ToList());

            return new List<Attachment> { keyboardCard.ToAttachment() };
        }
    }
}
