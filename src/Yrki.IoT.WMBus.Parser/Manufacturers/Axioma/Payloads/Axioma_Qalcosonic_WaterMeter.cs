using System;

namespace Yrki.IoT.WMBus.Parser.Manufacturers.Axioma.Payloads
{
    public class Axioma_Qalcosonic_WaterMeter : IParsedPayload
    {
        /// <summary>
        /// Be aware that the date and time of the meter is fluctuating,
        /// so this is not the exact time of the meter reading.
        /// It is also using the local time on where the factory is, and 
        /// is not correct when it comes to winter/summer-time. (Yes.. I know)
        /// </summary>
        public int? OnDate { get; set; }

        public int? OnTime { get; set; }


        /// <summary>
        /// Accumulated total volume in m3
        /// </summary>
        public double? TotalVolume { get; set; }

        /// <summary>
        /// Accumulated positive volume in m3
        /// </summary>
        public double? PositiveVolume { get; set; }

        /// <summary>
        /// Accumulated negative volume in m3
        /// </summary>
        public double? NegativeVolume { get; set; }

        /// <summary>
        /// Flow rate liters/hour
        /// </summary>
        public int? Flow { get; set; }

        /// <summary>
        /// Supply pipe temperature
        /// </summary>
        public double? Temperature { get; set; }

        /// <summary>
        /// Instantaneous error code
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Error free time (seconds)
        /// </summary>
        public int? ErrorFreeTime { get; set; }

        /// <summary>
        /// Remaining battery capacity
        /// 100 - full, 0 - empty
        /// </summary>
        public int? RemainingBatteryCapacity { get; set; }

        /// <summary>
        /// Date point of last month storage
        /// </summary>
        public DateTimeOffset? LastMonthDate { get; set; }

        /// <summary>
        /// Storage value of volume (m3)
        /// </summary>
        public double? LastMonthVolume { get; set; }

        /// <summary>
        /// Storage positive volume (m3)
        /// </summary>
        public double? LastMonthPositiveVolume { get; set; }

        /// <summary>
        /// Storage negative volume (m3)
        /// </summary>
        public double? LastMonthNegativeVolume { get; set; }
    }
}
