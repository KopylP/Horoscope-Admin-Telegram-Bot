using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class ForesightInputReceivedHandler: SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private readonly IMessageFactory _messageFactory;
    
    public ForesightInputReceivedHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.ForesightInputReceived)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        switch (request.GetMessage())
        {
            case ReplyCommands.Continue.NextSign:
                await HandleNextSignAsync();
                break;
            
            case ReplyCommands.Continue.NextDate:
                await HandleNextDateAsync();
                break;
            
            default:
                return Result.Fail(FailCodes.FailInput);
        }

        return Result.Success();
    }
    
    private async Task HandleNextSignAsync()
    {
        await ExecutionContext.Session
            .FireAdvanceToNextSignAsync();
    }

    private async Task HandleNextDateAsync()
    {
        await ExecutionContext.Session
            .FireAdvanceToNextDateAsync();
    }
}