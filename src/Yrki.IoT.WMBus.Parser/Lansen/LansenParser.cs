using System;
using System.Collections.Generic;
using System.Text;
using Yrki.IoT.WMBus.Parser.Lansen.Messages;

namespace Yrki.IoT.WMBus.Parser.Lansen
{
    internal class LansenParser : IInternalParser
    {
        private const int Offset = 18;

        public IParsedMessage Parse(byte meterType, byte protocolVersion, byte[] message)
        {
            var result = new LansenE2_CO2_S();

            if(meterType == 0x2A && protocolVersion == 0x0F)
            {

                // DR1
                result.TemperatureLastMeasuredValue = TemperatureLastMeasuredValue(message);

                // DR2
                result.TemperatureAverageLastHour = TemperatureAverageLastHour(message);
                
                // DR3
                result.TemperatureAverageLast24Hours = TemperatureAverageLast24Hours(message);
                
                // DR4
                result.HumidityLastMeasuredValue = HumidityLastMeasuredValue(message);
                
                // DR5
                result.HumidityAverageLastHour = HumidityAverageLastHour(message);
                
                // DR6
                result.HumidityAverageLast24Hours = HumidityAverageLast24Hours(message);

                // DR7
                result.CO2LastMeasuredValue = CO2LastMeasuredValue(message);

                // DR8
                result.CO2AverageLastHour = CO2AverageLastHour(message);

                // DR9
                result.CO2AverageLast24Hours = CO2AverageLast24Hours(message);

                // DR10
                result.CO2LastUsedCalibrationValue = CO2LastUsedCalibrationValue(message);

                // DR11
                result.CO2MinutesToNextCalibration = CO2MinutesToNextCalubration(message);

                // DR12
                result.SoundLevelLastMeasuredValue = SoundLevelLastMeasuredValue(message);

                // DR13
                result.SoundLevelAverageLastHour = SoundLevelAverageLastHour(message);

                // DR14
                result.OnTimeInDays = OnTimeInDays(message);

                // DR15
                result.OperatingTimeInDays = OperatingTimeInDays(message);

                // DR16
                result.ProductVersion = ProductVersion(message);

                // DR17
                result.StatusAndIndications = StatusAndIndications(message);

            }

            return result;
        }


        private double? TemperatureLastMeasuredValue(byte[] message)
        {
            if (message[18 - Offset] == 0x02 && message[19 - Offset] == 0x65)
            {
                var lsb = (byte)(message[20 - Offset]);
                var msb = (byte)(message[21 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.01;
            }

            return null;
        }

        private double? TemperatureAverageLastHour(byte[] message)
        {
            if (message[22 - Offset] == 0x42 && message[23 - Offset] == 0x65)
            {
                var lsb = (byte)(message[24 - Offset]);
                var msb = (byte)(message[25 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.01;
            }

            return null;
        }

        private double? TemperatureAverageLast24Hours(byte[] message)
        {
            if (message[26 - Offset] == 0x82 && message[27 - Offset] == 0x01 && message[28 - Offset] == 0x65)
            {
                var lsb = (byte)(message[29 - Offset]);
                var msb = (byte)(message[30 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.01;
            }

            return null;
        }

        private double? HumidityLastMeasuredValue(byte[] message)
        {
            if (message[31 - Offset] == 0x02 && message[32 - Offset] == 0xFB && message[33 - Offset] == 0x1A)
            {
                var lsb = (byte)(message[34 - Offset]);
                var msb = (byte)(message[35 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.1;
            }

            return null;
        }

        private double? HumidityAverageLastHour(byte[] message)
        {
            if (message[36 - Offset] == 0x42 && message[37 - Offset] == 0xFB && message[38 - Offset] == 0x1A)
            {
                var lsb = (byte)(message[39 - Offset]);
                var msb = (byte)(message[40 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.1;
            }

            return null;
        }

        private double? HumidityAverageLast24Hours(byte[] message)
        {
            if (message[41 - Offset] == 0x82 && message[42 - Offset] == 0x01 && message[43 - Offset] == 0xFB && message[44 - Offset] == 0x1A)
            {
                var lsb = (byte)(message[45 - Offset]);
                var msb = (byte)(message[46 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0) * 0.1;
            }

            return null;
        }


        private double? CO2LastMeasuredValue(byte[] message)
        {
            if (message[47 - Offset] == 0x02 && message[48 - Offset] == 0xFD && message[49 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[50 - Offset]);
                var msb = (byte)(message[51 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private double? CO2AverageLastHour(byte[] message)
        {
            if (message[52 - Offset] == 0x42 && message[53 - Offset] == 0xFD && message[54 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[55 - Offset]);
                var msb = (byte)(message[56 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private double? CO2AverageLast24Hours(byte[] message)
        {
            if (message[57 - Offset] == 0x82 && message[58 - Offset] == 0x01 && message[59 - Offset] == 0xFD && message[60 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[61 - Offset]);
                var msb = (byte)(message[62 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private double? CO2LastUsedCalibrationValue(byte[] message)
        {
            if (message[63 - Offset] == 0xC2 && message[64 - Offset] == 0x01 && message[65 - Offset] == 0xFD && message[66 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[67 - Offset]);
                var msb = (byte)(message[68 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private int? CO2MinutesToNextCalubration(byte[] message)
        {
            if (message[69 - Offset] == 0x82 && 
                message[70 - Offset] == 0x40 && 
                message[71 - Offset] == 0xFD && 
                message[72 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[73 - Offset]);
                var msb = (byte)(message[74 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private double? SoundLevelLastMeasuredValue(byte[] message)
        {
            if (message[75 - Offset] == 0x82 && 
                message[76 - Offset] == 0x80 && 
                message[77 - Offset] == 0x40 && 
                message[78 - Offset] == 0xFD &&
                message[79 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[80 - Offset]);
                var msb = (byte)(message[81 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private double? SoundLevelAverageLastHour(byte[] message)
        {
            if (message[82 - Offset] == 0xC2 &&
                message[83 - Offset] == 0x80 &&
                message[84 - Offset] == 0x40 &&
                message[85 - Offset] == 0xFD &&
                message[86 - Offset] == 0x3A)
            {
                var lsb = (byte)(message[87 - Offset]);
                var msb = (byte)(message[88 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }


        private int? OnTimeInDays(byte[] message)
        {
            if (message[89 - Offset] == 0x82 &&
                message[90 - Offset] == 0x02 &&
                message[91 - Offset] == 0x23)
            {
                var lsb = (byte)(message[92 - Offset]);
                var msb = (byte)(message[93 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private int? OperatingTimeInDays(byte[] message)
        {
            if (message[94 - Offset] == 0x02 &&
                message[95 - Offset] == 0x27)
            {
                var lsb = (byte)(message[96 - Offset]);
                var msb = (byte)(message[97 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private int? ProductVersion(byte[] message)
        {
            if (message[98 - Offset] == 0x02 &&
                message[99 - Offset] == 0xFD &&
                message[100 - Offset] == 0x0F)
            {
                var lsb = (byte)(message[101 - Offset]);
                var msb = (byte)(message[102 - Offset]);
                return BitConverter.ToInt16(new byte[] { lsb, msb }, 0);
            }

            return null;
        }

        private string[] StatusAndIndications(byte[] message)
        {
            if (message[103 - Offset] == 0x01 &&
                message[104 - Offset] == 0xFD &&
                message[105 - Offset] == 0x1B)
            {
                var value = (byte)(message[106 - Offset]);
                return new string[0];
            }

            return null;
        }
    }
}
