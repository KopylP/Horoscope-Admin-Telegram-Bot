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
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.InitiateDescriptionChange, State.AwaitingForesightInput)
            .Permit(Trigger.NavigateBack, State.AwaitingZodiacSign)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .OnEntryAsync(() => new PreviewMessage(_botClient, ExecutionContext.Draft).SendAsync());    }
}