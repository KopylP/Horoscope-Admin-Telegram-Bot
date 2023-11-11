using System.Globalization;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Messages.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public sealed class ProvideDateMessage: BaseMessage
{
    private const string Message = "Супер, введи дату у форматі [dd.mm.yyyy] або вибери зі списку нижче.";
    
    public ProvideDateMessage(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.GetChatId();
        await BotClient.SendTextMessageAsync(chatId, Message, replyMarkup: GenerateKeyboard());
    }

    private ReplyKeyboardMarkup GenerateKeyboard()
    {
        var keyboardBuilder = ReplyKeyboardMarkupBuilder.Create();
        
        GetNextWeekDates()
            .ToList()
            .ForEach(date => keyboardBuilder.AddRow(date));

        keyboardBuilder.AddBackRow();

        return keyboardBuilder.Build();
    }
    
    
    private IEnumerable<string> GetNextWeekDates()
    {
        DateTime today = DateTime.Today;

        for (int i = 0; i < 7; i++)
        {
            DateTime nextDay = today.AddDays(i);
            string formattedDate = nextDay.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            yield return formattedDate;
        }
    }
}