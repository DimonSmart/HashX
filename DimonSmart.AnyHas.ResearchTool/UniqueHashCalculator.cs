using DimonSmart.AnyHash;

namespace DimonSmart.AnyHas.ResearchTool;

public static class UniqueHashCalculator
{
    public static IEnumerable<UniqueCalculationResult> Calculate(IEnumerable<string> files, OneRunParameters parameters)
    {
        var xorHashAlgorithm = AnyHashFactory.CreateHashAlgorithm(parameters.HashLength);
        var md5HashAlgorithm = new Md5HashAlgorithm();

        Dictionary<byte[], int> xorUniqueHashCounter = new(10000, new ByteArrayEqualityComparer());
        Dictionary<byte[], int> md5UniqueHashCounter = new(10000, new ByteArrayEqualityComparer());

        UniqueCalculationResult xorResult = new("XOR",0, 0);
        UniqueCalculationResult md5Result = new("MD5", 0, 0);
        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            for (var i = 0; i <= bytes.Length - parameters.BufferSize; i++)
            {
                CalculateCollisions(parameters.BufferSize, xorHashAlgorithm, bytes, i, xorUniqueHashCounter);
                CalculateCollisions(parameters.BufferSize, md5HashAlgorithm, bytes, i, md5UniqueHashCounter);
            }

            xorResult = xorResult with
            {
                BlocksHashed = xorResult.BlocksHashed + bytes.Length - parameters.BufferSize,
                UniqueHashes = xorResult.UniqueHashes + xorUniqueHashCounter.Keys.Count
            };

            md5Result = md5Result with
            {
                BlocksHashed = md5Result.BlocksHashed + bytes.Length - parameters.BufferSize,
                UniqueHashes = md5Result.UniqueHashes + md5UniqueHashCounter.Keys.Count
            };
        }

        return new []{xorResult, md5Result};
    }

    private static void CalculateCollisions(int bufferSize, IHashAlgorithm xorHashAlgorithm, byte[] bytes, int i,
        Dictionary<byte[], int> uniqueHashCounter)
    {
        var xorHash = xorHashAlgorithm.ComputeHash(bytes, i, bufferSize);
        if (uniqueHashCounter.TryGetValue(xorHash, out var counter))
        {
            uniqueHashCounter[xorHash] = counter + 1;
        }
        else
        {
            uniqueHashCounter[xorHash] = 1;
        }
    }
}