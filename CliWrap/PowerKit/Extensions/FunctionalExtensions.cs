namespace CliWrap.PowerKit.Extensions;

/// <summary>
/// Extensions for functional programming concepts and patterns.
/// </summary>
internal static class FunctionalExtensions
{
    extension(string value)
    {

        /// <summary>
        /// Returns <see langword="null" /> if the string is <see langword="null" />, empty, or consists only of whitespace;
        /// otherwise, returns the string.
        /// </summary>
        public string? NullIfWhiteSpace() => !string.IsNullOrWhiteSpace(value) ? value : null;
    }
}
