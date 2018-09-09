using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MihaZupan;
using TelegramBotMessageSender.Socks5Proxy.Config;

namespace TelegramBotMessageSender.Socks5Proxy
{
    public class ProxyService: IProxyService
    {
        private readonly ILogger _logger;
        private readonly Socks5Config _config;

        public ProxyService(ILogger<ProxyService> logger, IOptionsSnapshot<Socks5Config> config)
        {
            _logger = logger;
            var currentConfig = config.Value;

            _config = CheckConfig(currentConfig) 
                ? currentConfig 
                : null;
        }

        public IWebProxy GetProxy()
        {
            if (_config == null)
            {
                _logger.LogError("Socks5Proxy Config is not valid.");
                return null;
            }

            return _config.UseAuthentication 
                ? new HttpToSocks5Proxy(_config.Host, _config.Port, _config.Username, _config.Password) 
                : new HttpToSocks5Proxy(_config.Host, _config.Port);
        }

        private bool CheckConfig(Socks5Config config)
        {
            if (string.IsNullOrWhiteSpace(config.Host))
            {
                _logger.LogError("Proxy hostname is missing");
                return false;
            }

            if (config.Port == 0)
            {
                _logger.LogError("Proxy port is missing");
                return false;
            }

            if (config.UseAuthentication)
            {
                if (string.IsNullOrWhiteSpace(config.Username))
                {
                    _logger.LogError("Proxy username is not defined");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(config.Password))
                {
                    _logger.LogError("Proxy password is not defined");
                    return false;
                }
            }
            return true;
        }
    }
}
