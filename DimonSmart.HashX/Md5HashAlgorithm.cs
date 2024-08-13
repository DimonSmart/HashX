using System.Security.Cryptography;
using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

public class Md5HashAlgorithm : IHashAlgorithm
{
    public int HashSize => MD5.HashSizeInBytes;
    public string Name => "MD5";

    public byte[] ComputeHash(byte[] buffer)
    {
        return MD5.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return MD5.HashData(buffer.AsSpan(offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        return MD5.HashData(buffer);
    }
}