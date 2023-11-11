using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class ForesightProvidedStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public ForesightProvidedStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.ForesightProvided)
            .Permit(Trigger.Start, State.WaitingForApiKey)
            .Permit(Trigger.NextSign, State.WaitingForSign)
            .Permit(Trigger.NextDate, State.WaitingForDate)
            .Permit(Trigger.Cancel, State.StartingEditHoroscope)
            .Permit(Trigger.Back, State.WaitingForForesight)
            .OnEntryAsync(() => new WantToContinueMessage(_botClient, ExecutionContext.GetDraft()).SendAsync());    }
}