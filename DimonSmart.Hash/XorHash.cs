using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

public class XorHash : IHashAlgorithm, IIncrementalHashAlgorithm
{
    private readonly byte[] _bytes;
    private readonly int _lengthInBits;
    private int _currentXorPositionForAdd;

    public XorHash(int hashSize)
    {
        HashSize = hashSize;
        _bytes = new byte[HashSize];
        _lengthInBits = 8 * HashSize;
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

        return (byte[])_bytes.Clone();
    }

    public void AddByte(byte incomingByte)
    {
        _currentXorPositionForAdd = XorByte(incomingByte, _currentXorPositionForAdd);
    }

    public byte[] GetBytes()
    {
        return _bytes;
    }

    private void Reset()
    {
        _currentXorPositionForAdd = 0;
        _bytes.Initialize();
    }

    protected int XorByte(byte incomingByte, int position)
    {
        var firstBytePos = position / 8 % _bytes.Length;
        var secondBytePos = (position + 8) / 8 % _bytes.Length;

        var shift = position % 8;
        var incomingPartForFirstByte = (byte)(incomingByte << shift);
        _bytes[firstBytePos] ^= incomingPartForFirstByte;

        if (shift != 0)
        {
            var incomingPartForSecondByte = (byte)(incomingByte >> (8 - shift));
            _bytes[secondBytePos] ^= incomingPartForSecondByte;
        }

        position = (position + 1) % _lengthInBits;
        return position;
    }
}