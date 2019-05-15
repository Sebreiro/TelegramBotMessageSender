using System;
using System.Linq;
using Microsoft.Extensions.Options;
using TelegramBotMessageSender.Services.Config;

namespace TelegramBotMessageSender.Services
{
    public class ChannelConfigConfigService : IChannelConfigService
    {
        private readonly TelegramConfig _config;

        public ChannelConfigConfigService(IOptionsSnapshot<TelegramConfig> config)
        {
            _config = config.Value;
        }

        public string GetChannelId(string channelName)
        {
            return _config
                .Channels
                .FirstOrDefault(x => x.ChannelName.Equals(channelName, StringComparison.CurrentCultureIgnoreCase))
                ?.ChannelId;
        }
    }
}
