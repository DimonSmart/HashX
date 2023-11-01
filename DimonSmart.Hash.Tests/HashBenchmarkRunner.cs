using DimonSmart.TinyBenchmark;
using Xunit;
using Xunit.Abstractions;

namespace DimonSmart.Hash.Tests;

public class HashBenchmarkRunner
{
    private readonly ITestOutputHelper _output;

    public HashBenchmarkRunner(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void RunAllBenchmarks()
    {
        TinyBenchmarkRunner
            .Create(_output.WriteLine)
            .WithRunCountLimits(500, 2000)
            .WithMaxRunExecutionTime(TimeSpan.FromSeconds(15))
            // .WithBestTimeAsResult()
            .Run()
            .WithCsvExporter()
            .SaveRawResults()
            .WithGraphExporter()
            .ExportAllRawGraph(SortTimeDirection.AscendingTimes)
            .ExportAllRawGraph()
            .ExportAllFunctionsCompareGraph();
    }
}