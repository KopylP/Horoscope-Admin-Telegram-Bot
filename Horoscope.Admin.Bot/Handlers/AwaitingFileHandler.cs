using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Infrastructure.Loaders;
using Horoscope.Admin.Bot.Infrastructure.Repositories;
using Horoscope.Admin.Bot.Infrastructure.Utils;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Horoscope.Admin.Bot.Handlers;

public class AwaitingFileHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    public string SuccessMessage = "Успішно опубліковано {0} передбачень.";
    
    private readonly ITelegramBotClient _botClient;
    private readonly IDraftLoader _draftLoader;
    private readonly IDraftRepository _draftRepository;
    private readonly IMessageFactory _messageFactory;
    private readonly TelegramFileDownloader _fileDownloader;
    
    public AwaitingFileHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        ITelegramBotClient botClient,
        IDraftLoader draftLoader,
        IDraftRepository draftRepository,
        IMessageFactory messageFactory,
        TelegramFileDownloader fileDownloader)
        : base(next, State.AwaitingFile)
    {
        _botClient = botClient;
        _draftLoader = draftLoader;
        _draftRepository = draftRepository;
        _messageFactory = messageFactory;
        _fileDownloader = fileDownloader;
    }
    
    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsValidDocumentRequest(request))
        {
            return Result.Fail(FailCodes.InvalidFileFormat);
        }

        var drafts = await LoadDraftsAsync(request);
        var publishedDrafts = await _draftRepository.PublishBulkAsync(drafts);
        await SendSuccessMessage(publishedDrafts);
        await ExecutionContext.Session.FireNavigateBeginningAsync();
        return Result.Success();
    }

    private static bool IsValidDocumentRequest(Update request)
        => request.Message is
           {
               Type: MessageType.Document,
               Document: not null,
               Document.FileName: not null
           } &&
           request.Message.Document.IsExcelExt();

    private async Task<IEnumerable<Draft>> LoadDraftsAsync(Update request)
    {
        using var stream = new MemoryStream();
        var file = await _botClient.GetFileAsync(request.Message!.Document!.FileId);
        await _botClient.DownloadFileAsync(file.FilePath!, stream);
        return _draftLoader.Load(stream.ToArray());
    }

    private async Task SendSuccessMessage(ResultList publishedDraftsResult)
    {
        var successResultsCount = publishedDraftsResult.SuccessResults.Count();
        await _messageFactory.CreateStandardMessage(string.Format(SuccessMessage, successResultsCount))
            .SendAsync();
    }
}