using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using Xunit;

namespace DimonSmart.Hash.Tests;

[RPlotExporter]
[DryJob]
public class HashBenchmarkTests
{
    private static readonly byte[] Bytes = GenerateRandomByteArray(1024);


    [Params(1, 2, 3, 4, 5, 6, 7, 8, 16, 32, 64, 128)]
    public int N = 1;
    public int HashSize { get; set; }

    [Benchmark]
    [Fact]
    public void XorHashTest()
    {
        var hashAlgorithm = new XorHash(N);
        var result = hashAlgorithm.ComputeHash(Bytes);
    }

    [Benchmark(Baseline = true)]
    [Fact]
    public void FirstBytesHashTest()
    {
        var hashAlgorithm = new FirstBytesHashAlgorithm(N);
        var result = hashAlgorithm.ComputeHash(Bytes);
    }

    [Fact]
    public void BenchMarkRunner()
    {
        var config =  ManualConfig.CreateMinimumViable();
        // config.AddExporter(CsvMeasurementsExporter.Default);
        config.AddExporter(RPlotExporter.Default);

        BenchmarkRunner.Run<HashBenchmarkTests>(config);
    }

    public static byte[] GenerateRandomByteArray(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Length must be greater than zero.");
        }

        var byteArray = new byte[length];
        var random = new Random();
        random.NextBytes(byteArray);
        return byteArray;
    }
}