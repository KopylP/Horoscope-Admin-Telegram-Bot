using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class AwaitingDateInputStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public AwaitingDateInputStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.AwaitingDateInput)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.DateSubmitted, State.AwaitingZodiacSign)
            .Permit(Trigger.NavigateBack, State.BeginningHoroscopeEdit)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .OnEntryAsync(() => new ProvideDateMessage(_botClient).SendAsync());
    }
}