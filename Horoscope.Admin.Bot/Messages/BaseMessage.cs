using Horoscope.Admin.Bot.Framework.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Messages;

public abstract class BaseMessage : IMessage
{
    protected readonly ITelegramBotClient BotClient;

    protected BaseMessage(ITelegramBotClient botClient)
    {
        BotClient = botClient;
    }

    public abstract Task SendAsync();
}