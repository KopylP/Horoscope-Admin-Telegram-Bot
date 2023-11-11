using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class ForecastProvidedHandler: SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Не знаю такої опції \ud83d\ude14 Вибери ще!";
    
    private readonly IMessageFactory _messageFactory;
    
    public ForecastProvidedHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.ForesightProvided)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        switch (request.GetMessage())
        {
            case ReplyCommands.Continue.NextSign:
                await ExecutionContext.GetSession().FireNextSignAsync();
                break;
            
            case ReplyCommands.Continue.NextDate:
                await ExecutionContext.GetSession().FireNextDateAsync();
                break;
            
            default:
                await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
                break;
        }
    }
}