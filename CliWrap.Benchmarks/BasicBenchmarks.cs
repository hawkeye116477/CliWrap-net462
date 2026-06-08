using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using RunProcessAsTask;

namespace CliWrap.Benchmarks;

[MemoryDiagnoser, Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class BasicBenchmarks
{
    [Benchmark(Baseline = true)]
    public async Task<int> CliWrap()
    {
        var result = await Cli.Wrap(Tests.Dummy.Program.FilePath).ExecuteAsync();
        return result.ExitCode;
    }

    [Benchmark]
    public async Task<int> RunProcessAsTask()
    {
        var result = await ProcessEx.RunAsync(Tests.Dummy.Program.FilePath);
        return result.ExitCode;
    }

    [Benchmark]
    public async Task<int> MedallionShell()
    {
        var result = await Medallion.Shell.Command.Run(Tests.Dummy.Program.FilePath).Task;
        return result.ExitCode;
    }
}
