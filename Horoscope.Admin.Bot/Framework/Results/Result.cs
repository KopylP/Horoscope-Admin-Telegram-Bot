namespace Horoscope.Admin.Bot.Framework.Results;

public record Result
{
    public virtual bool IsSuccess { get; }
    public virtual string? FailCode { get; }
    public bool IsFail => !IsSuccess;

    protected Result()
    {
    }
    
    public Result(bool isSuccess, string? failCode)
    {
        IsSuccess = isSuccess;
        FailCode = failCode;
    }

    public static Result Success() => new Result(isSuccess: true, failCode: default);
    
    public static Result Fail(string errorCode)
        => new Result(isSuccess: false, errorCode);


    public TMatch? Match<TMatch>(Func<Result, TMatch>? onSuccess, Func<Result, TMatch>? onFail)
    {
        if (IsSuccess && onSuccess is not null)
            return onSuccess(this);

        if (IsFail && onFail is not null)
            return onFail(this);

        return default;
    }
    
    public async Task OnSuccess(Func<Result, Task>? onSuccess)
    {
        if (IsSuccess && onSuccess is not null)
        {
            await onSuccess(this);
        }
    }
    
    public async Task OnFail(Func<Result, Task>? onFail)
    {
        if (IsFail && onFail is not null)
        {
            await onFail(this);
        }
    }
}


public sealed record ResultList : Result
{
    private enum SuccessStrategy
    {
        AllSuccess, // Success if all are successful
        NoneFail, // Success if none failed
        AnySuccess // Success if at least one is successful
    }

    public override bool IsSuccess => IsSuccessBasedOnStrategy();
    public override string? FailCode => DetermineFailCode();

    private readonly SuccessStrategy _strategy;
    private readonly List<Result> _results;

    private ResultList(SuccessStrategy strategy)
    {
        _strategy = strategy;
        _results = new List<Result>();
    }

    public void AddResults(params Result[] results)
    {
        if (results is null)
            throw new ArgumentException("Result cannot be null!");
        
        _results.AddRange(results);
    }

    public IEnumerable<Result> SuccessResults => _results.Where(p => p.IsSuccess);
    
    public IEnumerable<Result> FailResults => _results.Where(p => p.IsFail);

    public static ResultList CreateForAllSuccessStrategy() => new ResultList(SuccessStrategy.AllSuccess);
    public static ResultList CreateForNoneFailStrategy() => new ResultList(SuccessStrategy.NoneFail);
    public static ResultList CreateForAnySuccessStrategy() => new ResultList(SuccessStrategy.AnySuccess);
    
    private bool IsSuccessBasedOnStrategy()
    {
        return _strategy switch
        {
            SuccessStrategy.AllSuccess => _results.All(r => r.IsSuccess),
            SuccessStrategy.NoneFail => _results.All(r => !r.IsFail),
            SuccessStrategy.AnySuccess => _results.Any(r => r.IsSuccess),
            _ => throw new NotImplementedException()
        };
    }
    
    private string? DetermineFailCode()
    {
        if (IsSuccessBasedOnStrategy()) return null;

        // Example fail code determination, can be customized
        return _strategy switch
        {
            SuccessStrategy.AllSuccess => "NotAllSuccess",
            SuccessStrategy.NoneFail => "AtLeastOneFail",
            SuccessStrategy.AnySuccess => "NoneSuccess",
            _ => "UnknownFailure"
        };
    }
}