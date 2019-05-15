using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TelegramBotMessageSender.Services.Config;
using TelegramBotMessageSender.Socks5Proxy;

namespace TelegramBotMessageSender.Services
{
    public class SenderService : ISenderService
    {
        private readonly ILogger<SenderService> _logger;
        private readonly TelegramConfig _config;
        private readonly IProxyService _proxyService;
        private readonly IChannelConfigService _channelConfigService;

        private string _apiUrl = "https://api.telegram.org";
        private string _apiSendMessagePathTemplate = "bot{0}/sendMessage";

        public SenderService(
            IOptionsSnapshot<TelegramConfig> config, 
            ILogger<SenderService> logger, 
            IProxyService proxyService, 
            IChannelConfigService channelConfigService)
        {
            _logger = logger;
            _proxyService = proxyService;
            _channelConfigService = channelConfigService;

            _config = config.Value;

            //CheckConfig(_config);
        }

        public async Task<HttpResponseMessage> SendMessage(string channelName, string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (channelName == null)
                throw new ArgumentNullException(nameof(channelName));

            var channelId = _channelConfigService.GetChannelId(channelName);
            if (string.IsNullOrWhiteSpace(channelId))
                throw new InvalidOperationException($"Config ChannelId with ChannelName {channelName} is missing");

            var finalUrl = GetFinalUrl();

            var client = GetHttpClient();

            HttpResponseMessage response;
            try
            {
                var content = CreateBodyContent(channelId, message);

                response = await client.PostAsync(finalUrl, content);

                if (response.StatusCode != HttpStatusCode.OK)
                    _logger.LogError($"{response}");

                _logger.LogInformation($"Message Sent: {message}");

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                throw;
            }
            
            return response;
        }

        private HttpClient GetHttpClient()
        {
            if (_config.UseSocks5Proxy)
            {
                var proxy = _proxyService.GetProxy();

                if (proxy != null) return CreateHttpClient(proxy);

                _logger.LogError("Proxy is not valid. Using without proxy");
                return CreateHttpClient();
            }
            return CreateHttpClient();
        }

        private HttpClient CreateHttpClient(IWebProxy proxy = null)
        {
            if (proxy == null)
                return new HttpClient();

            _logger.LogDebug("Using Proxy");
            return new HttpClient(new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = proxy
            });
        }

        private StringContent CreateBodyContent(string channelId, string message)
        {
            var messageBodyObj = new
            {
                chat_id = channelId,
                text = message
            };
            var messageBodyJson = JsonConvert.SerializeObject(messageBodyObj);

            var content = new StringContent(messageBodyJson, Encoding.UTF8, "application/json");

            return content;
        }

        private string GetFinalUrl()
        {
            var templateUrl = $"{_apiUrl}/{_apiSendMessagePathTemplate}";
            var finalUrl = string.Format(templateUrl, _config.BotToken);
            return finalUrl;
        }

        //private bool CheckConfig(TelegramConfig config)
        //{
        //    if (string.IsNullOrWhiteSpace(config.BotToken))
        //        throw new InvalidOperationException("Config BotToken is missing");
        //    if (string.IsNullOrWhiteSpace(config.ChannelId))
        //        throw new InvalidOperationException("Config ChannelId is missing");

        //    return true;
        //}
    }
}
