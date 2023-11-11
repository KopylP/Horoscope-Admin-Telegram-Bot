using Horoscope.Admin.Bot.Framework.Sessions;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class BeforeStartStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.BeforeStart)
            .Permit(Trigger.Start, State.WaitingForApiKey);
    }
}