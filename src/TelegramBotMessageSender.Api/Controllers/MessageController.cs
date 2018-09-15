﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TelegramBotMessageSender.Services;
using TelegramBotMessageSender.WebApi.Params;

namespace TelegramBotMessageSender.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly ISenderService _senderService;

        public MessageController(ILogger<MessageController> logger, ISenderService senderService)
        {
            _logger = logger;
            _senderService = senderService;
        }

        /// <summary>
        /// Send Message with waiting for telegram Response
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageParam param)
        {
            if (param == null)
                throw new ArgumentException($"parameter {nameof(param)} is null");

            var result = await _senderService.SendMessage(param.Message);
            if (result == null)
            {
                var errorMessage = "Something bad happened. SendMessage Result is null";
                _logger.LogCritical(errorMessage);
                return new ObjectResult(errorMessage)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return new ObjectResult($"Telegram: {result.ReasonPhrase}")
            {
                StatusCode=(int)result.StatusCode,
            };
        }

        /// <summary>
        /// Send Message without waiting for telegram response
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IActionResult SendMessageAsync([FromBody] SendMessageParam param)
        {
            if (param == null)
                throw new ArgumentException($"parameter {nameof(param)} is null");

            _senderService.SendMessage(param.Message);

            return Ok();
        }
    }
}