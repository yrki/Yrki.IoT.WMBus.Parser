using Yrki.IoT.WMBus.Parser;

namespace Tests;

[TestClass]
public class ParserTest
{
    [TestMethod]
    public void ParseHeader_Success()
    {
        // Arrange
        var wmBusMessage = "3F4409072579120008167A0C00300557685205A5F41FC0DB828F70965D97D8F4E19FE9E8E7DAA267F9A096E5F6D98769CD1912F67172AAECDDEA8EF23DCEC40EC1";
        var parser = new Parser();

        // Act
        var result = parser.Parse(wmBusMessage);

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


    // [TestMethod]
    // public void ParseALotOfFiles()
    // {
    //     var parser = new Parser();

    //     var meters = new List<string>();


    //     var lines = File.ReadAllLines("../../../../../Example messages/Messages.txt");
    //     foreach (var line in lines)
    //     {
    //         var message = line.Substring(4, line.Length - 4);
    //         var result = parser.Parse(message);

    //         // File.AppendAllLines($"../../../../../Example messages/{result.AField}.txt", new string[] { line });
            
    //     }
    // }
}