using System;

namespace Yrki.IoT.WMBus.Parser.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string message)
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
}
