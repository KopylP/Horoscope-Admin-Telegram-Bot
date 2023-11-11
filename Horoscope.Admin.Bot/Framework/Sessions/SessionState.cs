using Stateless;

namespace Horoscope.Admin.Bot.Framework.Sessions;

public class SessionState<TState, TTrigger> : StateMachine<TState, TTrigger>
{
    public SessionState(TState initialState) : base(initialState)
    {
    }
}