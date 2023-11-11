using System.Web;
using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Messages.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public class ProvideForesightMessage : BaseMessage
{
    private const string Message = $$"""Дата: *{0:{{DateFormats.DdMmYyyy}}}*.{{"\n"}}""" +
                                   "Знак зодіаку: *{1}*.\n\n" +
                                   "Тепер додай передбачення!\n\n" +
                                   "Якщо хочеш додати декілька передбачень, розділи їх знаком '|'.";

    private readonly string _message;
    
    public ProvideForesightMessage(ITelegramBotClient botClient, Draft draft) : base(botClient)
    {
        _message = string.Format(Message, draft.Date, draft.Sign.GetDisplayName())
            .EscapeMarkdown();
    }

    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.GetChatId();
        await BotClient.SendTextMessageAsync(
            chatId,
            _message,
            replyMarkup: GenerateKeyboard(),
            parseMode: ParseMode.MarkdownV2);
    }

    private ReplyKeyboardMarkup GenerateKeyboard()
    {
        return ReplyKeyboardMarkupBuilder.Create()
            .AddBackAndCancelRow()
            .Build();
    }
}