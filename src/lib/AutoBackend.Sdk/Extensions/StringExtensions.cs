namespace AutoBackend.Sdk.Extensions;

internal static class StringExtensions
{
    internal static string ToCamelCase(this string str)
    {
        return char.ToLowerInvariant(str[0]) + str[1..];
    }

    internal static string ToSnakeCase(this string str)
    {
        return string
            .Concat(
                str
                    .Select((x, i) =>
                        i > 0 && char.IsUpper(x) &&
                        (char.IsLower(str[i - 1]) || i < str.Length - 1 && char.IsLower(str[i + 1]))
                            ? "_" + x
                            : x.ToString()))
            .ToLowerInvariant();
    }
}