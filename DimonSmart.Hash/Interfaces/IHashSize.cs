namespace DimonSmart.Hash.Interfaces;

public interface IHashSize
{
    /// <summary>Gets the size, in bits, of the computed hash code.</summary>
    /// <returns>The size, in bits, of the computed hash code.</returns>
    public int HashSize { get; }
}