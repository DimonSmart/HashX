namespace DimonSmart.HashX;


public class SlidingHash
{
    private HashXor _hashXor;
    private Queue<byte> _slidingBuffer;

    public SlidingHash(int hashLength, int slidingBufferSize)
    {
        _hashXor = new HashXor(hashLength);
        _slidingBuffer = new Queue<byte>(slidingBufferSize);
    }

    public void Add(byte incomingByte)
    {
        _hashXor.AddByte(incomingByte);
        _slidingBuffer.Enqueue(incomingByte);

        if (_slidingBuffer.Count > _hashXor.GetBytes().Length)
        {
            _hashXor.SubtractByte();
            _slidingBuffer.Dequeue();
        }
    }

    public byte[] GetHash()
    {
        return _hashXor.GetBytes();
    }
}

