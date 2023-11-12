namespace Horoscope.Admin.Bot.Session;

public enum Trigger
{
    Start,
    ApiKeySubmitted,
    BeginHoroscopeEditing,
    InitiateDescriptionChange,
    DateSubmitted,
    LanguageSelectionSubmitted,
    ForesightSubmitted,
    AdvanceToNextSign,
    AdvanceToNextLanguage,
    ProceedToPreview,
    AdvanceToNextDate,
    NavigateBack,
    NavigateBeginning
}