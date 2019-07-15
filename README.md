Telegram Bot Message Sender
===
ASP.<span></span>NET Core 2.x application that receives messages via post requests and sends them to the telegram via Bot API

### Note ###
 Startup project is _TelegramBotMessageSender_

 Post request with message:  
   - Request endpoint: `http://<host>:<port>/api/Message/SendMessage`  
   - Request body: `{channelName: "ChannelName", message:"TestMessage"}`  (see [Config section](#Config))
   - Request Header parameter `Content-Type:	application/json`

 
Config
---
_appsettings.json_  - main config and it's requeired  
_nlog.config_  - logging config and it's required too

### Nlog.config ###
By default it's configured to write Logs to the Console with minimum level "Info" and write all logs to the file.  
More information about nlog can be found in [NLog wiki](https://github.com/NLog/NLog/wiki)

### appsettings.json ###
Config can be changed in real time. Changes will be applied on the next incomming message

File structure:  
~~~
 "telegramConfig": {
    "useSocks5Proxy": true,    
    "botToken": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "channels": [
      {
        "channelName": "ChannelName1",
        "channelId": "-000000001"
      },
      {
        "channelName": "ChannelName2",
        "channelId": "-000000002"
      }
    ]
  },
  "socks5Config": {
    "host": "<ip>",
    "port": "<port>",
    "useAuthentication": true,
    "username": "admin",
    "password": "password"
  },
~~~
  * `botToken` - telegram bot token. It's can be obtained via @BotFather.   
More information: https://core.telegram.org/bots

  * `channels` - array of `channelName` - `channelId` mappings.  

    * `channelName` - fictional "channel name" that comes with a post request and maps to the telegram channel id  
    * `channelId` - telegram group/channel id to which the bot will send messages  
     In order to get the telegram group/channel id:
      * Add the telegram Bot to the group
      * Get the list of updtaes for your Bot: `https://api.telegram.org/bot<YourBOTToken>/getUpdates`. 
      * More information: https://stackoverflow.com/questions/32423837

  * `socks5Config` - just socks5 proxy settings, in case the telegram API is blocked  

