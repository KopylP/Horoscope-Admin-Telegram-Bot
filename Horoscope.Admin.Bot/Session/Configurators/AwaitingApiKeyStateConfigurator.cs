using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class AwaitingApiKeyStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public AwaitingApiKeyStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.AwaitingApiKey)
            .Permit(Trigger.ApiKeySubmitted, State.BeginningHoroscopeEdit)
            .PermitReentry(Trigger.Start)
            .OnEntryAsync(() => new ProvideApiKeyMessage(_botClient).SendAsync());
    }
}