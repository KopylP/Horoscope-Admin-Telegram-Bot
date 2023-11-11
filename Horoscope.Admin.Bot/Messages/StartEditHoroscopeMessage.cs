using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Keyword;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public sealed class StartEditHoroscopeMessage : BaseMessage
{
    private const string Message = $"Як будеш готовий, тисни \"{ReplyCommands.StartingEdit.Start}\"";
    
    public StartEditHoroscopeMessage(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task SendAsync()
    {
        var keyboard = ReplyKeyboardMarkupBuilder.Create()
            .AddRow(new KeyboardButton(ReplyCommands.StartingEdit.Start))
            .Build();
        
        await BotClient.SendTextMessageAsync(ExecutionContext.GetChatId(), Message, replyMarkup: keyboard);
    }
}