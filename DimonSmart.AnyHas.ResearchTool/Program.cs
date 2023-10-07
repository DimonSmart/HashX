using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;
using DimonSmart.AnyHash;

namespace DimonSmart.AnyHas.ResearchTool;

internal class Program
{
    private static void Main(string[] args)
    {
        var bufferSizeArg = new Option<int>(
            "--BufferSize",
            () => 1,
            "Buffer size (hashed block), bytes.");
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
        rootCommand.AddOption(bufferSizeArg);
        rootCommand.AddOption(minHashLengthArg);
        rootCommand.AddOption(maxHashLengthArg);
        rootCommand.AddArgument(rootFolderArg);

        rootCommand.SetHandler(
            (bufferSize, minHashLength, maxHashLength, rootFolder) =>
            {
                var files = Directory.EnumerateFiles(rootFolder).ToList();
                var results = new ConcurrentBag<UniqueCalculationResult>();

                // MD5 - like baseline (16 bytes)
                results.Add(UniqueHashCalculator.Calculate(new Md5HashAlgorithm(), files, bufferSize));
                // SHA1 - best results expectation
                results.Add(UniqueHashCalculator.Calculate(new Sha1HashAlgorithm(), files, bufferSize));

                // XorHash. Size variation from minHashLength to MaxHashLength (SHA1 length)

                Parallel.For(minHashLength, maxHashLength, i =>
                {
                    var xorHashAlgorithm = AnyHashFactory.CreateHashAlgorithm(i);
                    results.Add(UniqueHashCalculator.Calculate(xorHashAlgorithm, files, bufferSize));
                });

                File.WriteAllText("HashStatistics.json", JsonSerializer.Serialize(results));
                foreach (var result in results)
                {
                    Console.WriteLine($"{result}");
                }
            },
            bufferSizeArg, minHashLengthArg, maxHashLengthArg, rootFolderArg);
        rootCommand.Invoke(args);
    }
}