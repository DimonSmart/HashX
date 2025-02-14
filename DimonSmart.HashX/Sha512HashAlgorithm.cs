using DimonSmart.Hash.Interfaces;
using System.Security.Cryptography;

namespace DimonSmart.Hash;

public class Sha512HashAlgorithm : IHashAlgorithm
{
    public int HashSize => SHA512.HashSizeInBytes;
    public string Name => "SHA512";

    public byte[] ComputeHash(byte[] buffer)
    {
        return SHA512.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return SHA512.HashData(buffer.AsSpan(offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        return SHA512.HashData(buffer);
    }
}
