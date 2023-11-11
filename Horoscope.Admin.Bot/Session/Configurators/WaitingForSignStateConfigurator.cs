using Horoscope.Admin.Bot.Framework.Persistence;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class WaitingForSignStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;
    private readonly FirestoreProvider _firestoreProvider;

    public WaitingForSignStateConfigurator(ITelegramBotClient botClient, FirestoreProvider firestoreProvider)
    {
        _botClient = botClient;
        _firestoreProvider = firestoreProvider;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.WaitingForSign)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.GoToChangeDescription, State.WaitingForForesight)
            .Permit(Trigger.GoToPreview, State.Preview)
            .Permit(Trigger.Back, State.WaitingForDate)
            .Permit(Trigger.Cancel, State.StartingEditHoroscope)
            .OnEntryAsync(() => new ChooseSignMessage(_botClient, _firestoreProvider, ExecutionContext.GetDraft().Date ?? DateTime.MinValue).SendAsync());
    }
}