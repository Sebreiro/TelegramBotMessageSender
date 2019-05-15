using System.Collections.Generic;

namespace TelegramBotMessageSender.Services.Config
{
    public class TelegramConfig
    {
        public bool UseSocks5Proxy { get; set; }
        public string BotToken { get; set; }
        public List<TelegramChannelConfig> Channels { get; set; }
    }
}
