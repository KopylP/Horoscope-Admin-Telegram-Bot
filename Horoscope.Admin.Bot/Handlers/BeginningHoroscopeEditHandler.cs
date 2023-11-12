using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class BeginningHoroscopeEditHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Щось не збігається \ud83d\ude22 " +
                                        $"Натисніть кнопку \"{ReplyCommands.BeginningHoroscopeEdit.Begin}\"!";
    
    private readonly IMessageFactory _messageFactory;

    public BeginningHoroscopeEditHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.BeginningHoroscopeEdit)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsBeginCommandReceived(request))
        {
            await SendErrorMessageAsync();
            return;
        }
        
        await ExecutionContext.Session
            .FireBeginHoroscopeEditingAsync();
    }

    private static bool IsBeginCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.BeginningHoroscopeEdit.Begin;
    
    private async Task SendErrorMessageAsync() 
        => await _messageFactory
            .CreateStandardMessage(ErrorMessage)
            .SendAsync();
}