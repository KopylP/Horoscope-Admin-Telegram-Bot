using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public abstract class SessionBasedHandler<TRequest> : IChainOfResponsibilityHandler<TRequest>
{
    private readonly State _handlerState;
    private readonly IChainOfResponsibilityHandler<TRequest>? _next;

    protected SessionBasedHandler(IChainOfResponsibilityHandler<TRequest>? next, State handlerState)
    {
        _handlerState = handlerState;
        _next = next;
    }

    public async Task<Result> HandleAsync(TRequest request)
    {
        if (ExecutionContext.Session.State == _handlerState)
        {
            return await StateMatchedHandleAsync(request);
        }
        
        if (_next is not null)
        {
            return await _next.HandleAsync(request);
        }

        return Result.Success();
    }

    protected abstract Task<Result> StateMatchedHandleAsync(TRequest request);
}