using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Infrastructure.Repositories;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingForesightInputHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private readonly IDraftRepository _draftRepository;
    
    public AwaitingForesightInputHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IDraftRepository draftRepository) : base(next, State.AwaitingForesightInput)
    {
        _draftRepository = draftRepository;
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        var draft = ExecutionContext.Draft;
        draft.Foresight = (Foresight)request.GetMessage();
        await _draftRepository.PublishAsync(draft);
        await ExecutionContext.Session.FireForesightSubmittedAsync();

        return Result.Success();
    }
}