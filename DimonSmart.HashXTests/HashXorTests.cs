using DimonSmart.HashX;
using Xunit;

namespace DimonSmart.HashXTests;

public class HashXorTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0x80, 0x80)]
    [InlineData(0xFF, 0xFF)]
    public void TestHashXor(byte expectedValue, byte addedValue)
    {
        // Arrange
        var h = new HashXor(1);

        // Act
        h.AddByte(addedValue);

        // Assert
        Assert.Equal(new byte[] { expectedValue }, h.GetBytes());
    }

    [Theory]
    [InlineData(0x00, 0x00, 0x00)]
    [InlineData(0xFF, 0x00, 0xFF)]
    [InlineData(0xFF, 0xFF, 0x00)]
    [InlineData(0x80, 0x80, 0x00)]
    public void TestHashXorShiftTest(byte expectedValue, byte initial, byte addedValue)
    {
        // Arrange
        var h = new HashXor(1);
        h.AddByte(initial);

        // Act
        h.AddByte(addedValue);

        // Assert
        Assert.Equal(new byte[] { expectedValue }, h.GetBytes());
    }

    [Fact]
    public void TestHashXorFullFilled()
    {
        var h = new HashXor(1);

        for (int i = 0; i < 8; i++)
        {
            h.AddByte(1);
            var x = h.GetBytes().First();

        }
        Assert.Equal(new byte[] { 0xFF }, h.GetBytes());
    }

    private byte[] GetBytes(string input)
    {
        List<byte> intList = new();
        foreach (string s in input.Split(','))
        {
            intList.Add(byte.Parse(s));
        }
        return intList.ToArray();
    }
}
