using Horoscope.Admin.Bot.Framework.Sessions;

namespace Horoscope.Admin.Bot.Session;

public static class SessionStateExtensions
{
    public static Task FireStartAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.Start);
    }

    public static Task FireApiKeyProvidedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.ApiKeyProvided);
    }

    public static Task FireBackAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.Back);
    }

    public static Task FireCancelAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.Cancel);
    }

    public static Task FireNextSignAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.NextSign);
    }

    public static Task FireNextDateAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.NextDate);
    }

    public static Task FireGoToChangeDescriptionAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.GoToChangeDescription);
    }

    public static Task FireEditHoroscopeStartedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.EditHoroscopeStarted);
    }

    public static Task FireDateProvidedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.DateProvided);
    }

    public static Task FireForesightProvidedAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.ForesightProvided);
    }

    public static Task FireGoToPreviewAsync(this SessionState<State, Trigger> session)
    {
        return session.FireAsync(Trigger.GoToPreview);
    }
}