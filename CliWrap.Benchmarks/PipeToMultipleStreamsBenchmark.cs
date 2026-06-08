using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace CliWrap.Benchmarks;

[MemoryDiagnoser, Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class PipeToMultipleStreamsBenchmark
{
    [Benchmark(Baseline = true)]
    public async Task<(Stream, Stream)> CliWrap()
    {
        await using var stream1 = new MemoryStream();
        await using var stream2 = new MemoryStream();

        var target = PipeTarget.Merge(PipeTarget.ToStream(stream1), PipeTarget.ToStream(stream2));

        var command =
            Cli.Wrap(Tests.Dummy.Program.FilePath).WithArguments(["generate", "binary"]) | target;

        await command.ExecuteAsync();

        return (stream1, stream2);
    }
}
