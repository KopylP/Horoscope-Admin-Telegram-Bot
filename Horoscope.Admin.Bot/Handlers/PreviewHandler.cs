using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class PreviewHandler: SessionBasedHandler<NewtonsoftJsonUpdate>
{
    
    public PreviewHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next) 
        : base(next, State.Preview)
    {
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        switch (request.GetMessage())
        {
            case ReplyCommands.Preview.Edit:
                await HandleEditCommandAsync();
                return Result.Success();
            
            default:
                return Result.Fail(FailCodes.FailInput);
        }
    }
    
    private static async Task HandleEditCommandAsync()
    {
        await ExecutionContext.Session
            .FireInitiateDescriptionChangeAsync();
    }
}