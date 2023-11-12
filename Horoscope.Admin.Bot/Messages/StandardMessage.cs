using Telegram.Bot;

namespace Horoscope.Admin.Bot.Messages;

public sealed class StandardMessage : BaseMessage
{
    private readonly string _message;
    
    public StandardMessage(ITelegramBotClient botClient, string message) : base(botClient)
    {
        _message = message;
    }
    
    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.ChatId;
        await BotClient.SendTextMessageAsync(chatId, _message);
    }
}