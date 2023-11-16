using ClosedXML.Excel;
using Horoscope.Admin.Bot.Framework.Helpers;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class CellExtensions
{
    public static DateTime? GetDateOrNull(this IXLCell cell)
    {
        if (cell.TryGetValue(out DateTime dateValue))
        {
            return dateValue;
        }

        return null;
    }

    public static TEnum? GetEnumByDisplayNameOrNull<TEnum>(this IXLCell cell) where TEnum : Enum
    {
        if (cell.TryGetValue(out string cellValue) &&
            EnumHelpers.TryGetEnumValueFromDisplayName<TEnum>(cellValue, out var enumValue))
        {
            return enumValue;
        }

        return default;
    }
}