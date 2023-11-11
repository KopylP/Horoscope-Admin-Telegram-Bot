using Horoscope.Admin.Bot.Framework.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Messages;

public sealed class MessageFactory : IMessageFactory
{
    private readonly ITelegramBotClient _telegramBotClient;

    public MessageFactory(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public IMessage CreateStandardMessage(string message) => new StandardMessage(_telegramBotClient, message);
}