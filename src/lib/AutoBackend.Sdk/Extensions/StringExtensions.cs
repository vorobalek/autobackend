namespace AutoBackend.Sdk.Extensions;

internal static class StringExtensions
{
    extension(string str)
    {
        internal string ToCamelCase()
        {
            return char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}