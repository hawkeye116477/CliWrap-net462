#pragma warning disable CS0436

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal static class MemberPolyfills_NetCore21_TextReader
{
    extension(TextReader reader)
    {
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.read#system-io-textreader-read(system-span((system-char)))
        public int Read(Span<char> buffer)
        {
            var bufferArray = buffer.ToArray();
            var result = reader.Read(bufferArray, 0, bufferArray.Length);
            bufferArray.CopyTo(buffer);

            return result;
        }

        // Task infrastructure is required for async method support
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.readasync#system-io-textreader-readasync(system-memory((system-char))-system-threading-cancellationtoken)
        public async ValueTask<int> ReadAsync(
            Memory<char> buffer,
            CancellationToken cancellationToken = default
        )
        {
            var bufferArray = buffer.ToArray();

            cancellationToken.ThrowIfCancellationRequested();

            var result = await reader
                .ReadAsync(bufferArray, 0, bufferArray.Length)
                .ConfigureAwait(false);

            bufferArray.CopyTo(buffer);

            return result;
        }
    }
}

internal static class MemberPolyfills_Net70_TextReader
{
    extension(TextReader reader)
    {
        // Task infrastructure is required for async method support
        // https://learn.microsoft.com/dotnet/api/system.io.textreader.readlineasync#system-io-textreader-readlineasync(system-threading-cancellationtoken)
        public Task<string?> ReadLineAsync(CancellationToken cancellationToken)
        {
            // Impossible to polyfill this properly as it requires to track the buffer's state
            cancellationToken.ThrowIfCancellationRequested();
            return reader.ReadLineAsync();
        }
    }
}