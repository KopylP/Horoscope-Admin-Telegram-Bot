using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class PreviewStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public PreviewStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.Preview)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.GoToChangeDescription, State.WaitingForForesight)
            .Permit(Trigger.Back, State.WaitingForSign)
            .Permit(Trigger.Cancel, State.StartingEditHoroscope)
            .OnEntryAsync(() => new PreviewMessage(_botClient, ExecutionContext.GetDraft()).SendAsync());    }
}