using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class AwaitingZodiacSignStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;
    private readonly FirestoreProvider _firestoreProvider;

    public AwaitingZodiacSignStateConfigurator(ITelegramBotClient botClient, FirestoreProvider firestoreProvider)
    {
        _botClient = botClient;
        _firestoreProvider = firestoreProvider;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.AwaitingZodiacSign)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.InitiateDescriptionChange, State.AwaitingForesightInput)
            .Permit(Trigger.ProceedToPreview, State.Preview)
            .Permit(Trigger.NavigateBack, State.AwaitingDateInput)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .OnEntryAsync(() => new ChooseSignMessage(_botClient, _firestoreProvider, ExecutionContext.Draft.Date ?? DateTime.MinValue).SendAsync());
    }
}