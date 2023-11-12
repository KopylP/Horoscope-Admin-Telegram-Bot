using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class AwaitingForesightInputStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public AwaitingForesightInputStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.AwaitingForesightInput)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.ForesightSubmitted, State.ForesightInputReceived)
            .Permit(Trigger.NavigateBack, State.AwaitingZodiacSign)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .OnEntryAsync(() => new ProvideForesightMessage(_botClient, ExecutionContext.Draft).SendAsync());    }
}