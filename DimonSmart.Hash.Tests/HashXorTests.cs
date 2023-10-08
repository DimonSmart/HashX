using Xunit;

namespace DimonSmart.Hash.Tests;

public class XorHashTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0x80, 0x80)]
    [InlineData(0xFF, 0xFF)]
    public void HashXorTests(byte expectedValue, byte addedValue)
    {
        // Arrange
        var h = new XorHash(1);

        // Act
        h.AddByte(addedValue);

        // Assert
        Assert.Equal(new[] { expectedValue }, h.GetBytes());
    }

    [Theory]
    [InlineData(0x00, 0x00, 0x00)]
    [InlineData(0xFF, 0x00, 0xFF)]
    [InlineData(0xFF, 0xFF, 0x00)]
    [InlineData(0x80, 0x80, 0x00)]
    public void XorHashShiftTest(byte expectedValue, byte initial, byte addedValue)
    {
        // Arrange
        var h = new XorHash(1);
        h.AddByte(initial);

        // Act
        h.AddByte(addedValue);

        // Assert
        Assert.Equal(new[] { expectedValue }, h.GetBytes());
    }

    [Fact]
    public void AllBitsSetTest()
    {
        var h = new XorHash(1);

        for (var i = 0; i < 8; i++)
        {
            h.AddByte(1);
            var x = h.GetBytes().First();
        }

        Assert.Equal(new byte[] { 0xFF }, h.GetBytes());
    }

    private byte[] GetBytes(string input)
    {
        List<byte> intList = new();
        foreach (var s in input.Split(','))
        {
            intList.Add(byte.Parse(s));
        }

        return intList.ToArray();
    }
}