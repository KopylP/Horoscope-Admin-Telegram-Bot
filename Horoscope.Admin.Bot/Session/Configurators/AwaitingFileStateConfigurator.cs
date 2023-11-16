using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Telegram.Bot;

namespace Horoscope.Admin.Bot.Session.Configurators;

public class AwaitingFileStateConfigurator : ISessionStateConfigurator<State, Trigger>
{
    private readonly ITelegramBotClient _botClient;

    public AwaitingFileStateConfigurator(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }
    
    public void Configure(SessionState<State, Trigger> sessionState)
    {
        sessionState.Configure(State.AwaitingFile)
            .Permit(Trigger.Start, State.AwaitingApiKey)
            .Permit(Trigger.NavigateBack, State.BeginningHoroscopeEdit)
            .Permit(Trigger.NavigateBeginning, State.BeginningHoroscopeEdit)
            .OnEntryAsync(() => new LoadFileMessage(_botClient).SendAsync());
    }
}