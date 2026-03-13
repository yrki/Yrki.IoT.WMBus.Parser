using System;
using System.Linq;

namespace Yrki.IoT.WMBus.Parser.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string message)
        {
            message = new string(message.Where(static c => !char.IsWhiteSpace(c)).ToArray());

            if (message.Length % 2 != 0)
            {
                throw new FormatException("Hex message must contain an even number of digits.");
            }

            var bytes = new byte[message.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var index = i * 2;
                bytes[i] = Convert.ToByte(message.Substring(index, 2), 16);
            }

            return bytes;
        }
    }
}
