namespace Horoscope.Admin.Bot.Framework.Chains;

public interface IChainOfResponsibilityHandler<in TRequest>
{
    Task HandleAsync(TRequest request);
}