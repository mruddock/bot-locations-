namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Bing;
    using Connector;
    using ConnectorEx;
    using Dialogs;
    using Newtonsoft.Json.Linq;

    [Serializable]
    internal class FacebookLocationDialog : IDialog<Place>
    {
        private readonly string prompt;
        private readonly bool reverseGeocode;

        public FacebookLocationDialog(string prompt, bool reverseGeocode)
        {
            this.prompt = prompt;
            this.reverseGeocode = reverseGeocode;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.ChannelData = new FacebookMessage
            (
                text: this.prompt,
                quickReplies: new List<FacebookQuickReply>
                {
                        new FacebookQuickReply(
                            contentType: FacebookQuickReply.ContentTypes.Location,
                            title: default(string),
                            payload: default(string)
                        )
                }
            );

            await context.PostAsync(reply);

            context.Wait(this.LocationReceivedAsync);
        }

        private async Task LocationReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var msg = await argument;

            try
            {
                var attachement = msg.ChannelData?.message?.attachments;
                if (attachement != null)
                {
                    JArray array = JArray.FromObject(attachement);
                    if (array.Count > 0)
                    {
                        Uri myUri = new Uri(array[0]["url"].ToString());
                        string param1 = HttpUtility.ParseQueryString(myUri.Query).Get("u");
                        myUri = new Uri(param1);
                        param1 = HttpUtility.ParseQueryString(myUri.Query).Get("where1");
                        await context.PostAsync("Address: " + HttpUtility.UrlDecode(param1));
                    }
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.ToString());
            }

            var location = msg.Entities?.Where(t => t.Type == "Place").Select(t => t.GetAs<Place>()).FirstOrDefault();

            if (location != null && location.Geo.latitude != null && location.Geo.longitude != null && this.reverseGeocode)
            {
                var response =
                    await new BingGeoSpatialService().GetLocationsByPointAsync((double)location.Geo.latitude, (double)location.Geo.longitude);

                location.Address = response?.Locations?.FirstOrDefault()?.Address.FormattedAddress;
            }

            context.Done(location);
        }
    }
}
