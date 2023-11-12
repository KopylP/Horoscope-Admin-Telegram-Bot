using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
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

    public async Task HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (IsBackCommandReceived(request))
        {
            await ExecutionContext.Session.FireNavigateBackAsync();
        }
        else if (_next is not null)
        {
            await _next.HandleAsync(request);
        }
    }

    private static bool IsBackCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.Common.Back;
}