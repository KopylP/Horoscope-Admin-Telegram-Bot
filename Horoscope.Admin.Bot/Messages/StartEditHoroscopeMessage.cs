using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Keyword;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public sealed class StartEditHoroscopeMessage : BaseMessage
{
    private const string Message = $"Як будеш готовий, тисни \"{ReplyCommands.BeginningHoroscopeEdit.Begin}\"";
    
    public StartEditHoroscopeMessage(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task SendAsync()
    {
        var keyboard = ReplyKeyboardMarkupBuilder.Create()
            .AddRow(new KeyboardButton(ReplyCommands.BeginningHoroscopeEdit.Begin))
            .Build();
        
        await BotClient.SendTextMessageAsync(ExecutionContext.ChatId, Message, replyMarkup: keyboard);
    }
}