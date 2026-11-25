using System;

namespace CliWrap.PowerKit.Extensions;

/// <summary>
/// Extensions for functional programming concepts and patterns.
/// </summary>
internal static class FunctionalExtensions
{
    extension<TIn>(TIn input)
    {
        /// <summary>
        /// Passes the value through the specified transform function and returns the result.
        /// </summary>
        public TOut Pipe<TOut>(Func<TIn, TOut> transform) => transform(input);
    }
    extension(string value)
    {

        /// <summary>
        /// Returns <see langword="null" /> if the string is <see langword="null" />, empty, or consists only of whitespace;
        /// otherwise, returns the string.
        /// </summary>
        public string? NullIfWhiteSpace() => !string.IsNullOrWhiteSpace(value) ? value : null;
    }
}
