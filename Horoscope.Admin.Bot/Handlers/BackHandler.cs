using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class BackHandler :  IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;

    public BackHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next)
    {
        _next = next;
    }

    public async Task<Result> HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsBackCommandReceived(request) && _next is not null)
        {
            return await _next.HandleAsync(request);

        }
        
        await ExecutionContext.Session.FireNavigateBackAsync();
        return Result.Success();
    }

    private static bool IsBackCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.Common.Back;
}