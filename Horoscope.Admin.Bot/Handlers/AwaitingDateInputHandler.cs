using System.Globalization;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingDateInputHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string Format = "dd.MM.yyyy";
    
    private const string ErrorMessage = "Щось якась дивна дата \ud83d\ude22 Спробуй ще!";

    private readonly IMessageFactory _messageFactory;

    public AwaitingDateInputHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.AwaitingDateInput)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!ParseDateFromRequest(request, out var date))
        {
            await SendErrorMessageAsync();
            return;
        }

        ExecutionContext.Draft.Date = date.Date;
        await ExecutionContext.Session.FireDateSubmittedAsync();
    }
    
    private static bool ParseDateFromRequest(Update update, out DateTime date) 
        => DateTime.TryParseExact(update.GetMessage(), Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    
    private async Task SendErrorMessageAsync()
        => await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
}