namespace Horoscope.Admin.Bot.Session;

public enum State
{
    BeforeStart,
    AwaitingApiKey,
    BeginningHoroscopeEdit,
    AwaitingDateInput,
    AwaitingFile,
    AwaitingLanguageSelection,
    AwaitingZodiacSign,
    AwaitingForesightInput,
    ForesightInputReceived,
    Preview
}

public static class StateExtensions
{
    public static bool IsBeforeStart(this State state) => state == State.BeforeStart;
}