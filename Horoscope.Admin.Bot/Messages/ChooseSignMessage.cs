using System.Text;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Helpers;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Infrastructure.Persistence;
using Horoscope.Admin.Bot.Messages.Extensions;
using Horoscope.Admin.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public sealed class ChooseSignMessage: BaseMessage
{
    private const string Message = $$"""Дата: *{0:{{DateFormats.DdMmYyyy}}}*. {{"\n\n"}}Tепер вибери знак зодіаку!""";

    private const string PublishedSignMessage =
        "*{0}*:\n\n{1}";
    
    private readonly FirestoreProvider _firestoreProvider;
    private readonly string _message;
    private readonly DateTime _date;
    
    public ChooseSignMessage(
        ITelegramBotClient botClient,
        FirestoreProvider firestoreProvider,
        DateTime date) : base(botClient)
    {
        _firestoreProvider = firestoreProvider;
        _date = date;
        _message = string.Format(Message, date)
            .EscapeMarkdown();
    }

    public override async Task SendAsync()
    {
        await SendSignPublishedMessage();
        await SendMainMessage();
    }

    private async Task SendSignPublishedMessage()
    {
        var horoscopes = await _firestoreProvider
            .WhereEqualTo<HoroscopePersistence>("Date", _date.ToString(DateFormats.DdMmYyyy));
        
        var messageBuilder = new StringBuilder();
        messageBuilder.Append($"*{_date.ToString(DateFormats.DdMmYyyy)}*\n");
        messageBuilder.Append("Опубліковані передбачення:");

        foreach (var sign in EnumHelpers.GetEnumValues<ZodiacSign>(skipFirst: true))
        {
            messageBuilder.Append("\n\n");
            var publishedHoroscope = horoscopes
                .FirstOrDefault(horoscopePersistence => horoscopePersistence.Sign == sign.ToString());

            var foresight = (Foresight)publishedHoroscope?.Foresight;
            var foresightMessage = !foresight.IsEmpty ?
                foresight.ToString("nn") : 
                "[Передбачення ще не додано]";
            
            messageBuilder.Append(
                string.Format(PublishedSignMessage, sign.GetDisplayName(), foresightMessage));
        }

        var message = messageBuilder
            .ToString()
            .EscapeMarkdown();
        
        var chatId = ExecutionContext.ChatId;
        await BotClient.SendTextMessageAsync(
            chatId,
            message,
            parseMode: ParseMode.MarkdownV2);
    }

    private async Task SendMainMessage()
    {
        var chatId = ExecutionContext.ChatId;
        await BotClient.SendTextMessageAsync(
            chatId,
            _message,
            replyMarkup: GenerateKeyboard(),
            parseMode: ParseMode.MarkdownV2);
    }

    private ReplyKeyboardMarkup GenerateKeyboard()
    {
        var keyboardBuilder = ReplyKeyboardMarkupBuilder.Create();

        EnumHelpers.GetDisplayNames<ZodiacSign>(skipFirst: true)
            .ToTwoDimensionArray(rowSize: 2)
            .ToList()
            .ForEach(row => keyboardBuilder.AddRow(row));

        keyboardBuilder.AddBackAndCancelRow();

        return keyboardBuilder.Build();
    }
}

