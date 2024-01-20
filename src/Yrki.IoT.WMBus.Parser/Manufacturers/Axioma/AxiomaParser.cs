using System;
using Yrki.IoT.WMBus.Parser.Extensions;
using Yrki.IoT.WMBus.Parser.Manufacturers.Axioma.Payloads;

namespace Yrki.IoT.WMBus.Parser.Manufacturers.Axioma;

public class AxiomaParser : IPayloadParser
{
    IParsedPayload IPayloadParser.ParsePayload(byte meterType, byte protocolVersion, byte[] message)
    {
        var o = message.ToHexString(true);
        System.Diagnostics.Debug.WriteLine(message.ToHexString(true));

        if(meterType == 0x16)
        {
            var axiomaPayload = new Axioma_Qalcosonic_WaterMeter();
            for (int i = 0; i < message.Length - 2; i++)
            {
                if (message[i] == 0x04 && message[i + 1] == 0x6d)
                {
                    axiomaPayload.OnDate = BitConverter.ToInt32(new byte[] { message[i + 2], message[i + 3], message[i + 4], message[i + 5] });
                    i = i + 5;
                }

                if (message[i] == 0x04 && message[i + 1] == 0x20)
                {
                    axiomaPayload.OnTime = BitConverter.ToInt32(new byte[] { message[i + 2], message[i + 3], message[i + 4], message[i + 5] });
                    i = i + 5;
                }

                if (message[i] == 0x04 && message[i + 1] == 0x13)
                {
                    axiomaPayload.TotalVolume = BitConverter.ToInt32(new byte[] { message[i + 2], message[i + 3], message[i + 4], message[i + 5] }) / 1000.0;
                    i = i + 5;
                }
                // ETC;
            }


            return axiomaPayload;
        }
        else
        {
            throw new NotImplementedException("Meter of type " + meterType + " is not implemented.");
        }

        

    }
}
