using Horoscope.Admin.Bot.Framework.Chains;
using Horoscope.Admin.Bot.Framework.Results;
using Horoscope.Admin.Bot.Infrastructure.Repositories;
using Horoscope.Admin.Bot.Models;
using Horoscope.Admin.Bot.Session;

namespace Horoscope.Admin.Bot.Handlers;

public sealed class ExecutionContextSetupHandler: IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>
{
    private readonly ISessionStateProvider _sessionStateProvider;
    private readonly IDraftRepository _draftRepository;
    private readonly IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? _next;

    public ExecutionContextSetupHandler(
        IChainOfResponsibilityHandler<NewtonsoftJsonUpdate>? next,
        ISessionStateProvider sessionStateProvider,
        IDraftRepository draftRepository)
    {
        _sessionStateProvider = sessionStateProvider;
        _next = next;
        _draftRepository = draftRepository;
    }

    public async Task<Result> HandleAsync(NewtonsoftJsonUpdate update)
    {
        var chatId = update.Message!.Chat.Id;
        var sessionState = await _sessionStateProvider.GetStateAsync(chatId);
        var draft = await _draftRepository.GetAsync(chatId);
        
        ExecutionContext.Apply(sessionState);
        ExecutionContext.Apply(chatId);
        ExecutionContext.Apply(draft);

        if (_next is not null)
        {
            var result = await _next!.HandleAsync(update);

            await result.OnSuccess(async (_) =>
            {
                await _sessionStateProvider.UpdateAsync(chatId, ExecutionContext.Session);
                await _draftRepository.SaveAsync(chatId, ExecutionContext.Draft);
            });

            return result;
        }

        return Result.Success();
    }
}