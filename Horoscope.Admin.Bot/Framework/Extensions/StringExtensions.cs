namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class StringExtensions
{
    public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }
    
    public static string? RemoveSubstringFromEnd(this string? source, string? toRemove)
    {
        if (source == null) return null;
        if (toRemove == null || !source.EndsWith(toRemove)) return source;

        return source.Substring(0, source.Length - toRemove.Length);
    }
    
    public static string EscapeMarkdown(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        char[] specialChars = new char[] { '_', '[', ']', '(', ')', '~', '`', '>', '#', '+', '-', '=', '|', '{', '}', '.', '!' };
        foreach (var specialChar in specialChars)
        {
            text = text.Replace(specialChar.ToString(), "\\" + specialChar);
        }

        return text;
    }
}