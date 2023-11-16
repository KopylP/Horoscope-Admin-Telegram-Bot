using System.Globalization;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Infrastructure.Persistence;
using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Infrastructure.Repositories;

public sealed class DraftRepository : IDraftRepository
{
    private readonly FirestoreProvider _firestoreProvider;
    private readonly ILogger<DraftRepository> _logger;

    public DraftRepository(FirestoreProvider firestoreProvider, ILogger<DraftRepository> logger)
    {
        _firestoreProvider = firestoreProvider;
        _logger = logger;
    }
    
    public async Task<Draft> GetAsync(long chatId)
    {
        var draftPersistence = await _firestoreProvider
            .Get<DraftPersistence>(chatId.ToString());

        return new Draft
        {
            Date = !string.IsNullOrWhiteSpace(draftPersistence?.Date) ? 
                DateTime.ParseExact(draftPersistence.Date, DateFormats.DdMmYyyy, CultureInfo.InvariantCulture)
                : null,
            Foresight = (Foresight?)draftPersistence?.Foresight,
            Sign = draftPersistence?.Sign?.ToEnum<ZodiacSign>() ?? ZodiacSign.None
        };
    }

    public async Task SaveAsync(long chatId, Draft draft)
    {
        var draftPersistence = new DraftPersistence
        {
            Id = chatId.ToString(),
            Sign = draft.Sign.ToString(),
            Foresight = (string?)draft.Foresight,
            Date = draft.Date is not null ? 
                draft.Date.Value.ToString(DateFormats.DdMmYyyy) :
                string.Empty
        };

        await _firestoreProvider.AddOrUpdate(draftPersistence);
    }

    public async Task<Result> PublishAsync(Draft draft)
    {
        if (draft is null)
            throw new ArgumentException("Draft cannot be null!");

        if (!draft.IsReadyForPublish)
            return Result.Fail("DraftIsNotReadyForPublish");
        
        var horoscope = ToHoroscopePersistence(draft);
        await _firestoreProvider.AddOrUpdate(horoscope);
        return Result.Success();
    }
    
    public async Task<ResultList> PublishBulkAsync(IEnumerable<Draft>? drafts)
    {
        drafts = drafts?.ToArray() ?? Array.Empty<Draft>();
        
        var resultList = ResultList.CreateForAnySuccessStrategy();
        var results = drafts.Select(d => d.IsReadyForPublish ?
            Result.Success() :
            Result.Fail("DraftIsNotReadyForPublish"))
            .ToArray();
        resultList.AddResults(results);
        
        var readyForPublishDrafts = drafts
            .Where(draft => draft.IsReadyForPublish)
            .Select(ToHoroscopePersistence)
            .ToArray();
        
        if (readyForPublishDrafts.Any())
            await _firestoreProvider.AddOrUpdateBulk(readyForPublishDrafts);

        return resultList;
    }

    public async Task<Draft?> CreateFromPublished(DateTime date, ZodiacSign sign)
    {
        var id = GetPublishId(date, sign);
        
        var publish = await _firestoreProvider
            .Get<HoroscopePersistence>(id);

        if (publish == null)
            return null;
        
        return new Draft
        {
            Date = DateTime.ParseExact(publish.Date, DateFormats.DdMmYyyy, CultureInfo.InvariantCulture),
            Foresight = (Foresight)publish.Foresight,
            Sign = publish.Sign.ToEnum<ZodiacSign>()
        };
    }

    private string GetPublishId(DateTime date, ZodiacSign sign, string language = "UA") 
        => $"{date.ToString(DateFormats.DdMmYyyy)}-{sign.ToString()}-UA"
        .Replace(".", "-");

    private HoroscopePersistence ToHoroscopePersistence(Draft draft)
    {
        return new HoroscopePersistence
        {
            Id = GetPublishId(draft.Date!.Value, draft.Sign),
            Sign = draft.Sign.ToString(),
            Foresight = draft.Foresight!.Values,
            Date = draft.Date!.Value.ToString(DateFormats.DdMmYyyy),
        };
    }
}