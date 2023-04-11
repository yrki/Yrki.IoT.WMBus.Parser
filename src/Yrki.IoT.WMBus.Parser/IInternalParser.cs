namespace Yrki.IoT.WMBus.Parser
{
    internal interface IInternalParser
    {
        internal IParsedMessage Parse(byte meterType, byte protocolVersion, byte[] message);
    }
}
