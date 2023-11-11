using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class UpdateExtensions
{
    public static string GetMessage(this Update update)
        => update.Message?.Text?.Trim() ?? string.Empty;
}