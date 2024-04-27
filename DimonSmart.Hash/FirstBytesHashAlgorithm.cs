using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

/// <summary>
/// A performance baseline algorithm demonstration.
/// This algorithm simply cuts and returns the first 'HashSize' bytes from the buffer as the hash.
/// Do not use it as a real hash function.
/// This HashAlgorithm is intended for educational purposes only.
/// </summary>
public class FirstBytesHashAlgorithm(int hashSize) : IHashAlgorithm
{
    public int HashSize { get; } = hashSize;
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
            .AsSpan(offset, buffer.Length > HashSize + offset ? HashSize : buffer.Length)
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