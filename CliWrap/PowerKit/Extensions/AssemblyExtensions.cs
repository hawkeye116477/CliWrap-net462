using System.IO;
using System.Reflection;
using System.Resources;

namespace CliWrap.PowerKit.Extensions;

internal static class AssemblyExtensions
{
    extension(Assembly assembly)
    {
        /// <summary>
        /// Extracts the specified manifest resource to a file at the given path.
        /// Throws <see cref="MissingManifestResourceException" /> if the resource is not found.
        /// </summary>
        public void ExtractManifestResource(string resourceName, string filePath)
        {
            using var source =
                assembly.GetManifestResourceStream(resourceName)
                ?? throw new MissingManifestResourceException(
                    $"Failed to find resource '{resourceName}'."
                );

            using var destination = File.Create(filePath);
            source.CopyTo(destination);
            destination.Flush();
        }
    }
}
