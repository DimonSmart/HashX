namespace DimonSmart.HashX;

public interface IHash
{
    public void AddByte(byte incomingByte);
    public byte[] GetBytes();
}

