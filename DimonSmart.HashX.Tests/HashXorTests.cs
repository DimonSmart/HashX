using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace DimonSmart.Hash.Tests;

public class XorHashTests
{
    private readonly ITestOutputHelper _output;

    public XorHashTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(0x80, 0x80)]
    [InlineData(0xFF, 0xFF)]
    public void HashOneByteTests(byte expectedValue, byte addedValue)
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
    public void HashWithShiftTest(byte expectedValue, byte initial, byte addedValue)
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
            var b = h.GetBytes().First();
            _output.WriteLine($"HashValue: {b:X}");
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