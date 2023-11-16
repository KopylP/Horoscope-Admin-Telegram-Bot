using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Horoscope.Admin.Bot.Framework.Extensions;

namespace Horoscope.Admin.Bot.Framework.Helpers;

public static class EnumHelpers
{
    public static IEnumerable<T> GetEnumValues<T>(bool skipFirst = false) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().Skip(skipFirst ? 1 : 0).ToArray();
    }

    public static IEnumerable<string> GetDisplayNames<T>(bool skipFirst = false) where T : Enum
    {
        return GetEnumValues<T>(skipFirst).Select(p => p.GetDisplayName())
            .ToArray();
    }
    
    public static bool TryGetEnumValueFromDisplayName<T>(string displayName, out T? enumValue) where T : Enum
    {
        enumValue = default;
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attribute = field.GetCustomAttribute<DisplayAttribute>();
            if (attribute?.GetName() == displayName)
            {
                enumValue = (T?)field.GetValue(null);
                return true;
            }
        }

        try
        {
            enumValue = displayName.ToEnum<T>();
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}