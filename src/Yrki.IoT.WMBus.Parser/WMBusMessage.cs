namespace Yrki.IoT.WMBus.Parser;

public class WMBusMessage
{
    public int LField { get; set; }
    public int CField { get; set; }
    public string MField { get; set; }
    public string AField { get; set; }
    public byte Version { get; set; }
    public DeviceType DeviceType { get; set; }
    public byte CIField { get; set; }
    public byte Sequence { get; set; }
    public byte Status { get; set; }
    public EncryptionMethod EncryptionMethod { get; set; }
    public byte[] RawPayload { get; internal set; }
}    