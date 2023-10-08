namespace DimonSmart.Hash;

public class SlidingHash
{
    private readonly HashRoller _hashRoller;
    private readonly Queue<byte> _slidingBuffer;

    public SlidingHash(int hashLength, int slidingBufferSize)
    {
        _hashRoller = new HashRoller(hashLength);
        _slidingBuffer = new Queue<byte>(slidingBufferSize);
    }

    public void Add(byte incomingByte)
    {
        _hashRoller.AddByte(incomingByte);
        _slidingBuffer.Enqueue(incomingByte);

        if (_slidingBuffer.Count > _hashRoller.GetBytes().Length)
        {
            var outgoingByte = _slidingBuffer.Dequeue();
            _hashRoller.SubstractByte(outgoingByte);
        }
    }

    public byte[] GetHash()
    {
        return _hashRoller.GetBytes();
    }

    internal class HashRoller : XorHash
    {
        private int currentXorPositionForSubtract;

        public HashRoller(int hashLength) : base(hashLength)
        {
        }

        public void SubstractByte(byte incomingByte)
        {
            currentXorPositionForSubtract = XorByte(incomingByte, currentXorPositionForSubtract);
        }
    }
}