using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class StartHandler : IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private const string CommandName = "/start";

    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;
    
    public StartHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next)
    {
        _next = next;
    }

    public async Task<Result> HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsStartCommandReceived(request))
        {
            return await HandleNonStartCommandAsync(request);
        }

        return await HandleStartCommandAsync();
    }

    private static bool IsStartCommandReceived(Update request)
        => request.GetMessage() == CommandName;

    private async Task<Result> HandleStartCommandAsync()
    { 
        await ExecutionContext.Session.FireStartAsync();
        return Result.Success();
    }

    private async Task<Result> HandleNonStartCommandAsync(NewtonsoftJsonUpdate request)
    {
        if (ExecutionContext.Session.State.IsBeforeStart())
        {
            return Result.Fail(FailCodes.OnlyStartCommandAllowed);
        }
        
        if (_next != null)
        {
            return await _next.HandleAsync(request);
        }

        return Result.Success();
    }
}