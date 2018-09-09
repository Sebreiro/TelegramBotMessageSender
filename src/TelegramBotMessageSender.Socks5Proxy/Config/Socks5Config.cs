using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotMessageSender.Socks5Proxy.Config
{
    public class Socks5Config
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
