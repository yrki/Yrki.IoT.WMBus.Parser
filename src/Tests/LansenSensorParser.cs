using Yrki.IoT.WMBus.Parser;
using Yrki.IoT.WMBus.Parser.Lansen.Messages;
using FluentAssertions;

namespace Tests
{
    [TestClass]
    public class LansenSensorParserTest
    {
        //HEADER: 
        //XX                   // L-Field 
        //44                   // C-Field 
        //33 30                // M-Field 
        //67 00 01 00          // A-Field
        //0F                   // Protocol version 
        //2A                   // Meter type 
        //7A                   // CI-Field 
        //07                   // Access No 
        //00                   // Status Table 1 
        //03                   // Configuration - Number of encrypted blocks 
        //00                   // Configuration - Encryption 0x00 = no encryption, 0x05 - Encryption mode 5
        //PAYLOAD: 
        //2F 2F                // Encrypt Verification
        //02 65 22 11          // DR1: Temperature: Last measured value
        //42 65 65 43          // DR2: Temperature: Average last hour
        //82 01 65 22 11       // DR3: Temperature: Average last 24 hours
        //02 FB 1A 22 11       // DR4: Humidity: Last measured value
        //42 FB 1A 22 11       // DR5: Humidity: Average last hour
        //82 01 FB 1A 22 11    // DR6: Humidity: Average last 24 hours
        //02 FD 3A 22 11       // DR7: CO2: Last measured value
        //42 FD 3A 33 22       // DR8: CO2: Average last hour
        //82 01 FD 3A 02 01    // DR9: CO2: Average last 24 hours
        //C2 01 FD 3A 24 23    // DR10: Last used calibration value
        //82 40 FD 3A 02 00    // DR11: Minutes to next calibration
        //82 80 40 FD 3A 28 00 // DR12: Sound level (dB): Last measured value
        //C2 80 40 DF 3A 2B 00 // DR13: Sound level (dB): Average last hour
        //82 02 23 00 00       // DR14: On Time in days (since power up)
        //02 27 00 00          // DR15: Operating time in days (Total)
        //02 FD 0F 04 00       // DR16: Version
        //01 FD 1B 00          // DR17: Table 2 : Status and indications


        [TestMethod]
        public void Lansen_LAN_WMBUS_E2_CO2_S()
        {
            // Arrange  
            var message = """"
                6A
                44
                33 30
                67 00 01 00
                0F
                2A
                7A
                07
                00
                00 00
                2F 2F
                02 65 22 11
                42 65 65 43
                82 01 65 22 11
                02 FB 1A 22 11
                42 FB 1A 22 11
                82 01 FB 1A 22 11
                02 FD 3A 22 11
                42 FD 3A 33 22
                82 01 FD 3A 02 01
                C2 01 FD 3A 24 23
                82 40 FD 3A 02 00
                82 80 40 FD 3A 28 00
                C2 80 40 FD 3A 2B 00
                82 02 23 00 00
                02 27 00 00
                02 FD 0F 04 00
                01 FD 1B 00
                """";

            message = message.Replace(" ", "");
            message = message.Replace("\r\n", "");

            var parser = new Parser();


            // Act
            var result = parser.Parse(message);

            // Assert
            result.MField.Should().Be("LAS");
            result.AField.Should().Be("00010067");
            result.DeviceType.Should().Be(DeviceType.CarbonDioxide);
            result.EncryptionMethod.Should().Be(EncryptionMethod.None);
            result.ParsedPayload.Should().BeOfType<LansenE2_CO2_S>();
            
            var payload = (LansenE2_CO2_S)result.ParsedPayload;
            payload.TemperatureLastMeasuredValue.Should().Be(43.86);
            payload.TemperatureAverageLastHour.Should().Be(172.53);
            payload.TemperatureAverageLast24Hours.Should().Be(43.86);
            payload.HumidityLastMeasuredValue.Should().Be(438.6);
            payload.HumidityAverageLastHour.Should().Be(438.6);
            payload.HumidityAverageLast24Hours.Should().Be(438.6);
            payload.CO2LastMeasuredValue.Should().Be(4386);
            payload.CO2AverageLastHour.Should().Be(8755);
            payload.CO2AverageLast24Hours.Should().Be(258);
            payload.CO2LastUsedCalibrationValue.Should().Be(8996);
            payload.CO2MinutesToNextCalibration.Should().Be(2);
            payload.SoundLevelLastMeasuredValue.Should().Be(40);
            payload.SoundLevelAverageLastHour.Should().Be(43);
            payload.OnTimeInDays.Should().Be(0);
            payload.OperatingTimeInDays.Should().Be(0);
            payload.ProductVersion.Should().Be(4);
            payload.StatusAndIndications.Should().HaveCount(0);
        }

    }


    public enum DeviceStatus
    {
        NoError,
        DeviceNotActivated,
        LowBattery,
        CO2CalibrationNotYetDone
    }

    public enum SensorStatus
    {
        DeviceDoesNotSupportIndication,
        DeviceSupportsIndication,
        VisualLedIndicationEnabled,
        SoundIndicationEnabled,
        CO2ExternalSensorErrorOrFirstMeasurementNotDoneYet,
        CO2ValueWasUpdatedWithThisPacket,
        DeviceNotActivated,
        CO2CalibrationNotYetDone
    }
}
