namespace Yrki.IoT.WMBus.Parser.Manufacturers.Lansen.Payloads;

public class LansenE2_CO2_S : IParsedPayload
{
    public double? TemperatureLastMeasuredValue { get; set; }
    public double? TemperatureAverageLastHour { get; set; }
    public double? TemperatureAverageLast24Hours { get; set; }
    public double? HumidityLastMeasuredValue { get; set; }
    public double? HumidityAverageLastHour { get; set; }
    public double? HumidityAverageLast24Hours { get; set; }
    public double? CO2LastMeasuredValue { get; set; }
    public double? CO2AverageLastHour { get; set; }
    public double? CO2AverageLast24Hours { get; set; }
    public double? CO2LastUsedCalibrationValue { get; set; }
    public int? CO2MinutesToNextCalibration { get; set; }
    public double? SoundLevelLastMeasuredValue { get; set; }
    public double? SoundLevelAverageLastHour { get; set; }
    public int? OnTimeInDays { get; set; }
    public int? OperatingTimeInDays { get; set; }
    public int? ProductVersion { get; set; }
    public string[] StatusAndIndications { get; set; }

}
