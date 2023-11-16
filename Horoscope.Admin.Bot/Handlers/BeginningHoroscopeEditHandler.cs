using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class BeginningHoroscopeEditHandler : SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private readonly IMessageFactory _messageFactory;

    public BeginningHoroscopeEditHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        IMessageFactory messageFactory) 
        : base(next, State.BeginningHoroscopeEdit)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task<Result> StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        if (IsManuallyCommandReceived(request))
        {
            await ExecutionContext.Session
                .FireBeginHoroscopeEditingManuallyAsync();
        }
        else if (IsLoadFileCommandReceived(request))
        {
            await ExecutionContext.Session
                .FireInitiateLoadFileAsync();
        }
        else
        {
            return Result.Fail(FailCodes.FailInput);
        }

        return Result.Success();
    }

    private static bool IsManuallyCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.BeginningHoroscopeEdit.Manually;
    
    private static bool IsLoadFileCommandReceived(Update request)
        => request.GetMessage() == ReplyCommands.BeginningHoroscopeEdit.LoadFile;
}