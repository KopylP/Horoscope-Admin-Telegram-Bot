using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class StartingEditHoroscopeStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public StartingEditHoroscopeStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.StartingEditHoroscope)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.EditHoroscopeStarted, State.WaitingForDate)
            .OnEntryAsync(() => new StartEditHoroscopeMessage(_botClient).SendAsync());
    }
}