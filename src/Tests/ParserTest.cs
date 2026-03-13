using System;
using Microsoft.Extensions.Configuration;
using Yrki.IoT.WMBus.Parser;
using Yrki.IoT.WMBus.Parser.Extensions;

namespace Tests;

public class ParserTest
{
    private const string AxiomaHeaderMessage = "3F4409072579120008167A0C00300557685205A5F41FC0DB828F70965D97D8F4E19FE9E8E7DAA267F9A096E5F6D98769CD1912F67172AAECDDEA8EF23DCEC40EC1";
    private const string LansenPayloadMessage = """
        6A
        44
        33 30
        67 00 01 00
        0F
        2A
        7A
        07
        00
        00 00
        2F 2F
        02 65 22 11
        42 65 65 43
        82 01 65 22 11
        02 FB 1A 22 11
        42 FB 1A 22 11
        82 01 FB 1A 22 11
        02 FD 3A 22 11
        42 FD 3A 33 22
        82 01 FD 3A 02 01
        C2 01 FD 3A 24 23
        82 40 FD 3A 02 00
        82 80 40 FD 3A 28 00
        C2 80 40 FD 3A 2B 00
        82 02 23 00 00
        02 27 00 00
        02 FD 0F 04 00
        01 FD 1B 00
        """;

    private readonly IConfigurationRoot _configuration;

    public ParserTest()
    {
        var builder = new ConfigurationBuilder()
                        .AddUserSecrets<ParserTest>()
                        .AddUserSecrets<ParserTest>();

        _configuration = builder.Build();
    }

    [Fact]
    public void Shall_parse_a_header_from_a_valid_message()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var result = parser.ParseHeader(AxiomaHeaderMessage);

        // Assert
        Assert.Equal(63, result.LField);
        Assert.Equal(0x44, result.CField);
        Assert.Equal("AXI", result.MField);
        Assert.Equal("00127925", result.AField);
        Assert.Equal((byte)0x08, result.Version);
        Assert.Equal(DeviceType.ColdWater, result.DeviceType);
        Assert.Equal((byte)0x7A, result.CIField);
        Assert.Equal(EncryptionMethod.Aes128, result.EncryptionMethod);
    }

    [Fact]
    public void Shall_parse_a_header_when_the_message_contains_mixed_whitespace()
    {
        // Arrange
        var wmBusMessage = "3F 44\t09 07 25 79 12 00\r\n08 16 7A 0C 00 30 05 57";
        var parser = new Parser();

        // Act
        var result = parser.ParseHeader(wmBusMessage);

        // Assert
        Assert.Equal(63, result.LField);
        Assert.Equal(0x44, result.CField);
        Assert.Equal("AXI", result.MField);
        Assert.Equal("00127925", result.AField);
        Assert.Equal(DeviceType.ColdWater, result.DeviceType);
        Assert.Equal(EncryptionMethod.Aes128, result.EncryptionMethod);
    }

    [Fact]
    public void Shall_reject_a_header_message_with_an_odd_number_of_hex_digits()
    {
        // Arrange
        var parser = new Parser();

        // Act
        var action = () => parser.ParseHeader("ABC");

        // Assert
        var exception = Assert.Throws<FormatException>(action);
        Assert.Equal("Hex message must contain an even number of digits.", exception.Message);
    }

    [Fact]
    public void Shall_return_the_same_header_from_string_and_byte_array_overloads()
    {
        // Arrange
        var parser = new Parser();
        var messageBytes = AxiomaHeaderMessage.ToByteArray();

        // Act
        var fromString = parser.ParseHeader(AxiomaHeaderMessage);
        var fromBytes = parser.ParseHeader(messageBytes);

        // Assert
        Assert.Equal(fromString.LField, fromBytes.LField);
        Assert.Equal(fromString.CField, fromBytes.CField);
        Assert.Equal(fromString.MField, fromBytes.MField);
        Assert.Equal(fromString.AField, fromBytes.AField);
        Assert.Equal(fromString.Version, fromBytes.Version);
        Assert.Equal(fromString.DeviceType, fromBytes.DeviceType);
        Assert.Equal(fromString.CIField, fromBytes.CIField);
        Assert.Equal(fromString.Sequence, fromBytes.Sequence);
        Assert.Equal(fromString.Status, fromBytes.Status);
        Assert.Equal(fromString.EncryptionMethod, fromBytes.EncryptionMethod);
    }

    [Fact]
    public void Shall_throw_when_parsing_a_payload_for_an_unknown_manufacturer()
    {
        // Arrange
        var parser = new Parser();
        var unknownManufacturerMessage = LansenPayloadMessage.Replace("33 30", "00 00");

        // Act
        var exception = Assert.Throws<Exception>(() => parser.ParsePayload(unknownManufacturerMessage, string.Empty));

        // Assert
        Assert.Equal("Could not find parser for this manufacturer", exception.Message);
    }


    [Fact(Skip = "Needs to have a user-secret with the correct key")]
    public void Shall_parse_an_encrypted_axioma_message()
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
