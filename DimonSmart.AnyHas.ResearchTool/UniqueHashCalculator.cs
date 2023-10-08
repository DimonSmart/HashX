using DimonSmart.AnyHash;

namespace DimonSmart.AnyHas.ResearchTool;

public static class UniqueHashCalculator
{
    public static UniqueCalculationResult Calculate(IHashAlgorithm hashAlgorithm, IEnumerable<string> files,
        int bufferSize)
    {
        Dictionary<byte[], int> uniqueHashCounter = new(10000, new ByteArrayEqualityComparer());
        UniqueCalculationResult result = new(hashAlgorithm.Name, 0, 0, bufferSize, hashAlgorithm.HashSize);

        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file);
            for (var i = 0; i <= bytes.Length - bufferSize; i++)
                CalculateCollisions(bufferSize, hashAlgorithm, bytes, i, uniqueHashCounter);

            result = result with
            {
                BlocksHashed = result.BlocksHashed + bytes.Length - result.BufferSize,
                UniqueHashes = result.UniqueHashes + uniqueHashCounter.Keys.Count
            };
        }

        return result;
    }

    private static void CalculateCollisions(int bufferSize, IHashAlgorithm hashAlgorithm, byte[] bytes, int i,
        IDictionary<byte[], int> uniqueHashCounter)
    {
        var xorHash = hashAlgorithm.ComputeHash(bytes, i, bufferSize);
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