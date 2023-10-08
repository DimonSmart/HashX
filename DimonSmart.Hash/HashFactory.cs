using DimonSmart.Hash.Interfaces;

namespace DimonSmart.Hash;

public static class HashFactory
{
    public static IHashAlgorithm CreateHashAlgorithm(int hashLength)
    {
        // TODO: In case of performance implementations
        // based on uint, ulong we can provide
        // different implementations
        return new XorHash(hashLength);
    }
}