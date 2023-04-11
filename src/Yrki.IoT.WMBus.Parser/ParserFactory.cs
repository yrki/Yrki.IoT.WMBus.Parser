using System;
using System.Collections.Generic;
using System.Text;
using Yrki.IoT.WMBus.Parser.Lansen;
using Yrki.IoT.WMBus.Parser.Lansen.Messages;

namespace Yrki.IoT.WMBus.Parser
{
    internal static class ParserFactory
    {
        internal static IInternalParser GetParser(string manufacturerId)
        {
            switch (manufacturerId)
            {
                case "LAS":
                    return new LansenParser();
                default:
                    throw new Exception("Could not find parser for this manufacturer");
            }
        }
    }
}
