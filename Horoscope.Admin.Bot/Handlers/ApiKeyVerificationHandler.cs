using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class ApiKeyVerificationHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Ключ не підходить \ud83d\ude22 Спробуй ще!";
    private const string Message = "Супер, підходить! " +
                                   "Раджу видалити повідомлення з Api Key \ud83d\ude09, " +
                                   "щоб він не потрапив не в ті руки!";
        
    private readonly string _apiKey;
    private readonly IMessageFactory _messageFactory;
    
    public ApiKeyVerificationHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IConfiguration configuration,
        IMessageFactory messageFactory) : base(next, State.WaitingForApiKey)
    {
        _apiKey = configuration["ApiKey"]!;
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (request.GetMessage() != _apiKey)
        {
            await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
            return;
        }

        await _messageFactory.CreateStandardMessage(Message).SendAsync();
        await ExecutionContext.GetSession().FireApiKeyProvidedAsync();
    }
}