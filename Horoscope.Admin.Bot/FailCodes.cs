namespace Horoscope.Admin.Bot;

public static class FailCodes
{
    public const string InvalidApiKey = nameof(InvalidApiKey);
    public const string InvalidDateInput = nameof(InvalidDateInput);
    public const string InvalidZodiacSign = nameof(InvalidZodiacSign);
    public const string FailInput = nameof(FailInput);
    public const string OnlyStartCommandAllowed = nameof(OnlyStartCommandAllowed);
    public const string InvalidFileFormat = nameof(InvalidFileFormat);
}