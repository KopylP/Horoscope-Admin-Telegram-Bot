namespace Horoscope.Admin.Bot.Framework.Sessions.Implementations;

public class SessionStateFactory<TState, TTrigger> : ISessionStateFactory<TState, TTrigger>
{
    private readonly IEnumerable<ISessionStateConfigurator<TState, TTrigger>> _configuratiors;

    public SessionStateFactory(IEnumerable<ISessionStateConfigurator<TState, TTrigger>>? configuratiors)
    {
        _configuratiors = configuratiors ?? Array.Empty<ISessionStateConfigurator<TState, TTrigger>>();
    }

    public SessionState<TState, TTrigger> Create(TState initialState)
    {
        var state = new SessionState<TState, TTrigger>(initialState);
        
        foreach (var configurator in _configuratiors)
        {
            configurator.Configure(state);
        }

        return state;
    }
}