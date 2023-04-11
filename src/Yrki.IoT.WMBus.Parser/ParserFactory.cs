using System;
using Yrki.IoT.WMBus.Parser.Manufacturers.Axioma;
using Yrki.IoT.WMBus.Parser.Manufacturers.Lansen;

namespace Yrki.IoT.WMBus.Parser;

internal static class ParserFactory
{
    internal static IPayloadParser GetParser(string manufacturerId)
    {
        switch (manufacturerId)
        {
            case "AXI": 
                return new AxiomaParser();
            case "LAS":
                return new LansenParser();
            default:
                throw new Exception("Could not find parser for this manufacturer");
        }
    }
}
