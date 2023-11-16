using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Infrastructure.Repositories;

public interface IDraftRepository
{
    Task<Draft> GetAsync(long chatId);
    Task SaveAsync(long chatId, Draft draft);
    Task<Result> PublishAsync(Draft draft);
    Task<ResultList> PublishBulkAsync(IEnumerable<Draft> drafts);
    Task<Draft?> CreateFromPublished(DateTime date, ZodiacSign sign);
}