namespace DimonSmart.Hash.Interfaces;

/// <summary>
/// Defines the size, in bits, of the computed hash code.
/// </summary>
public interface IHashSize
{
    /// <summary>
    /// Gets the size, in bits, of the computed hash code.
    /// </summary>
    int HashSize { get; }
}
