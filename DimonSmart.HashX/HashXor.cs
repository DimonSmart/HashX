namespace DimonSmart.HashX;

public class HashXor : IHash
{
    protected byte[] _bytes;
    protected int currentXorPositionForAdd = 0;
    protected int _lengthInBits;
    public HashXor(int hashLength)
    {
        _bytes = new byte[hashLength];
        _lengthInBits = 8 * _bytes.Length;
    }

    public void AddByte(byte incomingByte)
    {
        currentXorPositionForAdd = XorByte(incomingByte, currentXorPositionForAdd);
    }

    protected int XorByte(byte incomingByte, int position)
    {
        var firstBytePos = (position / 8) % _bytes.Length;
        var secondBytePos = ((position + 8) / 8) % _bytes.Length;

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

    public byte[] GetBytes() => _bytes;
}
