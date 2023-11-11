namespace Horoscope.Admin.Bot.Session;

public enum State
{
    BeforeStart,
    WaitingForApiKey,
    StartingEditHoroscope,
    WaitingForDate,
    WaitingForLanguage,
    WaitingForSign,
    WaitingForForesight,
    ForesightProvided,
    Preview
}