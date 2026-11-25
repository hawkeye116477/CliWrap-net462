using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CliWrap.PowerKit.Extensions;

/// <summary>
/// Extensions for <see cref="Stream" />.
/// </summary>
internal static class StreamExtensions
{
    extension(Stream source)
    {

        /// <summary>
        /// Copies the contents of the stream to the destination stream, optionally flushing after each write.
        /// </summary>
        public async Task CopyToAsync(
            Stream destination,
            bool autoFlush,
            CancellationToken cancellationToken = default
        )
        {
            using var buffer = MemoryPool<byte>.Shared.Rent(81920);

            while (
                await source.ReadAsync(buffer.Memory, cancellationToken).ConfigureAwait(false)
                    is > 0
                        and var bytesRead
            )
            {
                await destination
                    .WriteAsync(buffer.Memory[..bytesRead], cancellationToken)
                    .ConfigureAwait(false);

                if (autoFlush)
                {
                    await destination.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}