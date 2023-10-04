namespace DimonSmart.AnyHash;

public static class AnyHashFactory
{
    public static IHashAlgorithm CreateHashAlgorithm(int hashLength)
    {
        // TODO: In case of performance implementations
        // based on uint, ulong we can provide
        // different implementations
        return new XorHash(hashLength);
    }
}