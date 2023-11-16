using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingApiKeyHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string Message = "Супер, підходить! " +
                                   "Раджу видалити повідомлення з Api Key \ud83d\ude09, " +
                                   "щоб він не потрапив не в ті руки!";
        
    private readonly string _apiKey;
    private readonly IMessageFactory _messageFactory;
    
    public AwaitingApiKeyHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IConfiguration configuration,
        IMessageFactory messageFactory) : base(next, State.AwaitingApiKey)
    {
        _apiKey = configuration["ApiKey"]!;
        _messageFactory = messageFactory;
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!IsApiKeyValid(request))
        {
            return Result.Fail(FailCodes.InvalidApiKey);
        }

        await SendApiKeyCorrectMessage();
        await ExecutionContext.Session.FireApiKeySubmittedAsync();

        return Result.Success();
    }

    private bool IsApiKeyValid(NewtonsoftJsonUpdate request)
        => request.GetMessage() == _apiKey;

    private async Task SendApiKeyCorrectMessage() 
        => await _messageFactory
        .CreateStandardMessage(Message)
        .SendAsync();
}