using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Models;

namespace Horoscope.Admin.Bot.Repositories;

public interface IDraftRepository
{
    public Task<Draft> GetAsync(long chatId);
    public Task SaveAsync(long chatId, Draft draft);
    public Task PublishAsync(Draft draft);
    public Task<Draft?> CreateFromPublished(DateTime date, Sign sign);
}