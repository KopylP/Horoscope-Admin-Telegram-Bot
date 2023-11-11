using Horoscope.Admin.Bot.Framework.Comparers;

namespace Horoscope.Admin.Bot.Models;

public sealed record Foresight
{
    public bool IsEmpty => !Values.Any();
    public string[] Values { get; }

    private Foresight(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Values = Array.Empty<string>();
        }
        else
        {
            Values = input
                .Split('|')
                .Select(s => s.Trim())
                .Distinct(new CaseInsensitiveComparer())
                .ToArray();
        }
    }

    private Foresight(string[]? values) => Values = values ?? Array.Empty<string>();
    
    public static explicit operator Foresight(string? input) => new(input);

    public static explicit operator Foresight(string[]? values) => new(values);

    public static explicit operator string?(Foresight? foresight) => foresight?.ToString();
    
    public override string ToString() => string.Join(" | ", Values);
}
