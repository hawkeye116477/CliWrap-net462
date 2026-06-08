using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace CliWrap.Benchmarks;

[MemoryDiagnoser, Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class PipeToStreamBenchmarks
{
    [Benchmark(Baseline = true)]
    public async Task<Stream> CliWrap()
    {
        await using var stream = new MemoryStream();

        var command =
            Cli.Wrap(Tests.Dummy.Program.FilePath).WithArguments(["generate", "binary"]) | stream;

        await command.ExecuteAsync();

        return stream;
    }

    [Benchmark]
    public async Task<Stream> MedallionShell()
    {
        await using var stream = new MemoryStream();

        var command =
            Medallion.Shell.Command.Run(Tests.Dummy.Program.FilePath, ["generate", "binary"])
            > stream;

        await command.Task;

        return stream;
    }
}
