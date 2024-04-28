using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;

namespace DimonSmart.Hash.ResearchTool;

internal static class Program
{
    private static void Main(string[] args)
    {
        var algorithmsArg = new Option<IEnumerable<HashAlgorithm>>(
            "--Algorithm",
            "Algorithms to perform calculations");
        algorithmsArg.AddValidator(result =>
        {
            var values = result.Tokens.Select(t => t.Value).ToList();
            var duplicates = values.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            if (duplicates.Any())
            {
                result.ErrorMessage =
                    $"Duplicate algorithm(s) detected: {string.Join(", ", duplicates)}. Each algorithm should be specified only once.";
            }
        });
        var minBufferSizeArg = new Option<int>(
            "--MinBufferSize",
            () => 1,
            "Min Buffer size (hashed block), bytes.");
        var maxBufferSizeArg = new Option<int>(
            "--MaxBufferSize",
            () => 1,
            "Max Buffer size (hashed block), bytes.");
        var bufferSizeStepArg = new Option<int>(
            "--BufferSizeStep",
            () => 1,
            "Buffer size step from MinBufferSize to MaxBufferSize");
        var minHashLengthArg = new Option<int>(
            "--MinHashLength",
            () => 2,
            "Min Hash length, bytes.");
        var maxHashLengthArg = new Option<int>(
            "--MaxHashLength",
            () => 2,
            "Max Hash length, bytes.");
        var resultFileNameArg = new Option<string>(
            "--ResultFileName",
            () => "HashStatistics.json",
            "File name to store hash calculations statistics. (Ex.MyResults.json)");

        var rootFolderArg = new Option<string>(
            "--RootFolder",
            "Root folder for hash collision statistic calculation.");

        var uniqueCommand = new Command("unique", "Calculate unique hashes (vs collision)")
        {
            algorithmsArg, rootFolderArg, resultFileNameArg, minBufferSizeArg, maxBufferSizeArg
        };

        uniqueCommand.SetHandler((algorithms, rootFolder, resultFileName, minBufferSize, maxBufferSize) =>
        {
            Console.WriteLine($"Algorithms: {string.Join(", ", algorithms.Select(alg => alg.ToString()))}");
            Console.WriteLine($"Root Folder: {rootFolder}");
            Console.WriteLine($"Result File Name: {resultFileName}");
            Console.WriteLine($"Minimum Buffer Size: {minBufferSize}");
            Console.WriteLine($"Maximum Buffer Size: {maxBufferSize}");

            var files = Directory.EnumerateFiles(rootFolder)
                .Where(f => new FileInfo(f).Length > maxBufferSize)
                .ToList();
            var finalResults = new ConcurrentBag<UniqueCalculationResult>();
            for (var bufferSize = minBufferSize; bufferSize <= maxBufferSize; bufferSize++)
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
            }

            Console.WriteLine("Calculation finished");
            File.WriteAllText(resultFileName,
                JsonSerializer.Serialize(finalResults
                    .OrderBy(r => r.BufferSize)
                    .ThenBy(r => r.AlgorithmName)
                    .ThenBy(r => r.HashLength)
                    .ToArray()
                ));
            foreach (var result in finalResults)
            {
                Console.WriteLine($"{result}");
            }
        }, algorithmsArg, rootFolderArg, resultFileNameArg, minBufferSizeArg, maxBufferSizeArg);


        var hashSizeCommand = new Command("hash-size", "Calculate different hash sizes (for XorHashOnly");
        hashSizeCommand.SetHandler(
            (minBufferSize, maxBufferSize, minHashLength, maxHashLength, resultFileName, rootFolder) =>
            {
                var files = Directory.EnumerateFiles(rootFolder)
                    .Where(f => new FileInfo(f).Length > maxBufferSize)
                    .ToList();
                var finalResults = new ConcurrentBag<UniqueCalculationResult>();
                for (var bufferSize = minBufferSize; bufferSize <= maxBufferSize; bufferSize++)
                {
                    Console.WriteLine($"BufferSize:{bufferSize}");
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

                Console.WriteLine("Calculation finished");
                File.WriteAllText(resultFileName,
                    JsonSerializer.Serialize(finalResults
                        .OrderBy(r => r.BufferSize)
                        .ThenBy(r => r.AlgorithmName)
                        .ThenBy(r => r.HashLength)
                        .ToArray()
                    ));
                foreach (var result in finalResults)
                {
                    Console.WriteLine($"{result}");
                }
            },
            minBufferSizeArg, maxBufferSizeArg, minHashLengthArg, maxHashLengthArg, resultFileNameArg, rootFolderArg);

        var rootCommand = new RootCommand("Hash collision research") { uniqueCommand, hashSizeCommand };

        rootCommand.Invoke(args);
    }
}