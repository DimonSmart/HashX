namespace DimonSmart.HashX;

public static class HashXorFactory
{
    public static IHash CreateHash(int hashLength)
    {
        // TODO: In case of performant implementations
        // based on uint, ulong we can provide
        // different implementations
        return new HashXor(hashLength);
    }
}

