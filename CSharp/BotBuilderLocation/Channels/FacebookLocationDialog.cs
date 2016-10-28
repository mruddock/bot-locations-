namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Connector;
    using ConnectorEx;
    using Dialogs;

    [Serializable]
    internal class FacebookLocationDialog : IDialog<Place>
    {
        private readonly string prompt;

        public FacebookLocationDialog(string prompt)
        {
            this.prompt = prompt;
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

            var place = msg.Entities?.Where(t => t.Type == "Place").Select(t => t.GetAs<Place>()).FirstOrDefault();
            
            context.Done(place);
        }
    }
}
