# CliWrap.Benchmarks

All benchmarks below were ran with the following configuration:

```ini
BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 5 7530U with Radeon Graphics 3.76GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.201
  [Host]     : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
```

## Basic benchmarks

Run a process, wait for completion, and return the exit code.

```ini
| Method           | Mean     | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|----------------- |---------:|---------:|---------:|------:|--------:|----------:|------------:|
| CliWrap          | 48.64 ms | 0.876 ms | 0.777 ms |  1.00 |    0.02 |  64.34 KB |        1.00 |
| MedallionShell   | 48.66 ms | 0.955 ms | 0.938 ms |  1.00 |    0.02 |   77.9 KB |        1.21 |
| RunProcessAsTask | 48.82 ms | 0.947 ms | 1.013 ms |  1.00 |    0.03 |  69.56 KB |        1.08 |
```

## Buffering benchmarks

Run a process, read standard output and error, wait for completion, and return buffered output and error data.
Target program writes a total of 1 million characters to each stream.

```ini
| Method           | Mean    | Error    | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2      | Allocated | Alloc Ratio |
|----------------- |--------:|---------:|---------:|------:|--------:|-----------:|-----------:|----------:|----------:|------------:|
| ProcessX         | 1.397 s | 0.0277 s | 0.0406 s |  0.95 |    0.03 |  3000.0000 |  3000.0000 | 3000.0000 | 382.81 MB |        0.96 |
| RunProcessAsTask | 1.444 s | 0.0289 s | 0.0270 s |  0.98 |    0.02 |  3000.0000 |  3000.0000 | 3000.0000 | 382.73 MB |        0.96 |
| CliWrap          | 1.472 s | 0.0262 s | 0.0245 s |  1.00 |    0.02 | 34000.0000 | 22000.0000 | 3000.0000 | 397.78 MB |        1.00 |
| MedallionShell   | 1.649 s | 0.0328 s | 0.0391 s |  1.12 |    0.03 | 33000.0000 | 30000.0000 | 7000.0000 | 661.07 MB |        1.66 |

```

## Async event stream benchmarks

Run a process as a pull-based event stream and return the number of lines written to each stream.
Target program writes a total of 1 million characters to each stream.

```ini
| Method   | Mean    | Error    | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|--------- |--------:|---------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| ProcessX | 1.327 s | 0.0251 s | 0.0258 s |  0.97 |    0.03 | 62000.0000 | 62000.0000 | 62000.0000 | 192.05 MB |        0.93 |
| CliWrap  | 1.370 s | 0.0268 s | 0.0338 s |  1.00 |    0.03 | 51000.0000 | 49000.0000 | 49000.0000 | 205.53 MB |        1.00 |
```

## Observable event stream benchmarks

Run a process as a push-based event stream and return the number of lines written to each stream.
Target program writes a total of 1 million characters to each stream.

```ini
| Method  | Mean    | Error    | StdDev   | Ratio | RatioSD | Gen0       | Gen1       | Gen2       | Allocated | Alloc Ratio |
|-------- |--------:|---------:|---------:|------:|--------:|-----------:|-----------:|-----------:|----------:|------------:|
| CliWrap | 1.366 s | 0.0268 s | 0.0286 s |  1.00 |    0.03 | 46000.0000 | 44000.0000 | 44000.0000 | 206.67 MB |        1.00 |
```

## Pipe from stream benchmarks

Run a process and pipe a stream into standard input.

```ini
| Method         | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |---------:|---------:|---------:|---------:|------:|--------:|----------:|------------:|
| CliWrap        | 57.02 ms | 1.121 ms | 1.049 ms | 57.10 ms |  1.00 |    0.03 |  66.01 KB |        1.00 |
| MedallionShell | 57.64 ms | 1.148 ms | 2.794 ms | 56.62 ms |  1.01 |    0.05 |  84.46 KB |        1.28 |
```

## Pipe to stream benchmarks

Run a process and pipe the standard output into a memory stream.
Target program writes a total of 1 million bytes to each stream.

```ini
| Method         | Mean     | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |---------:|---------:|---------:|------:|--------:|----------:|------------:|
| CliWrap        | 55.62 ms | 0.874 ms | 0.818 ms |  1.00 |    0.02 |  344.9 KB |        1.00 |
| MedallionShell | 55.88 ms | 0.576 ms | 0.511 ms |  1.00 |    0.02 | 440.01 KB |        1.28 |
```

## Pipe to multiple streams benchmarks

Run a process and pipe the standard output into two memory streams.
Target program writes a total of 1 million bytes to each stream.

```ini
| Method  | Mean     | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
|-------- |---------:|---------:|---------:|------:|--------:|----------:|------------:|
| CliWrap | 55.79 ms | 1.075 ms | 1.006 ms |  1.00 |    0.02 | 717.49 KB |        1.00 |
```
