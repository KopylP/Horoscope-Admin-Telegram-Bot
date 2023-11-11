namespace Horoscope.Admin.Bot.Framework.Comparers;

public class CaseInsensitiveComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode(string obj)
    {
        return obj.ToLowerInvariant().GetHashCode();
    }
}