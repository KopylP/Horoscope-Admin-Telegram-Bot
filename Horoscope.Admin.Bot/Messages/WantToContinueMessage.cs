using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Messages.Extensions;
using Horoscope.Admin.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Horoscope.Admin.Bot.Messages;

public class WantToContinueMessage: BaseMessage
{
    private const string Message = "Зміни записано!\n\n" +
                                   $$"""Дата: *{0:{{DateFormats.DdMmYyyy}}}*.{{"\n"}}""" +
                                   "Знак зодіаку: *{1}*.\n" +
                                   "Передбачення: \n" +
                                   "{2}\n\n" +
                                   "Хочете продовжити?";
    
    private readonly string _message;
    
    public WantToContinueMessage(ITelegramBotClient botClient, Draft draft) : base(botClient)
    {
        _message = string.Format(Message, draft.Date, draft.Sign.GetDisplayName(), draft.Foresight)
            .EscapeMarkdown();
    }

    public override async Task SendAsync()
    {
        var keyboard = ReplyKeyboardMarkupBuilder.Create()
            .AddRow(ReplyCommands.Continue.NextSign, ReplyCommands.Continue.NextDate)
            .AddBackAndCancelRow()
            .Build();
        
        await BotClient.SendTextMessageAsync(
            ExecutionContext.ChatId,
            _message, 
            replyMarkup: keyboard,
            parseMode: ParseMode.MarkdownV2);
    }
}