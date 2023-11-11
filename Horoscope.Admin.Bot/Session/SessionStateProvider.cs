using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Persistence;

namespace Horoscope.Admin.Bot.Session;

public sealed class SessionStateProvider : ISessionStateProvider
{
    private readonly FirestoreProvider _firestoreProvider;
    private readonly ISessionStateFactory<State, Trigger> _sessionStateFactory;

    public SessionStateProvider(
        FirestoreProvider firestoreProvider,
        ISessionStateFactory<State, Trigger> sessionStateFactory)
    {
        _firestoreProvider = firestoreProvider;
        _sessionStateFactory = sessionStateFactory;
    }

    public async Task<SessionState<State,Trigger>> GetStateAsync(long chatId)
    {
        var sessionStatePersistence = await _firestoreProvider
            .Get<SessionStatePersistence>(chatId.ToString());

        var initialState = sessionStatePersistence?
                               .CurrentState?
                               .ToEnum<State>() ?? State.BeforeStart;

        return _sessionStateFactory.Create(initialState);
    }

    public async Task UpdateAsync(long chatId, SessionState<State,Trigger> sessionState)
    {
        var persistence = SessionStatePersistence.Create(chatId.ToString(), sessionState.State);
        await _firestoreProvider.AddOrUpdate(persistence);
    }
}