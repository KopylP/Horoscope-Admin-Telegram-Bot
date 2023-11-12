using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Helpers;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Repositories;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingZodiacSignHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Я не знаю такого знаку \ud83d\ude22 Спробуй ще! ";

    private readonly IMessageFactory _messageFactory;
    private readonly IDraftRepository _draftRepository;
    
    public AwaitingZodiacSignHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory, IDraftRepository draftRepository)  : base(next, State.AwaitingZodiacSign)
    {
        _messageFactory = messageFactory;
        _draftRepository = draftRepository;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!TryGetSignFromRequest(request, out var sign))
        {
            await SendErrorMessageAsync();
            return;
        }

        ExecutionContext.Draft.Sign = sign;
        await HandleSignAsync(sign);
    }
    
    private bool TryGetSignFromRequest(NewtonsoftJsonUpdate request, out Sign sign)
    {
        return EnumHelpers.TryGetEnumValueFromDisplayName(
            request.GetMessage(), out sign) && sign != Sign.None;
    }
    
    private async Task HandleSignAsync(Sign sign)
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
    
    private async Task SendErrorMessageAsync()
    {
        await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
    }
}