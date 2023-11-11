using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class WaitingForApiKeyStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public WaitingForApiKeyStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.WaitingForApiKey)
            .Permit(Trigger.ApiKeyProvided, State.StartingEditHoroscope)
            .PermitReentry(Trigger.Start)
            .OnEntryAsync(() => new ProvideApiKeyMessage(_botClient).SendAsync());
    }
}