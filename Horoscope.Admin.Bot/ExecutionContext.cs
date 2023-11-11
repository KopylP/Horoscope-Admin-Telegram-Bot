using Horoscope.Admin.Bot.Framework.Contexts;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot;

public static class ExecutionContext
{
    private static readonly AsyncLocal<long> ChatId = new();
    private static readonly AsyncLocal<SessionState<State,Trigger>> Session = new();
    private static readonly AsyncLocal<Draft> Draft = new();
    
    public static void Apply(SessionState<State,Trigger> session)
    {
        Session.Value = session;
    }
    
    public static void Apply(Draft draft)
    {
        Draft.Value = draft;
    }
    
    public static void Apply(long chatId)
    {
        ChatId.Value = chatId;
    }
    
    public static SessionState<State,Trigger> GetSession()
    {
        return Session.Value!;
    }
    
    public static Draft GetDraft()
    {
        return Draft.Value!;
    }
    
    public static long GetChatId()
    {
        return ChatId.Value!;
    }
}