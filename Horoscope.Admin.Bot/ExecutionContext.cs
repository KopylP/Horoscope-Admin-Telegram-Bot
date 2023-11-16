using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot;

public static class ExecutionContext
{
    private static readonly AsyncLocal<long> _chatId = new();
    private static readonly AsyncLocal<SessionState<State,Trigger>> _session = new();
    private static readonly AsyncLocal<Draft> _draft = new();

    public static Draft Draft => _draft.Value!;
    public static long ChatId => _chatId.Value;
    public static SessionState<State, Trigger> Session => _session.Value!;
    
    public static void Apply(SessionState<State,Trigger> session)
    {
        _session.Value = session;
    }
    
    public static void Apply(Draft draft)
    {
        _draft.Value = draft;
    }
    
    public static void Apply(long chatId)
    {
        _chatId.Value = chatId;
    }
}