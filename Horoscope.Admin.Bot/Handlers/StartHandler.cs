using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

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
        var session = ExecutionContext.GetSession();
        
        if (request.GetMessage() != CommandName)
        {
            if (session.State == State.BeforeStart)
            {
                var startNotAllowedMessage = $"Only the {CommandName} command is allowed";
                await _messageFactory.CreateStandardMessage(startNotAllowedMessage).SendAsync();
            }
            else if (_next != null)
            {
                await _next.HandleAsync(request);
            }
            
            return;
        }

        await session.FireStartAsync();
    }
}