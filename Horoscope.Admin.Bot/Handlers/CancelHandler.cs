using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class CancelHandler :  IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;

    public CancelHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next)
        => _next = next;
    
    public async Task HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (request.GetMessage() == ReplyCommands.Common.Cancel)
        {
            await ExecutionContext.GetSession().FireCancelAsync();
            return;
        }

        if (_next is not null)
            await _next.HandleAsync(request);
    }
}