namespace Horoscope.Admin.Bot.Resources;

public static class Shared
{
    public static readonly IReadOnlyDictionary<string, string> Messages = new Dictionary<string, string>
    {
        [FailCodes.InvalidApiKey] = "Ключ не підходить \ud83d\ude22 Спробуй ще!",
        [FailCodes.InvalidDateInput] = "Щось якась дивна дата \ud83d\ude22 Спробуй ще!",
        [FailCodes.InvalidZodiacSign] = "Я не знаю такого знаку \ud83d\ude22 Спробуй ще!",
        [FailCodes.FailInput] = "Щось не збігається \ud83d\ude22 Спробуй ще!",
        [FailCodes.OnlyStartCommandAllowed] = "Дозволено тільки команду /start",
        [FailCodes.InvalidFileFormat] = "Цей файл не підтримується \ud83d\ude22 Спробуй інший!",
    };
}