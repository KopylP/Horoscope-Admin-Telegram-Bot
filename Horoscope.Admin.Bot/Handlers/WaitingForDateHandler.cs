using System.Globalization;
using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class WaitingForDateHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string Format = "dd.MM.yyyy";
    
    private const string ErrorMessage = "Щось якась дивна дата \ud83d\ude22 Спробуй ще!";

    private readonly IMessageFactory _messageFactory;

    public WaitingForDateHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.WaitingForDate)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        var isSuccessful = DateTime.TryParseExact(
            request.GetMessage(),
            Format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date);

        if (isSuccessful)
        {
            ExecutionContext.GetDraft().Date = date.Date;
            await ExecutionContext.GetSession().FireDateProvidedAsync();
            
            return;
        }

        await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
    }
}