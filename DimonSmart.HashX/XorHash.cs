using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

public class XorHash : IHashAlgorithm, IIncrementalHashAlgorithm
{
    protected byte[] Bytes;
    protected int CurrentXorPositionForAdd;
    protected int LengthInBits;

    public XorHash(int hashSize)
    {
        HashSize = hashSize;
        Bytes = new byte[HashSize];
        LengthInBits = 8 * HashSize;
    }

    public int HashSize { get; }

    public string Name => "XorHash";

    public byte[] ComputeHash(byte[] buffer)
    {
        Reset();
        return ComputeHash(new ReadOnlySpan<byte>(buffer));
    }

    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        Reset();

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        if (count < 0 || count > buffer.Length)
        {
            throw new ArgumentException();
        }

        if (buffer.Length - count < offset)
        {
            throw new IndexOutOfRangeException();
        }

        return ComputeHash(new ReadOnlySpan<byte>(buffer, offset, count));
    }

    public byte[] ComputeHash(ReadOnlySpan<byte> buffer)
    {
        Reset();
        foreach (var byteValue in buffer)
        {
            AddByte(byteValue);
        }

        return (byte[])Bytes.Clone();
    }

    public void AddByte(byte incomingByte)
    {
        CurrentXorPositionForAdd = XorByte(incomingByte, CurrentXorPositionForAdd);
    }

    public byte[] GetBytes()
    {
        return Bytes;
    }

    private void Reset()
    {
        CurrentXorPositionForAdd = 0;
        Bytes.Initialize();
    }

    protected int XorByte(byte incomingByte, int position)
    {
        var firstBytePos = position / 8 % Bytes.Length;
        var secondBytePos = (position + 8) / 8 % Bytes.Length;

        var shift = position % 8;
        var incomingPartForFirstByte = (byte)(incomingByte << shift);
        Bytes[firstBytePos] ^= incomingPartForFirstByte;

        if (shift != 0)
        {
            var incomingPartForSecondByte = (byte)(incomingByte >> (8 - shift));
            Bytes[secondBytePos] ^= incomingPartForSecondByte;
        }

        position = (position + 1) % LengthInBits;
        return position;
    }
}