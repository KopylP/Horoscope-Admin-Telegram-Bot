using Telegram.Bot;

namespace Horoscope.Admin.Bot.Messages;

public sealed class ProvideApiKeyMessage : BaseMessage
{
    private const string Message = "Привіт, друже, для продовження введи Api Key, " +
                                  "щоб я міг тобі довіряти :)";
    
    public ProvideApiKeyMessage(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.GetChatId();
        await BotClient.SendTextMessageAsync(chatId, Message);
    }
}