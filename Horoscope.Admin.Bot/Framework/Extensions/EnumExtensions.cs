using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
                   .GetField(enumValue.ToString())
                   ?.GetCustomAttribute<DisplayAttribute>()
                   ?.GetName()
               ?? enumValue.ToString();
    }
}