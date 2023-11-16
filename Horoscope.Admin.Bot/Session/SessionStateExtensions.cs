using Horoscope.Admin.Bot.Framework.Sessions;

namespace Horoscope.Admin.Bot.Session;

public static class SessionStateExtensions
{
    public static Task FireStartAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.Start);
    }

    public static Task FireApiKeySubmittedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.ApiKeySubmitted);
    }

    public static Task FireNavigateBackAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.NavigateBack);
    }

    public static Task FireNavigateBeginningAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.NavigateBeginning);
    }

    public static Task FireAdvanceToNextSignAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.AdvanceToNextSign);
    }

    public static Task FireAdvanceToNextDateAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.AdvanceToNextDate);
    }

    public static Task FireInitiateDescriptionChangeAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.InitiateDescriptionChange);
    }

    public static Task FireBeginHoroscopeEditingManuallyAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.BeginHoroscopeEditingManually);
    }

    public static Task FireDateSubmittedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.DateSubmitted);
    }

    public static Task FireForesightSubmittedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.ForesightSubmitted);
    }

    public static Task FireProceedToPreviewAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.ProceedToPreview);
    }

    public static Task FireInitiateLoadFileAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.InitiateLoadFile);
    }
}