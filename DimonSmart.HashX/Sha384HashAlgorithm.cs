using DimonSmart.Hash.Interfaces;
using System.Security.Cryptography;

namespace DimonSmart.Hash;

public class Sha384HashAlgorithm : IHashAlgorithm
{
    public int HashSize => SHA384.HashSizeInBytes;
    public string Name => "SHA384";

    public byte[] ComputeHash(byte[] buffer)
    {
        return SHA384.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return SHA384.HashData(buffer.AsSpan(offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        return SHA384.HashData(buffer);
    }
}
