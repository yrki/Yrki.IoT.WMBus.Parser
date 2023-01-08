using System;
using System.Linq;

namespace Yrki.IoT.WMBus.Parser
{
    public class Parser
    {

        public WMBusMessage Parse(string message)
        {
            byte[] bytes = ConvertHexStringToByteArray(message);

            return Parse(bytes);
        }

        public WMBusMessage Parse(byte[] message)
        {
            var wmBusMessage = new WMBusMessage();

            wmBusMessage.LField = message[0];
            wmBusMessage.CField = message[1];
            wmBusMessage.MField = ConvertToManufacturerCode(message.AsSpan(2, 2).ToArray());
            wmBusMessage.AField = ConvertToUniqueNumber(message.AsSpan(4, 4).ToArray());
            wmBusMessage.Version = message[8];
            wmBusMessage.DeviceType = (DeviceType)message[9];
            wmBusMessage.CIField = message[10];
            wmBusMessage.Sequence = message[11];
            wmBusMessage.Status = message[12];
            wmBusMessage.EncryptionMethod = GetEncryptionMethod(message.AsSpan(13, 2).ToArray());
            
            return wmBusMessage;

        }

        private EncryptionMethod GetEncryptionMethod(byte[] bytes)
        {
            if(bytes[0] == 0x00 && bytes[1] == 0x00)
            {
                return EncryptionMethod.None;
            }
            else
            {
                return EncryptionMethod.Aes128;
            }
        }

        private string ConvertToUniqueNumber(byte[] bytes)
        {
            var afield = String.Empty;
            
            foreach (var b in bytes.Reverse())
            {
                afield += b.ToString("X2");
            }

            return afield;
        }

        private string ConvertToManufacturerCode(byte[] bytes)
        {
            var mfield = BitConverter.ToUInt16(bytes);
            var characters = new char[3];

            characters[0] = (char)((mfield / 1024) + 64);
            characters[1] = (char)(((mfield % 1024) / 32) + 64);
            characters[2] = (char)((mfield % 32) + 64);

            return new string(characters);
        }

        private byte[] ConvertHexStringToByteArray(string message)
        {
            var bytes = new byte[message.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var index = i * 2;
                bytes[i] = Convert.ToByte(message.Substring(index, 2), 16);
            }

            return bytes;
        }
    }




    public enum EncryptionMethod
    {
        None = 0,
        Aes128 = 1
    }

    public enum MeterType
    {
        Water_Cold,
        Water_Hot

    }
}

