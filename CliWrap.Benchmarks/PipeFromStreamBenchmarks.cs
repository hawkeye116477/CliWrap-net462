using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace CliWrap.Benchmarks;

[MemoryDiagnoser, Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class PipeFromStreamBenchmarks
{
    [Benchmark(Baseline = true)]
    public async Task<Stream> CliWrap()
    {
        await using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        var command =
            stream | Cli.Wrap(Tests.Dummy.Program.FilePath).WithArguments(["echo", "stdin"]);

        await command.ExecuteAsync();

        return stream;
    }

    [Benchmark]
    public async Task<Stream> MedallionShell()
    {
        await using var stream = new MemoryStream([1, 2, 3, 4, 5]);

        var command =
            Medallion.Shell.Command.Run(Tests.Dummy.Program.FilePath, ["echo", "stdin"]) < stream;

        await command.Task;

        return stream;
    }
}
