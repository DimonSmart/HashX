using System.CommandLine;

namespace DimonSmart.AnyHas.ResearchTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hashedBlockLengthArg = new Option<int>(
                name: "--HashedBlockLength",
                getDefaultValue: () => 1,
                description: "Hashed block length, bytes.");
            var hashSizeArg = new Option<int>(
                name: "--HashSize",
                getDefaultValue: () => 2,
                description: "Hash size, bytes.");
            var rootFolderArg = new Argument<string>(
                name: "--RootFolder",
                description: "Root folder for hash collision statistic calculation.");

            var rootCommand = new RootCommand("Hash collision research");
            rootCommand.AddOption(hashedBlockLengthArg);
            rootCommand.AddOption(hashSizeArg);
            rootCommand.AddArgument(rootFolderArg);

            rootCommand.SetHandler(
                (hashedBlockLength, hashLength, rootFolder) =>
                {
                    Console.WriteLine($"HashedBlockLength:{hashedBlockLength}");
                    Console.WriteLine($"HashLength:{hashLength}");

                    var files = Directory.EnumerateFiles(rootFolder);
                    var results = UniqueHashCalculator.Calculate(files,
                        new OneRunParameters(hashedBlockLength, hashLength));

                    foreach (var result in results)
                    {
                        Console.WriteLine($"{result}");
                    }
                },
                hashedBlockLengthArg, hashSizeArg, rootFolderArg);
            rootCommand.Invoke(args);
        }
    }
}