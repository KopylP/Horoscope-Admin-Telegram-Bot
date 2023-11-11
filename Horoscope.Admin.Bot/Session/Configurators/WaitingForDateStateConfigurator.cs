using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class WaitingForDateStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public WaitingForDateStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.WaitingForDate)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.DateProvided, State.WaitingForSign)
            .Permit(Trigger.Back, State.StartingEditHoroscope)
            .Permit(Trigger.Cancel, State.StartingEditHoroscope)
            .OnEntryAsync(() => new ProvideDateMessage(_botClient).SendAsync());
    }
}