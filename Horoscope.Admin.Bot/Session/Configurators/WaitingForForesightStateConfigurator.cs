using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class WaitingForForesightStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public WaitingForForesightStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.WaitingForForesight)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.ForesightProvided, State.ForesightProvided)
            .Permit(Trigger.Back, State.WaitingForSign)
            .Permit(Trigger.Cancel, State.StartingEditHoroscope)
            .OnEntryAsync(() => new ProvideForesightMessage(_botClient, ExecutionContext.GetDraft()).SendAsync());    }
}