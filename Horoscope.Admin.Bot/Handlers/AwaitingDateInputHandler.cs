using System.Globalization;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class AwaitingDateInputHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string Format = "dd.MM.yyyy";
    public AwaitingDateInputHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next) 
        : base(next, State.AwaitingDateInput)
    {
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (!ParseDateFromRequest(request, out var date))
        {
            return Result.Fail(FailCodes.InvalidDateInput);
        }

        ExecutionContext.Draft.Date = date.Date;
        await ExecutionContext.Session.FireDateSubmittedAsync();

        return Result.Success();
    }
    
    private static bool ParseDateFromRequest(Update update, out DateTime date) 
        => DateTime.TryParseExact(update.GetMessage(), Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
}