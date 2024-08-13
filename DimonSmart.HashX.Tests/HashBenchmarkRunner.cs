using DimonSmart.TinyBenchmark;
using Xunit;
using Xunit.Abstractions;
using static DimonSmart.TinyBenchmark.Exporters.IGraphExporter.GraphExportOption;

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
            .WinMinMaxFunctionExecutionCount(1000, 100000)
            .WithMaxRunExecutionTime(TimeSpan.FromSeconds(30))
            .Run()
            //.WithCsvExporter()
            //.SaveRawResults()
            .WithGraphExporter()
            //.ExportAllRawGraph(SortTimeDirection.AscendingTimes)
            //.ExportAllRawGraph()
            .ExportAllFunctionsCompareGraph(IncludeErrorMarks);
    }
}