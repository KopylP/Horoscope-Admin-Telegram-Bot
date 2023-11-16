using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Resources;

namespace Horoscope.Admin.Bot.Handlers;

public class ErrorHandler : IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private readonly IMessageFactory _messageFactory;
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;
    
    public ErrorHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory)
    {
        _messageFactory = messageFactory;
        _next = next;
    }
    
    public async Task<Result> HandleAsync(NewtonsoftJsonUpdate request)
    {
        if (_next is null)
            return Result.Success();

        var result = await _next.HandleAsync(request);
        
        await result.OnFail(async res =>
        {
            var message = Shared.Messages[res.FailCode!];
            await _messageFactory.CreateStandardMessage(message)
                .SendAsync();
        });

        return result;
    }
}