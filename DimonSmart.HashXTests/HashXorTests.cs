using DimonSmart.HashX;
using Xunit;

namespace DimonSmart.HashXTests;

public class HashXorTests
{
    [Fact]
    public void TestHashXorOneByte()
    {
        var h = new HashXor(1);

        // Add bytes 1, 2, 4, 8, 16, etc.
        byte expected = 0;
        for (int i = 0; i < 8; i++)
        {
            h.AddByte((byte)(1 << i));
            expected ^= (byte)(1 << i);
            Assert.Equal(expected, h.GetBytes()[0]);
        }

        // Verify wrapping to beginning of byte array
        h.AddByte(1);
        var expected2 = 1;
        Assert.Equal(expected2, h.GetBytes()[0]);

        // Verify wrapping across multiple bytes
        h = new HashXor(2);
        h.AddByte(1);
        h.AddByte(2);
        h.AddByte(4);
        var expected3 = new byte[] { 5, 0 }; // 0101 XOR 0010 XOR 0100 = 5
        Assert.Equal(expected3, h.GetBytes());
    }

    [Fact]
    public void TestHashXorTwoBytes()
    {
        var h = new HashXor(2);

        // Add bytes 1, 2, 3, 4, etc.
        for (int i = 0; i < 16; i += 2)
        {
            h.AddByte((byte)i);
            h.AddByte((byte)(i + 1));
            var expected = new byte[] { (byte)i, (byte)(i + 1) };
            Assert.Equal(expected, h.GetBytes());
        }

        // Verify wrapping to beginning of byte array
        h.AddByte(1);
        var expected2 = new byte[] { 1, 0 };
        Assert.Equal(expected2, h.GetBytes());
    }

    [Fact]
    public void TestHashXorOverflow()
    {
        var h = new HashXor(1);

        // Add 256 bytes
        for (int i = 0; i < 256; i++)
        {
            h.AddByte(1);
            var expected = (byte)(i % 2 == 0 ? 0 : 1);
            Assert.Equal(expected, h.GetBytes()[0]);
        }
    }
}
