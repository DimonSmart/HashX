namespace DimonSmart.Hash;

public class SlidingHash(int hashLength, int slidingBufferSize)
{
    private readonly HashRoller _hashRoller = new(hashLength);
    private readonly Queue<byte> _slidingBuffer = new Queue<byte>(slidingBufferSize);

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

    internal class HashRoller(int hashLength) : XorHash(hashLength)
    {
        private int _currentXorPositionForSubtract;

        public void SubstractByte(byte incomingByte)
        {
            _currentXorPositionForSubtract = XorByte(incomingByte, _currentXorPositionForSubtract);
        }
    }
}