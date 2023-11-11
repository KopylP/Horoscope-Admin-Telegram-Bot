using Horoscope.Admin.Bot.Framework.Sessions;

namespace Horoscope.Admin.Bot.Session;

public interface ISessionStateProvider
{
    public Task<SessionState<State, Trigger>> GetStateAsync(long chatId);
    public Task UpdateAsync(long chatId, SessionState<State, Trigger> sessionState);
}