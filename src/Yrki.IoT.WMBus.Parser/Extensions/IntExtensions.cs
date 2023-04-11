namespace Yrki.IoT.WMBus.Parser.Extensions
{
    public static class IntExtensions
    {
        public static bool BitIsSetAtPosition(this int value, int position)
        {
            return (value >> position & 1) == 1;
        }
    }
}
