namespace DimonSmart.AnyHas.ResearchTool;

public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
{
    public bool Equals(byte[]? x, byte[]? y)
    {
        if (x == null || y == null)
            return false;

        if (x.Length != y.Length)
            return false;

        return !x.Where((t, i) => t != y[i]).Any();
    }

    public int GetHashCode(byte[] obj)
    {
        unchecked
        {
            int hash = 17;
            foreach (byte b in obj)
            {
                hash = hash * 31 + b.GetHashCode();
            }
            return hash;
        }
    }
}