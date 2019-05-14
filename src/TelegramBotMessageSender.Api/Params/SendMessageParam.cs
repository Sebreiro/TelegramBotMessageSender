using System.ComponentModel.DataAnnotations;

namespace TelegramBotMessageSender.WebApi.Params
{
    public class SendMessageParam
    {
        [Required(ErrorMessage = "ChannelName is required")]
        public string ChannelName { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
