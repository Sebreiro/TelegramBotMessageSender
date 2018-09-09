using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TelegramBotMessageSender.Services.Config;
using TelegramBotMessageSender.Socks5Proxy;

namespace TelegramBotMessageSender.Services
{
    public class SenderService : ISenderService
    {
        private readonly ILogger<SenderService> _logger;
        private readonly TelegramConfig _config;
        private readonly IProxyService _proxyService;

        private string _apiUrl = "https://api.telegram.org";
        private string _apiSendMessagePathTemplate = "bot{0}/sendMessage?chat_id={1}&text={2}";

        public SenderService(IOptionsSnapshot<TelegramConfig> config, ILogger<SenderService> logger, IProxyService proxyService)
        {
            _logger = logger;
            _proxyService = proxyService;

            _config = config.Value;

            CheckConfig(_config);
        }

        public async Task<HttpResponseMessage> SendMessage(string message)
        {
            if (message == null)
                throw new ArgumentException("Message is null");

            var finalUrl = GetFinalUrl(message);

            var client = GetHttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, finalUrl);

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(requestMessage);

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

        private string GetFinalUrl(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is null");

            var fullUrl = $"{_apiUrl}/{_apiSendMessagePathTemplate}";
            var finalUrl = string.Format(fullUrl, _config.BotToken, _config.ChannelId, message);

            return finalUrl;
        }

        private bool CheckConfig(TelegramConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.BotToken))
                throw new InvalidOperationException("Config BotToken is missing");
            if (string.IsNullOrWhiteSpace(config.ChannelId))
                throw new InvalidOperationException("Config ChannelId is missing");

            return true;
        }
    }
}
