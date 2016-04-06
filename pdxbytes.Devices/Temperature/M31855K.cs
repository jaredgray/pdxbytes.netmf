using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using pdxbytes.Devices.Core;

namespace pdxbytes.Devices.Temperature
{
    public class M31855K
    {
        //public M31855K(Cpu.Pin cs, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)40000)
        //{
        //    this.spi = new SPI(new ExtendedSpiConfiguration(
        //        SPI_mod: spiModule,
        //        ChipSelect_Port: cs,
        //        ChipSelect_ActiveState: false,
        //        ChipSelect_SetupTime: 0,
        //        ChipSelect_HoldTime: 0,
        //        Clock_IdleState: false,
        //        Clock_Edge: true,
        //        Clock_RateKHz: speedKHz,
        //        BitsPerTransfer: 32));

        //}

        public M31855K(Cpu.Pin sclk, Cpu.Pin cs, Cpu.Pin data)
        {
            spi = new CASPI(sclk, cs, data);
        }

        private CASPI spi;


        //public double ReadCelsius()
        //{
        //    byte[] buffer = new byte[4];
        //    spi.WriteRead(new byte[] { 0x00 }, buffer);

        //    return 0;
        //}
        public double ReadCelsius()
        {
            var result = spi.ReadInt32();

            if ((result & 0x7) > 0)
                return double.NaN;

            if ((result & 0x80000000) > 0) // negative value. shift 18 bits and 
            {
                // Negative value, drop the lower 18 bits and explicitly extend sign bits.
                result = (int)(0xFFFFC000 | ((result >> 18) & 0x00003FFFF));
            }
            else
            {
                // Positive value, just drop the lower 18 bits.
                result >>= 18;
            }
            //Serial.println(v, HEX);

            double centigrade = result;

            // LSB = 0.25 degrees C
            centigrade *= 0.25;
            return centigrade;
        }
    }
}
