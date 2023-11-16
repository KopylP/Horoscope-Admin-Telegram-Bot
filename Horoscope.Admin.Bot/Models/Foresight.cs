using Horoscope.Admin.Bot.Framework.Comparers;

namespace Horoscope.Admin.Bot.Models;

public sealed record Foresight : IFormattable
{
    public bool IsEmpty => !Values.Any();
    public string[] Values { get; }

    public Foresight(string? input)
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

    public Foresight(string[]? values) => Values = values ?? Array.Empty<string>();
    
    public static explicit operator Foresight(string? input) => new(input);

    public static explicit operator Foresight(string[]? values) => new(values);

    public static explicit operator string?(Foresight? foresight) => foresight?.ToString();
    
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        string separator = format switch
        {
            "n" => "\n",
            "nn" => "\n\n",
            "c" => ", ",
            _ => " | "
        };

        return string.Join(separator, Values);
    }

    public string ToString(string? format) => ToString(format, default);

    public override string ToString() => ToString(null, null);
}
