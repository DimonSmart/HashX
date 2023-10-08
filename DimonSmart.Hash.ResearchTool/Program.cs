using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;

namespace DimonSmart.Hash.ResearchTool;

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
                var files = Directory.EnumerateFiles(rootFolder)
                    .Where(f => new FileInfo(f).Length > maxBufferSize)
                    .ToList();
                var finalResults = new ConcurrentBag<UniqueCalculationResult>();
                for (var bufferSize = minBufferSize; bufferSize < maxBufferSize; bufferSize++)
                {
                    Console.WriteLine($"BufferSize:{bufferSize}");
                    // ZeroHash = highest possible result
                    var zeroHashResult =
                        UniqueHashCalculator.Calculate(new ZeroHashAlgorithm(bufferSize), files, bufferSize);
                    finalResults.Add(zeroHashResult);
                    Console.WriteLine(zeroHashResult);

                    // MD5 - like baseline (16 bytes)
                    var md5Result = UniqueHashCalculator.Calculate(new Md5HashAlgorithm(), files, bufferSize);
                    finalResults.Add(md5Result);
                    Console.WriteLine(md5Result);

                    // SHA1 - best results expectation
                    var sha1Result = UniqueHashCalculator.Calculate(new Sha1HashAlgorithm(), files, bufferSize);
                    finalResults.Add(sha1Result);
                    Console.WriteLine(sha1Result);

                    // XorHash. Size variation from minHashLength to MaxHashLength (SHA1 length)

                    var size = bufferSize;
                    Parallel.For(minHashLength, maxHashLength, i =>
                    {
                        var xorHashAlgorithm = HashFactory.CreateHashAlgorithm(i);
                        var xorResult = UniqueHashCalculator.Calculate(xorHashAlgorithm, files, size);
                        lock (files)
                        {
                            Console.WriteLine(xorResult);
                        }

                        finalResults.Add(xorResult);
                    });
                }

                File.WriteAllText("HashStatistics.json", JsonSerializer.Serialize(finalResults));
                foreach (var result in finalResults)
                {
                    Console.WriteLine($"{result}");
                }
            },
            minBufferSizeArg, maxBufferSizeArg, minHashLengthArg, maxHashLengthArg, rootFolderArg);
        rootCommand.Invoke(args);
    }
}