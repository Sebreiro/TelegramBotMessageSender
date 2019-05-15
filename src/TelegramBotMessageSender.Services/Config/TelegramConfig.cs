using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotMessageSender.Services.Config
{
    public class TelegramConfig
    {
        public bool UseSocks5Proxy { get; set; } = false;

        [Required]
        public string BotToken { get; set; }

        public List<TelegramChannelConfig> Channels { get; set; }
    }
}
