using Microsoft.SPOT.Hardware;
using pdxbytes.DeviceInterfaces;
using pdxbytes.DeviceInterfaces.Configuration;
using pdxbytes.Devices.Display;
using System;

namespace pdxbytes.DeviceDrivers.Display
{
    public class ILI9341DeviceConfiguration : DeviceConfiguration, IDeviceConfiguration
    {
        public ILI9341DeviceConfiguration(Cpu.Pin cs, Cpu.Pin dc, Cpu.Pin reset, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)40000)
        {
            this.ChipSelect = cs;
            this.DC = dc;
            this.Reset = reset;
            this.Spi = spiModule;
            this.SpeedKhz = speedKHz;
        }

        private Cpu.Pin ChipSelect;
        private Cpu.Pin DC;
        private Cpu.Pin Reset;
        private SPI.SPI_module Spi;
        private uint SpeedKhz;

        public override IDevice CreateDevice()
        {
            return new ILI9341_240x320CAP(this.ChipSelect, this.DC, this.Reset, this.Spi, this.SpeedKhz);
        }
        
    }
}
