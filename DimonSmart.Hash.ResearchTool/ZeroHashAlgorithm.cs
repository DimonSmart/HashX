using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash.ResearchTool;

public class ZeroHashAlgorithm : IHashAlgorithm
{
    private const string BlockSIzeMismatchErrorMessage = "ZeroHash should be used only with blockSize == HashSize";

    public ZeroHashAlgorithm(int hashSize)
    {
        HashSize = hashSize;
    }

    public int HashSize { get; }
    public string Name => "Zero";

    public byte[] ComputeHash(byte[] buffer)
    {
        if (buffer.Length != HashSize)
        {
            throw new InvalidOperationException(BlockSIzeMismatchErrorMessage);
        }

        return buffer;
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int length)
    {
        if (length != HashSize)
        {
            throw new InvalidOperationException("ZeroHash should be used only with blockSize == HashSize");
        }

        return buffer.AsSpan(offset, length).ToArray();
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