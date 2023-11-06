using DimonSmart.TinyBenchmark.Attributes;

namespace DimonSmart.Hash.Tests;

public class VariableHashLengthBenchmarkTests
{
    private static readonly byte[] Bytes = GenerateRandomByteArray(1024);

    [TinyBenchmarkRangeParameter(1, 60, 5)]
    public int N { get; set; }

    [TinyBenchmark]
    public void XorHashTest(int n)
    {
        var hashAlgorithm = new XorHash(n);
        _ = hashAlgorithm.ComputeHash(Bytes);
    }

    [TinyBenchmark]
    public void MD5HashTest(int n)
    {
        var hashAlgorithm = new Md5HashAlgorithm();
        var result = hashAlgorithm.ComputeHash(Bytes);
    }

    [TinyBenchmark]
    public void SHA1HashTest(int n)
    {
        var hashAlgorithm = new Sha1HashAlgorithm();
        var result = hashAlgorithm.ComputeHash(Bytes);
    }

    public void FirstBytesHashTest()
    {
        var hashAlgorithm = new FirstBytesHashAlgorithm(N);
        var result = hashAlgorithm.ComputeHash(Bytes);
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