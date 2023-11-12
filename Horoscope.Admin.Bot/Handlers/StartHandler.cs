using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class StartHandler : IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private const string CommandName = "/start";

    private readonly IMessageFactory _messageFactory;
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;
    
    public StartHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next, IMessageFactory commandfactory)
    {
        _messageFactory = commandfactory;
        _next = next;
    }

    public async Task HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsStartCommandReceived(request))
        {
            await HandleNonStartCommandAsync(request);
            return;
        }

        await HandleStartCommandAsync();
    }

    private static bool IsStartCommandReceived(Update request)
        => request.GetMessage() == CommandName;
    
    private async Task HandleStartCommandAsync() 
        => await ExecutionContext.Session
            .FireStartAsync();
    
    private async Task HandleNonStartCommandAsync(NewtonsoftJsonUpdate request)
    {
        if (ExecutionContext.Session.State.IsBeforeStart())
        {
            var startNotAllowedMessage = $"Only the {CommandName} command is allowed";
            await _messageFactory.CreateStandardMessage(startNotAllowedMessage).SendAsync();
        }
        else if (_next != null)
        {
            await _next.HandleAsync(request);
        }
    }
}