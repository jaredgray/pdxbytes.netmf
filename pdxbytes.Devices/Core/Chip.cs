using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace pdxbytes.Devices.Core
{
    class Chip
    {
        /// <summary>
        /// Writes a single Byte of data to the chip
        /// </summary>
        /// <param name="Address">Memory Address. Must not exceed 4095</param>
        /// <param name="Data">a single Byte of Data</param>
        public void WriteByte(uint Address, byte Data)
        {
            if (Address > 4095) { throw new Exception("Address cannot exceed 4095"); }
            byte[] iByte = new byte[1];
            iByte[0] = Data;
            //byte address1a = (byte)(Address >> 8);            
            //byte address2a = (byte)(Address - (address1a << 8));            
            byte address1 = (byte)((Address & 0xff00) >> 8);
            //1111111100000000 mask + shift            
            byte address2 = (byte)(Address & 0xff);//0000000011111111 mask            
            var writeX = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(new byte[] { address1, address2, Data }) };
            //if (EEPROM.Execute(writeX,TimeOutMs)==0)
            //{
            //    throw new Exception("I2C transaction failed");
            //}
        }
        ///// <summary> 
        ///// Reads a Single Byte from the Chip from a single memory location 0-4095
        /////  </summary> 
        /////  <param name="Address">Memory Address </param> 
        ///// <returns></returns>   
        //public byte ReadByte(uint Address)
        //{
        //    if (Address > maxBytes)
        //    {                throw new Exception("Address cannot exceed 4095");            }
        //    byte[] returnByte=new byte[1];
        //    byte address1 = (byte)((Address & 0xff00) >> 8);
        //    byte address2 = (byte)(Address & 0xff);
        //    var readX = new I2CDevice.I2CTransaction[] 
        //    {
        //        I2CDevice.CreateWriteTransaction(new byte[] {address1, address2}), I2CDevice.CreateReadTransaction(returnByte)
        //    };
        //    if (EEPROM.Execute(readX, TimeOutMs) == 0)
        //    {
        //        throw new Exception("I2C transaction failed");
        //    }
        //    return returnByte[0];     
        //}
    }
}
