using System;
using System.Collections.Generic;
using System.Text;

namespace Yrki.IoT.WMBus.Parser.Extensions
{
    internal static class ByteArrayExtensions
    {
        //Convert byte array to hex string
        public static string ToHexString(this byte[] bytes)
        {
            return bytes.ToHexString(false);
        }

        //Convert byte array to hex string
        public static string ToHexString(this byte[] bytes, bool spaceBetweenCharacters)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
                if(spaceBetweenCharacters)
                {
                    hex.Append(" ");
                }
            }

            return hex.ToString();
        }
    }
}
