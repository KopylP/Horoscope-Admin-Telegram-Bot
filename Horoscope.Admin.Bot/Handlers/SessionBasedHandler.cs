using Horoscope.Admin.Bot.Framework.Chains;
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

    public async Task HandleAsync(TRequest request)
    {
        if (ExecutionContext.GetSession().State == _handlerState)
        {
            await StateMatchedHandleAsync(request);
            return;
        }

        if (_next is not null) 
            await _next.HandleAsync(request);
    }

    protected abstract Task StateMatchedHandleAsync(TRequest request);
}