namespace Yrki.IoT.WMBus.Parser;

internal interface IPayloadParser
{
    internal IParsedPayload ParsePayload(byte meterType, byte protocolVersion, byte[] message);
}
