namespace DimonSmart.AnyHash;

public interface IHashAlgorithm : IHashSize
{
    public string Name { get; }
    public byte[] ComputeHash(byte[] buffer);
    public byte[] ComputeHash(byte[] buffer, int offset, int count);
}