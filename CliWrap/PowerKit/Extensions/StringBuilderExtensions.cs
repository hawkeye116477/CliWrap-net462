using System.Text;

namespace CliWrap.PowerKit.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        /// <summary>
        /// Appends the specified character only if the builder is not empty.
        /// </summary>
        public StringBuilder AppendIfNotEmpty(char value) =>
            builder.Length > 0 ? builder.Append(value) : builder;
    }
}
