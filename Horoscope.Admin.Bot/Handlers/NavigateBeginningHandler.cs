using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class NavigateBeginningHandler :  IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;

    public NavigateBeginningHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next)
    {
        _next = next;
    }
    
    public async Task<Result> HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsNavigateBeginningCommandReceived(request) && _next is not null)
        {
            return await _next.HandleAsync(request);
        }
        
        await ExecutionContext.Session.FireNavigateBeginningAsync();
        return Result.Success();
    }
    
    private static bool IsNavigateBeginningCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.Common.NavigateBeginning;
}