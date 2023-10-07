using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;
using DimonSmart.AnyHash;

namespace DimonSmart.AnyHas.ResearchTool;

internal class Program
{
    private static void Main(string[] args)
    {
        var minBufferSizeArg = new Option<int>(
            "--MinBufferSize",
            () => 1,
            "Max Buffer size (hashed block), bytes.");
        var maxBufferSizeArg = new Option<int>(
            "--MaxBufferSize",
            () => 1,
            "Max Buffer size (hashed block), bytes.");
        var minHashLengthArg = new Option<int>(
            "--MinHashLength",
            () => 2,
            "Min Hash length, bytes.");
        var maxHashLengthArg = new Option<int>(
            "--MaxHashLength",
            () => 2,
            "Max Hash length, bytes.");


        var rootFolderArg = new Argument<string>(
            "--RootFolder",
            "Root folder for hash collision statistic calculation.");

        var rootCommand = new RootCommand("Hash collision research");
        rootCommand.AddOption(minBufferSizeArg);
        rootCommand.AddOption(maxBufferSizeArg);
        rootCommand.AddOption(minHashLengthArg);
        rootCommand.AddOption(maxHashLengthArg);
        rootCommand.AddArgument(rootFolderArg);

        rootCommand.SetHandler(
            (minBufferSize, maxBufferSize, minHashLength, maxHashLength, rootFolder) =>
            {
                var files = Directory.EnumerateFiles(rootFolder).ToList();
                var results = new ConcurrentBag<UniqueCalculationResult>();
                for (int bufferSize = minBufferSize; bufferSize < maxBufferSize; bufferSize++)
                {
                    Console.WriteLine($"BufferSize:{bufferSize}");

                    // MD5 - like baseline (16 bytes)
                    var md5Result = UniqueHashCalculator.Calculate(new Md5HashAlgorithm(), files, bufferSize);
                    results.Add(md5Result);
                    Console.WriteLine($"MD5:{md5Result}");

                    // SHA1 - best results expectation
                    var sha1Result = UniqueHashCalculator.Calculate(new Sha1HashAlgorithm(), files, bufferSize);
                    results.Add(sha1Result);
                    Console.WriteLine($"MD5:{sha1Result}");

                    // XorHash. Size variation from minHashLength to MaxHashLength (SHA1 length)

                    Parallel.For(minHashLength, maxHashLength, i =>
                    {
                        var xorHashAlgorithm = AnyHashFactory.CreateHashAlgorithm(i);
                        results.Add(UniqueHashCalculator.Calculate(xorHashAlgorithm, files, bufferSize));
                    });
                }

                File.WriteAllText("HashStatistics.json", JsonSerializer.Serialize(results));
                foreach (var result in results)
                {
                    Console.WriteLine($"{result}");
                }
            },
            minBufferSizeArg, maxBufferSizeArg, minHashLengthArg, maxHashLengthArg, rootFolderArg);
        rootCommand.Invoke(args);
    }
}