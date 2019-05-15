using Autofac;
using TelegramBotMessageSender.Services;
using TelegramBotMessageSender.Socks5Proxy;

namespace TelegramBotMessageSender.Initialization
{
    public class ContainerConfiguration
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<SenderService>().As<ISenderService>();
            builder.RegisterType<ProxyService>().As<IProxyService>();
            builder.RegisterType<ChannelConfigService>().As<IChannelConfigService>();
        }
    }
}
