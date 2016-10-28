namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;

    internal static class ChannelHandlerFactory
    {
        public static IChannelHandler CreateChannelHandler(string channelId)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(channelId, "facebook"))
            {
                return new FacebookChannelHandler();
            }

            return new DefaultChannelHandler();
        }
    }
}
