using DimonSmart.Hash.Interfaces;
using System.Security.Cryptography;

namespace DimonSmart.Hash;

public class Sha256HashAlgorithm : IHashAlgorithm
{
    public int HashSize => SHA256.HashSizeInBytes;
    public string Name => "SHA256";

    public byte[] ComputeHash(byte[] buffer)
    {
        return SHA256.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return SHA256.HashData(buffer.AsSpan(offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        return SHA256.HashData(buffer);
    }
}
