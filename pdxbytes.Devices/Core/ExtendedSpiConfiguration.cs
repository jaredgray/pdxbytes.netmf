using System;
using Microsoft.SPOT;
using System.Reflection;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Devices.Core
{
    public class ExtendedSpiConfiguration : SPI.Configuration
    {
        public ExtendedSpiConfiguration(
            Cpu.Pin ChipSelect_Port,
            bool ChipSelect_ActiveState,
            uint ChipSelect_SetupTime,
            uint ChipSelect_HoldTime,
            bool Clock_IdleState,
            bool Clock_Edge,
            uint Clock_RateKHz,
            SPI.SPI_module SPI_mod
            )
            : this(ChipSelect_Port,
                  ChipSelect_ActiveState,
                  ChipSelect_SetupTime,
                  ChipSelect_HoldTime,
                  Clock_IdleState,
                  Clock_Edge,
                  Clock_RateKHz,
                  SPI_mod,
                  Cpu.Pin.GPIO_NONE,
                  false,
                  16)
        {
        }

        public ExtendedSpiConfiguration(
            Cpu.Pin ChipSelect_Port,
            bool ChipSelect_ActiveState,
            uint ChipSelect_SetupTime,
            uint ChipSelect_HoldTime,
            bool Clock_IdleState,
            bool Clock_Edge,
            uint Clock_RateKHz,
            SPI.SPI_module SPI_mod,
            uint BitsPerTransfer
            )
            : this(ChipSelect_Port,
                  ChipSelect_ActiveState,
                  ChipSelect_SetupTime,
                  ChipSelect_HoldTime,
                  Clock_IdleState,
                  Clock_Edge,
                  Clock_RateKHz,
                  SPI_mod,
                  Cpu.Pin.GPIO_NONE,
                  false,
                  BitsPerTransfer)
        {
        }

        public ExtendedSpiConfiguration(
                        Cpu.Pin ChipSelect_Port,
                        bool ChipSelect_ActiveState,
                        uint ChipSelect_SetupTime,
                        uint ChipSelect_HoldTime,
                        bool Clock_IdleState,
                        bool Clock_Edge,
                        uint Clock_RateKHz,
                        SPI.SPI_module SPI_mod,
                        Cpu.Pin BusyPin,
                        bool BusyPin_ActiveState

                    )
            : this(ChipSelect_Port, ChipSelect_ActiveState, ChipSelect_SetupTime, ChipSelect_HoldTime, Clock_IdleState, Clock_Edge, Clock_RateKHz, SPI_mod, BusyPin, BusyPin_ActiveState, 16)
        {
        }

        public ExtendedSpiConfiguration(
                                Cpu.Pin ChipSelect_Port,
                                bool ChipSelect_ActiveState,
                                uint ChipSelect_SetupTime,
                                uint ChipSelect_HoldTime,
                                bool Clock_IdleState,
                                bool Clock_Edge,
                                uint Clock_RateKHz,
                                SPI.SPI_module SPI_mod,
                                Cpu.Pin BusyPin,
                                bool BusyPin_ActiveState,
                                uint BitsPerTransfer
                            )
            : base(ChipSelect_Port, ChipSelect_ActiveState, ChipSelect_SetupTime, ChipSelect_HoldTime, Clock_IdleState, Clock_Edge, Clock_RateKHz, SPI_mod, BusyPin, BusyPin_ActiveState)
        {
            // Netduino's MCU can read/write 8 to 16 bits per transfer (variable-bit SPI)
            if (BitsPerTransfer < 8 || BitsPerTransfer > 16)
                throw new ArgumentException();
            
        }
        

    }
}
