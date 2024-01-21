using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks.Sources;
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
            var initializationVector = GetInitVector(message);
            var payload = message.AsSpan(15, message.Length - 15).ToArray();
            var decryptedPayload = DecryptPayload(header, initializationVector, payload, encryptionKey);

#if DEBUG
            System.Diagnostics.Debug.WriteLine(message.ToHexString());
            System.Diagnostics.Debug.WriteLine(encryptionKey);
            System.Diagnostics.Debug.WriteLine(initializationVector.ToHexString());
            System.Diagnostics.Debug.WriteLine(payload.ToHexString());
            System.Diagnostics.Debug.WriteLine(decryptedPayload.ToHexString());
#endif

            return parser.ParsePayload(message[9], message[8], decryptedPayload);
        }

        private byte[] GetInitVector(byte[] message)
        {
            var blockSize = 16;
            var iv = new byte[blockSize];

            iv[0] = message[2]; // Manufacturer byte 1
            iv[1] = message[3]; // Manufacturer byte 2
            iv[2] = message[4]; // Address byte 1
            iv[3] = message[5]; // Address byte 2
            iv[4] = message[6]; // Address byte 3
            iv[5] = message[7]; // Address byte 4
            iv[6] = message[8]; // Version byte 1
            iv[7] = message[9]; // Device type byte 1

            // Use the AccessNumber as padding
            // Note: This can be differnet index on T1 and C1 messages, so must be handled differently
            for (int i = 8; i < 16; i++)
            {
                iv[i] = message[11];
            }

            return iv;
        }

        private byte[] DecryptPayload(WMBusMessage header, byte[] initializationVector, byte[] payload, string encryptionKey)
        {
            byte[] decryptedPayload = null;
            if (header.EncryptionMethod == EncryptionMethod.Aes128)
            {
                decryptedPayload = DecryptAes128(initializationVector, payload, encryptionKey);
            }
            else if(header.EncryptionMethod == EncryptionMethod.None)
            {
                decryptedPayload = payload;
            }
            else
            {
                throw new NotImplementedException("Encryption method is not implemented");
            }

            if (!IsSuccessfullyDecrypted(decryptedPayload)) throw new ApplicationException("Could not decrypt message with current key");

            return TrimEncryptionVerificationBytes(decryptedPayload);
        }

        private byte[] DecryptAes128(byte[] initializationVector, byte[] payload, string encryptionKey)
        {
            byte[] decryptedPayload;
            // Decrypt the payload
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = encryptionKey.ToByteArray();
                aesAlg.IV = initializationVector;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.Zeros;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var decrypted = new MemoryStream(payload.Length);


                using (var encrypted = new MemoryStream(payload))
                {
                    using (var csDecrypt = new CryptoStream(encrypted, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Flush();
                        csDecrypt.CopyTo(decrypted);
                    }
                }

                decryptedPayload = decrypted.GetBuffer();
            }

            return decryptedPayload;
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
            else if (bytes[1] == 0x05)
            {
                return EncryptionMethod.Aes128;
            }
            else
            {
                throw new NotImplementedException("Encryption method is not implemented");
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

