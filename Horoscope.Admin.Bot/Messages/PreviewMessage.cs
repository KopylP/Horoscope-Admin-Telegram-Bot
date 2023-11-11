using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Messages.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public class PreviewMessage : BaseMessage
{
    private const string Message = $$"""Дата: *{0:{{DateFormats.DdMmYyyy}}}*.{{"\n"}}""" +
                                   "Знак зодіаку: *{1}*.\n\n" +
                                   "Передбачення:";
    
    private readonly string _message;
    private readonly string _foresight;
    
    public PreviewMessage(ITelegramBotClient botClient, Draft draft) : base(botClient)
    {
        _message = string.Format(Message, draft.Date, draft.Sign.GetDisplayName())
            .EscapeMarkdown();
        _foresight = (string)draft.Foresight!;
    }

    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.GetChatId();
        await BotClient.SendTextMessageAsync(
            chatId,
            _message,
            parseMode: ParseMode.MarkdownV2);
        
        await BotClient.SendTextMessageAsync(
            chatId,
            _foresight,
            replyMarkup: GenerateKeyboard());
    }
    
    private ReplyKeyboardMarkup GenerateKeyboard()
    {
        var keyboardBuilder = ReplyKeyboardMarkupBuilder.Create()
            .AddRow(ReplyCommands.Preview.Edit)
            .AddBackAndCancelRow();

        return keyboardBuilder.Build();
    }
}