using System;

namespace Yrki.IoT.WMBus.Parser.Manufacturers.Axioma;

public class AxiomaParser : IPayloadParser
{
    IParsedPayload IPayloadParser.ParsePayload(byte meterType, byte protocolVersion, byte[] message)
    {
        throw new NotImplementedException();
    }
}
