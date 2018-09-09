using System.Net;

namespace TelegramBotMessageSender.Socks5Proxy
{
    public interface IProxyService
    {
        IWebProxy GetProxy();
    }
}