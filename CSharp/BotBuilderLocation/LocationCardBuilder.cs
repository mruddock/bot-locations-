namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bing;
    using Builder.Dialogs;
    using Connector;
    using ConnectorEx;
    using Internals.Fibers;

    /// <summary>
    /// A class for creating location cards.
    /// </summary>
    [Serializable]
    public class LocationCardBuilder : ILocationCardBuilder
    {
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationCardBuilder"/> class.
        /// </summary>
        /// <param name="apiKey">The geo spatial API key.</param>
        public LocationCardBuilder(string apiKey)
        {
            SetField.NotNull(out this.apiKey, nameof(apiKey), apiKey);
        }

        /// <summary>
        /// Creates locations hero cards.
        /// </summary>
        /// <param name="locations">List of the locations.</param>
        /// <param name="alwaysShowNumericPrefix">Indicates whether a list containing exactly one location should have a '1.' prefix in its label.</param>
        /// <returns>The locations card as a list.</returns>
        public IEnumerable<HeroCard> CreateHeroCards(IList<Location> locations, bool alwaysShowNumericPrefix = false)
        {
            var cards = new List<HeroCard>();

            int i = 1;

            foreach (var location in locations)
            {
                string address = alwaysShowNumericPrefix || locations.Count > 1 ? $"{i}. {location.Address.FormattedAddress}" : location.Address.FormattedAddress;

                var heroCard = new HeroCard
                {
                    Subtitle = address
                };

                if (location.Point != null)
                {
                    var image =
                        new CardImage(
                            url: new BingGeoSpatialService(this.apiKey).GetLocationMapImageUrl(location, i));

                    heroCard.Images = new[] { image };
                }

                cards.Add(heroCard);

                i++;
            }

            return cards;
        }

        /// <summary>
        /// Creates a location keyboard card (buttons) with numbers and/or additional labels.
        /// </summary>
        /// <param name="selectText">The card prompt.</param>
        /// <param name="optionCount">The number of options for which buttons should be made.</param>
        /// <param name="additionalLabels">Additional buttons labels.</param>
        /// <returns>The keyboard card.</returns>
        public KeyboardCard CreateKeyboardCard(string selectText, int optionCount = 0, params string[] additionalLabels)
        {
            var combinedLabels = new List<string>();
            combinedLabels.AddRange(Enumerable.Range(1, optionCount).Select(i => i.ToString()));
            combinedLabels.AddRange(additionalLabels);

            var buttons = new List<CardAction>();

            foreach (var label in combinedLabels)
            {
                buttons.Add(new CardAction
                {
                    Type = "imBack",
                    Title = label,
                    Value = label
                });
            }

            return new KeyboardCard(selectText, buttons);
        }
    }
}
