using System.Diagnostics.CodeAnalysis;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class UpdateExtensions
{
    public static string GetMessage(this Update update)
        => update.Message?.Text?.Trim() ?? string.Empty;

    public static bool IsExcelExt(this Document document)
    {
        return document.FileName!.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase);
    }
}