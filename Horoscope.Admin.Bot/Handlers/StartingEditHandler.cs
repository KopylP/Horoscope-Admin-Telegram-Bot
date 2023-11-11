using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class StartingEditHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Щось не збігається \ud83d\ude22 " +
                                        $"Натисніть кнопку \"{ReplyCommands.StartingEdit.Start}\"!";
    
    private readonly IMessageFactory _messageFactory;

    public StartingEditHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.StartingEditHoroscope)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (request.GetMessage() != ReplyCommands.StartingEdit.Start)
        {
            await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
            return;
        }
        
        await ExecutionContext.GetSession().FireEditHoroscopeStartedAsync();
    }
}