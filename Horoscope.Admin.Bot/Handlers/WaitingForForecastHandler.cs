using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Repositories;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class WaitingForForecastHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private readonly IDraftRepository _draftRepository;
    
    public WaitingForForecastHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IDraftRepository draftRepository) : base(next, State.WaitingForForesight)
    {
        _draftRepository = draftRepository;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        var draft = ExecutionContext.GetDraft();
        draft.Foresight = (Foresight)request.GetMessage();
        await _draftRepository.PublishAsync(draft);
        await ExecutionContext.GetSession().FireForesightProvidedAsync();
    }
}