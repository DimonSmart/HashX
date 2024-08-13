using System.Security.Cryptography;
using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

public class Sha1HashAlgorithm : IHashAlgorithm
{
    public int HashSize => SHA1.HashSizeInBytes;
    public string Name => "SHA1";

    public byte[] ComputeHash(byte[] buffer)
    {
        return SHA1.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return SHA1.HashData(buffer.AsSpan(offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        return SHA1.HashData(buffer);
    }
}