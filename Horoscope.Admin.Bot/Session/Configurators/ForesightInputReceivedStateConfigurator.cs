using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class ForesightInputReceivedStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public ForesightInputReceivedStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.ForesightInputReceived)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.AdvanceToNextSign, State.AwaitingZodiacSign)
            .Permit(Trigger.AdvanceToNextDate, State.AwaitingDateInput)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .Permit(Trigger.NavigateBack, State.AwaitingForesightInput)
            .OnEntryAsync(() => new WantToContinueMessage(_botClient, ExecutionContext.Draft).SendAsync());    }
}