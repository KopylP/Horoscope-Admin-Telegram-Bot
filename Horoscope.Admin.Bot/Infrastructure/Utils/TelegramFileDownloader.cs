using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Horoscope.Admin.Bot.Infrastructure.Utils;

public  class TelegramFileDownloader
{
    private readonly BotSettings _botSettings;

    public TelegramFileDownloader(BotSettings botSettings)
    {
        _botSettings = botSettings;
    }

    public async Task DownloadFileAsync(Message message, Stream stream)
    {
        if (message is not { Type: MessageType.Document, Document: not null, Document.FileName: not null })
        {
            throw new ArgumentException("The provided message must be a document and cannot be null.");
        }
 
        if (stream is null)
        {
            throw new ArgumentException("Stream cannot be null.");
        }

        using var httpClient = new HttpClient();
        
        var response = await httpClient.GetAsync(GetFileUrl(message.Document!.FileId));
        response.EnsureSuccessStatusCode();
 
        await response.Content.CopyToAsync(stream);
    }

    private string GetFileUrl(string fileId)
    {
        return $"https://api.telegram.org/bot{_botSettings.BotToken}/getFile?file_id={fileId}";
    }
}