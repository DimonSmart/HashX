namespace DimonSmart.Hash.Interfaces;

public interface IIncrementalHashAlgorithm : IHashSize
{
    public void AddByte(byte incomingByte);
    public byte[] GetBytes();
}