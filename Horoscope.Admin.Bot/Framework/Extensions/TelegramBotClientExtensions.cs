using Telegram.Bot;
using Telegram.Bot.Types;
using File = Telegram.Bot.Types.File;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task<File> GetFile(this ITelegramBotClient botClient, Message message)
    {
        if (message.Document == null)
        {
            throw new ArgumentException("The provided message does not contain a document.", nameof(message));
        }

        var fileId = message.Document.FileId;
        var file = await botClient.GetFileAsync(fileId);
        
        return file;
    }
}