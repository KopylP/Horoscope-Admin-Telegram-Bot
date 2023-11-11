namespace Horoscope.Admin.Bot.Framework.Extensions;

public static class ArrayExtensions
{
    public static T[][] ToTwoDimensionArray<T>(this IEnumerable <T> array, int rowSize)
    {
        return array
            .Select((value, index) => new { Index = index, Value = value })
            .GroupBy(item => item.Index / rowSize)
            .Select(group => group.Select(g => g.Value).ToArray())
            .ToArray();
    }
}