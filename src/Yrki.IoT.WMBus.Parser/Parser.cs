using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Yrki.IoT.WMBus.Parser.Extensions;

namespace Yrki.IoT.WMBus.Parser
{
    public class Parser
    {
        public WMBusMessage ParseHeader(string hexMessage)
        {
            return ParseHeader(hexMessage.ToByteArray());
        }

        public WMBusMessage ParseHeader(byte[] message)
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


        public IParsedPayload ParsePayload(string hexMessage, string encryptionKey)
        {
            return ParsePayload(hexMessage.ToByteArray(), encryptionKey);
        }

        public IParsedPayload ParsePayload(byte[] message, string encryptionKey)
        {
            var header = ParseHeader(message);
            var parser = ParserFactory.GetParser(header.MField);

            var payload = message.AsSpan(15, message.Length - 15).ToArray();

            var decryptedPayload = DecryptPayload(header, payload, encryptionKey);

            return parser.ParsePayload(message[9], message[8], decryptedPayload);
        }


        private byte[] DecryptPayload(WMBusMessage header, byte[] payload, string encryptionKey)
        {
            if (header.EncryptionMethod != EncryptionMethod.None)
            {
                // TODO: Get the Init Vector from the message
                // Decrypt the payload
                throw new NotImplementedException();
            }

            if (!IsSuccessfullyDecrypted(payload)) throw new ApplicationException("Could not decrypt message with current key");

            return TrimEncryptionVerificationBytes(payload);
        }

        private static byte[] TrimEncryptionVerificationBytes(byte[] payload)
        {
            return payload.AsSpan(2, payload.Length - 2).ToArray();
        }

        private bool IsSuccessfullyDecrypted(byte[] payload)
        {
            if (payload[0] == 0x2F && payload[1] == 0x2F) return true;
            return false;
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

