using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Helpers;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Repositories;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class WaitingForSignHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Я не знаю такого знаку \ud83d\ude22 Спробуй ще! ";

    private readonly IMessageFactory _messageFactory;
    private readonly IDraftRepository _draftRepository;
    
    public WaitingForSignHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory, IDraftRepository draftRepository)  : base(next, State.WaitingForSign)
    {
        _messageFactory = messageFactory;
        _draftRepository = draftRepository;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (EnumHelpers.TryGetEnumValueFromDisplayName<Sign>(
                request.GetMessage(), out var sign) && sign != Sign.None)
        {
            var draft = ExecutionContext.GetDraft();

            var publishedDraft = await _draftRepository.CreateFromPublished(draft.Date!.Value, sign);

            if (publishedDraft is not null)
            {
                draft.Update(publishedDraft);
                await ExecutionContext.GetSession().FireAsync(Trigger.GoToPreview);
                
                return;
            }

            draft.Sign = sign;
            await ExecutionContext.GetSession().FireGoToChangeDescriptionAsync();
            
            return;
        }
        
        await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
    }
}