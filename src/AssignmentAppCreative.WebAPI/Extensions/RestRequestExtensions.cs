using System.Collections.Immutable;
using System.Globalization;

namespace AssignmentAppCreative.WebAPI.Extensions;

public static class RestRequestExtensions
{
    public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);
    public static bool IsNotNullOrEmpty(this string? text) => !text.IsNullOrEmpty();

    public static string ToTitleCase(this string title) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());

    public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> items)
    {
        var itemsList = items.ToImmutableList();
        await Task.WhenAll(itemsList);

        return itemsList.Select(i => i.Result);
    }
}