using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

/// <summary>
/// Performance base-line algorithm only!
/// This algorithm simply treat cut first HashSize bytes from buffer as hash
/// Do not use it as a real hash function.
/// This HashAlgorithm is for educational purposes only.
/// </summary>
public class FirstBytesHashAlgorithm : IHashAlgorithm
{
    public FirstBytesHashAlgorithm(int hashSize)
    {
        HashSize = hashSize;
    }

    public int HashSize { get; }
    public string Name => "CutHash";

    public byte[] ComputeHash(byte[] buffer)
    {
        return buffer
            .AsSpan(0, buffer.Length > HashSize ? HashSize : buffer.Length)
            .ToArray();
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int length)
    {
        return buffer
            .AsSpan(offset, buffer.Length > (HashSize + offset) ? HashSize : buffer.Length)
            .ToArray();
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length != HashSize)
        {
            throw new InvalidOperationException("ZeroHash should be used only with blockSize == HashSize");
        }

        return buffer.ToArray();
    }
}