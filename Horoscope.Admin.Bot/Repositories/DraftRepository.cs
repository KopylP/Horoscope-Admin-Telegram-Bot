using System.Globalization;
using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Framework.Dates;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Persistence;

namespace Horoscope.Admin.Bot.Repositories;

public sealed class DraftRepository : IDraftRepository
{
    private readonly FirestoreProvider _firestoreProvider;

    public DraftRepository(FirestoreProvider firestoreProvider)
    {
        _firestoreProvider = firestoreProvider;
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
            Sign = draftPersistence?.Sign?.ToEnum<Sign>() ?? Sign.None
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

    public async Task PublishAsync(Draft draft)
    {
        var date = draft.Date!.Value.ToString(DateFormats.DdMmYyyy);
        
        var horoscope = new HoroscopePersistence
        {
            Id = GetPublishId(draft.Date.Value, draft.Sign),
            Sign = draft.Sign.ToString(),
            Foresight = draft.Foresight!.Values,
            Date = date
        };

        await _firestoreProvider.AddOrUpdate(horoscope);
    }

    public async Task<Draft?> CreateFromPublished(DateTime date, Sign sign)
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
            Sign = publish.Sign.ToEnum<Sign>()
        };
    }

    private string GetPublishId(DateTime date, Sign sign, string language = "UA") 
        => $"{date.ToString(DateFormats.DdMmYyyy)}-{sign.ToString()}-UA"
        .Replace(".", "-");
}