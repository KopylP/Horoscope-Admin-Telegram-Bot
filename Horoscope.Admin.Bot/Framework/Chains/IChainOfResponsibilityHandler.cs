using Horoscope.Admin.Bot.Framework.Results;

namespace Horoscope.Admin.Bot.Framework.Chains;

public interface IChainOfResponsibilityHandler<in TRequest>
{
    Task<Result> HandleAsync(TRequest request);
}