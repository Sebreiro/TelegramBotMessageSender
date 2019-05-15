namespace TelegramBotMessageSender.Services
{
    public interface IChannelConfigService
    {
        string GetChannelId(string channelName);
    }
}