using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Helpers;
using Horoscope.Admin.Bot.Framework.Keyword;
using Horoscope.Admin.Bot.Messages.Extensions;
using Horoscope.Admin.Bot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Horoscope.Admin.Bot.Messages;

public class LoadFileMessage : BaseMessage
{
    private const string Message = "Завантаж файл з розширенням .xlsx з датами та знаками зодіаку.";
    
    public LoadFileMessage(ITelegramBotClient botClient) : base(botClient)
    {
    }

    public override async Task SendAsync()
    {
        var chatId = ExecutionContext.ChatId;
        await BotClient.SendTextMessageAsync(
            chatId, 
            text: Message, 
            replyMarkup: GenerateKeyboard());
    }
    
    private ReplyKeyboardMarkup GenerateKeyboard()
    {
        return ReplyKeyboardMarkupBuilder.Create()
            .AddBackRow()
            .Build();
    }
}