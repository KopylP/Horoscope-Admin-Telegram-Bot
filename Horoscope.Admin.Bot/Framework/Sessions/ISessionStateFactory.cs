namespace Horoscope.Admin.Bot.Framework.Sessions;

public interface ISessionStateFactory<TState, TTrigger>
{
    public SessionState<TState, TTrigger> Create(TState initialState);
}