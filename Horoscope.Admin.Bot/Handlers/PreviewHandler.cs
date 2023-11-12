using Horoscope.Admin.Bot.Commands;
using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Extensions;
using Horoscope.Admin.Bot.Framework.Sessions;
using Horoscope.Admin.Bot.Messages;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class PreviewHandler: SessionBasedHandler<NewtonsoftJsonUpdate>
{
    private const string ErrorMessage = "Не знаю такої опції \ud83d\ude14 Вибери ще!";
    
    private readonly IMessageFactory _messageFactory;
    
    public PreviewHandler(IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next, IMessageFactory messageFactory) 
        : base(next, State.Preview)
    {
        _messageFactory = messageFactory;
    }

    protected override async Task StateMatchedHandleAsync(NewtonsoftJsonUpdate request)
    {
        switch (request.GetMessage())
        {
            case ReplyCommands.Preview.Edit:
                await HandleEditCommandAsync();
                break;
            
            default:
                await SendErrorMessageAsync();
                break;
        }
    }
    
    private async Task HandleEditCommandAsync()
    {
        await ExecutionContext.Session
            .FireInitiateDescriptionChangeAsync();
    }

    private async Task SendErrorMessageAsync()
    {
        await _messageFactory.CreateStandardMessage(ErrorMessage).SendAsync();
    }
}