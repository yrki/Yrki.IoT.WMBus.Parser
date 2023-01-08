namespace Yrki.IoT.WMBus.Parser
{
    public enum DeviceType
    {
        ElectricityMeter = 0x02,
        GasMeter = 0x03,
        HeatMeter = 0x04,
        WarmWater = 0x06,
        WaterMeter = 0x07,
        HeatCostAllocator = 0x08,
        CoolingMeterOutlet = 0x0A,
        CoolingMeterInlet = 0x0B,
        HeatMeterInlet = 0x0C,

        CombinedHeatCoolingMeter = 0x0D,
        HotWater = 0x15,
        ColdWater = 0x16,
        WasteWater = 0x28,
        
        Breaker = 0x20,
        ValveGasOrWater = 0x21,
        CustomerUnitDisplayDevice = 0x25,
        CommunicationController = 0x31,
        UnidirectionalRepeater = 0x32,
        BidirectionalRepeater = 0x33,
        RadioConverterSystemSide = 0x36,
        RadioConverterMeterSide = 0x37,
        
        Other = 0x00,

        OilMeter = 0x01,
        SteamMeter = 0x05,
        CompressedAir = 0x09,
        BusSystemComponent = 0x0E,

        UnknownDeviceType = 0x0F,
        ConsumptionMeter1 = 0x10,
        ConsumptionMeter2 = 0x11,
        ConsumptionMeter3 = 0x12,
        ConsumptionMeter4 = 0x13,
        CalorificValueMeter = 0x14,
        DualRegisterHotVColdWaterMeter = 0x17,
        PressureMeter = 0x18,

        ADConverter = 0x19,
        SmokeDetector = 0x1A,
        RoomSensor = 0x1B,
        Garbage = 0x29,
        CarbonDioxide = 0x2A,

        Sensor1 = 0x1D,
        Sensor2 = 0x1E,
        Sensor3 = 0x1F,
        SwitchingDevice1 = 0x22,
        SwitchingDevice2 = 0x23,
        SwitchingDevice3 = 0x24,
        CustomerUnit1 = 0x26,
        CustomerUnit2 = 0x27,
        EnrionmentalMeter1 = 0x2B,
        EnrionmentalMeter2 = 0x2C,
        EnrionmentalMeter3 = 0x2D,
        EnrionmentalMeter4 = 0x2E,
        EnrionmentalMeter5 = 0x2F,
        SystemDevice1 = 0x30,
        SystemDevice2 = 0x34,
        SystemDevice3 = 0x35,
        SystemDevice4 = 0x38,
        SystemDevice5 = 0x39,
        SystemDevice6 = 0x3A,
        SystemDevice7 = 0x3B,
        SystemDevice8 = 0x3C,
        SystemDevice9 = 0x3D,
        SystemDevice10 = 0x3E,
        SystemDevice11 = 0x3F,       
        NotApplicable = 0xFF
    }
}

