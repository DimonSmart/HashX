namespace DimonSmart.AnyHash;

public interface IIncrementalHashAlgorithm : IHashSize
{
    public void AddByte(byte incomingByte);
    public byte[] GetBytes();
}