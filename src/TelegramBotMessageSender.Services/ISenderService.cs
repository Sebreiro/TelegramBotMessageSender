using System.Net.Http;
using System.Threading.Tasks;

namespace TelegramBotMessageSender.Services
{
    public interface ISenderService
    {
        Task<HttpResponseMessage> SendMessage(string channelName, string message);
    }
}
