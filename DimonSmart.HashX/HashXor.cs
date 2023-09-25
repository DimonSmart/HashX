namespace DimonSmart.HashX;

public class HashXor
{
    private byte[] _bytes;
    private int currentXorPositionForAdd;
    private int currentXorPositionForSubtract;

    public HashXor(int hashLength)
    {
        _bytes = new byte[hashLength];
        currentXorPositionForAdd = 0;
        currentXorPositionForSubtract = 0;
    }

    public void AddByte(byte incomingByte)
    {
        int startPos = currentXorPositionForAdd / 8;
        int endPos = (currentXorPositionForAdd + 8) / 8;

        // XOR the incoming byte with the current position in the byte array
        _bytes[startPos % _bytes.Length] ^= (byte)(incomingByte << (currentXorPositionForAdd % 8));
        if (endPos != startPos)
        {
            _bytes[endPos % _bytes.Length] ^= (byte)(incomingByte >> (8 - (currentXorPositionForAdd % 8)));
        }

        // Increment the current position
        currentXorPositionForAdd = (currentXorPositionForAdd + (8 * _bytes.Length)) % (8 * _bytes.Length);

        if (currentXorPositionForAdd - currentXorPositionForSubtract > 8 * _bytes.Length)
        {
            SubtractByte();
        }
    }

    public void SubtractByte()
    {
        byte removedByte = _bytes[currentXorPositionForSubtract / 8];
        _bytes[currentXorPositionForSubtract / 8] ^= (byte)(removedByte << (currentXorPositionForSubtract % 8)); // Zero out the removed byte
        currentXorPositionForSubtract = (currentXorPositionForSubtract + 8) % (8 * _bytes.Length);
    }

    public byte[] GetBytes()
    {
        return _bytes;
    }
}

