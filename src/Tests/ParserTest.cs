using Microsoft.Extensions.Configuration;
using Yrki.IoT.WMBus.Parser;

namespace Tests;

[TestClass]
public class ParserTest
{
    private readonly IConfigurationRoot _configuration;

    public ParserTest()
    {
        var builder = new ConfigurationBuilder()
                        .AddUserSecrets<ParserTest>()
                        .AddUserSecrets<ParserTest>();

        _configuration = builder.Build();
    }

    [TestMethod]
    public void Parse_header()
    {
        // Arrange
        var wmBusMessage = "3F4409072579120008167A0C00300557685205A5F41FC0DB828F70965D97D8F4E19FE9E8E7DAA267F9A096E5F6D98769CD1912F67172AAECDDEA8EF23DCEC40EC1";
        var parser = new Parser();

        // Act
        var result = parser.ParseHeader(wmBusMessage);

        // Assert
        Assert.AreEqual(63, result.LField);
        Assert.AreEqual(0x44, result.CField);
        Assert.AreEqual("AXI", result.MField);
        Assert.AreEqual("00127925", result.AField);
        Assert.AreEqual(0x08, result.Version);
        Assert.AreEqual(DeviceType.ColdWater, result.DeviceType);
        Assert.AreEqual(0x7A, result.CIField);
        Assert.AreEqual(EncryptionMethod.Aes128, result.EncryptionMethod);
    }


    [TestMethod]
    [Ignore("Needs to have a user-secret with the correct key")]
    public void Parse_an_encrypted_message()
    {
        // Arrange
        var wmBusMessage = "3E4409072579120008167A0C00300557685205A5F41FC0DB828F70965D97D8F4E19FE9E8E7DAA267F9A096E5F6D98769CD1912F67172AAECDDEA8EF23DCEC4";
        var key = _configuration.GetSection("ENCRYPTIONKEY").Value;
        var parser = new Parser();

        // Act
        var payload = parser.ParsePayload(wmBusMessage, key);

        // Assert
        
    }
}