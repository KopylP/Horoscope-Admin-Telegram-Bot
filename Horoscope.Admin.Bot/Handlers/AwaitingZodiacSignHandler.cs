using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Helpers;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Infrastructure.Repositories;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingZodiacSignHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private readonly IDraftRepository _draftRepository;
    
    public AwaitingZodiacSignHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IDraftRepository draftRepository)  : base(next, State.AwaitingZodiacSign)
    {
        _draftRepository = draftRepository;
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!TryGetSignFromRequest(request, out var sign))
        {
            return Result.Fail(FailCodes.InvalidZodiacSign);
        }

        ExecutionContext.Draft.Sign = sign;
        await HandleSignAsync(sign);

        return Result.Success();
    }
    
    private bool TryGetSignFromRequest(NewtonsoftJsonUpdate request, out ZodiacSign sign)
    {
        return EnumHelpers.TryGetEnumValueFromDisplayName(
            request.GetMessage(), out sign) && sign != ZodiacSign.None;
    }
    
    private async Task HandleSignAsync(ZodiacSign sign)
    {
        var draft = ExecutionContext.Draft;
        var publishedDraft = await _draftRepository.CreateFromPublished(draft.Date!.Value, sign);

        if (publishedDraft != null)
        {
            draft.Update(publishedDraft);
            await ExecutionContext.Session
                .FireProceedToPreviewAsync();
        }
        else
        {
            await ExecutionContext.Session
                .FireInitiateDescriptionChangeAsync();
        }
    }
}