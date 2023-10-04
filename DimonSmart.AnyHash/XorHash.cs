namespace DimonSmart.AnyHash;

public class XorHash : IHashAlgorithm, IIncrementalHashAlgorithm
{
    protected byte[] Bytes;
    protected int LengthInBits;
    protected int CurrentXorPositionForAdd;

    public XorHash(int hashSize)
    {
        HashSize = hashSize;
        Bytes = new byte[HashSize];
        LengthInBits = 8 * HashSize;
    }

    public int HashSize { get; private set; }

    public byte[] ComputeHash(byte[] buffer)
    {
        Reset();
        return ComputeHashCore(buffer, 0, buffer.Length);
    }

    public byte[] ComputeHashCore(byte[] buffer, int offset, int count)
    {
        for (var index = offset; index < offset + count; index++)
        {
            AddByte(buffer[index]);
        }

        return (byte[])Bytes.Clone();
    }
    public byte[] ComputeHash(byte[] buffer, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        if (offset < 0)
            throw new ArgumentOutOfRangeException(nameof(offset));
        if (count < 0 || count > buffer.Length)
            throw new ArgumentException();
        if (buffer.Length - count < offset)
            throw new ArgumentException();
        return ComputeHashCore(buffer, offset, count);
    }

    private void Reset()
    {
        CurrentXorPositionForAdd = 0;
        Bytes.Initialize();
    }

    public void AddByte(byte incomingByte)
    {
        CurrentXorPositionForAdd = XorByte(incomingByte, CurrentXorPositionForAdd);
    }

    public byte[] GetBytes()
    {
        return Bytes;
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