namespace Horoscope.Admin.Bot.Framework.Sessions;

public interface ISessionStateConfigurator<TState, TTrigger>
{
    public void Configure(SessionState<TState, TTrigger> sessionState);
}