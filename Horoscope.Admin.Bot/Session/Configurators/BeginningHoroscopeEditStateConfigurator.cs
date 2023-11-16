using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public sealed class BeginningHoroscopeEditStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public BeginningHoroscopeEditStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.BeginningHoroscopeEdit)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.BeginHoroscopeEditingManually, State.AwaitingDateInput)
            .Permit(Trigger.InitiateLoadFile, State.AwaitingFile)
            .OnEntryAsync(() => new StartEditHoroscopeMessage(_botClient).SendAsync());
    }
}