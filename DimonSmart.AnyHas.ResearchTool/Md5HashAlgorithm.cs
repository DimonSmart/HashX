using System.Security.Cryptography;
using DimonSmart.AnyHash;

namespace DimonSmart.AnyHas.ResearchTool;

public class Md5HashAlgorithm : IHashAlgorithm
{
    public int HashSize => MD5.HashSizeInBytes;
    public byte[] ComputeHash(byte[] buffer)
    {
        return MD5.HashData(buffer);
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        return MD5.HashData(buffer.AsSpan(offset, count));
    }
}