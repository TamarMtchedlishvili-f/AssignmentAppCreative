using System.Collections.Immutable;
using System.Globalization;
using RestSharp;

namespace AssignmentAppCreative.HelperClasses;

public static class RestRequestExtensions
{
    public static bool IsNullOrEmpty(this string? text) => string.IsNullOrEmpty(text);
    public static bool IsNotNullOrEmpty(this string? text) => !text.IsNullOrEmpty();

    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }

    // ReSharper disable once InconsistentNaming
    public static RestRequest AddAPIKey(this RestRequest request)
        => request.AddParameter("appid", "750047cf32f3161bd1d50cbb8646cf49");

    public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> items)
    {
        var itemsList = items.ToImmutableList();
        await Task.WhenAll(itemsList);

        return itemsList.Select(i => i.Result);
    }
}