using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using System.Reflection;

namespace pdxbytes.Devices.Core
{
    public abstract class I2CPlug
    {
        private const int DefaultClockRate = 400;
        private const int TransactionTimeout = 1000;

        private I2CDevice.Configuration i2cConfig;
        private I2CDevice i2cDevice;
        

        public I2CPlug(byte address, int clockRateKhz)
        {
            this.i2cConfig = new I2CDevice.Configuration(address, clockRateKhz);
            this.i2cDevice = new I2CDevice(this.i2cConfig);
            
        }
        public I2CPlug(byte address)
            : this(address, DefaultClockRate)
        {
        }

        private void Write(byte[] writeBuffer)
        {
            // create a write transaction containing the bytes to be written to the device
            I2CDevice.I2CTransaction[] writeTransaction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateWriteTransaction(writeBuffer)
            };

            //this.i2cDevice.Config = this.i2cConfig;
            // write the data to the device
            int written = this.i2cDevice.Execute(writeTransaction, TransactionTimeout);
            if (0 == written)
            {
                Debug.Print("Nothing written to the device");
                return;
            }
            //    throw new Exception("The I2C device attached is not responding");

            while (written < writeBuffer.Length)
            {
                byte[] newBuffer = new byte[writeBuffer.Length - written];
                Array.Copy(writeBuffer, written, newBuffer, 0, newBuffer.Length);
                
                writeTransaction = new I2CDevice.I2CWriteTransaction[]
                {
                    I2CDevice.CreateWriteTransaction(newBuffer)
                };

                written += this.i2cDevice.Execute(writeTransaction, TransactionTimeout);
            }

            // make sure the data was sent
            if (written != writeBuffer.Length)
            {
                throw new Exception("Could not write to device.");
            }
        }
        private void Read(byte[] readBuffer)
        {
            // create a read transaction
            I2CDevice.I2CTransaction[] readTransaction = new I2CDevice.I2CTransaction[]
            {
                I2CDevice.CreateReadTransaction(readBuffer)
            };
            this.Read(readTransaction, readBuffer.Length);
        }
        
        protected void Read(I2CDevice.I2CTransaction[] readTransaction, int readLength)
        {

            // read data from the device
            int read = this.i2cDevice.Execute(readTransaction, TransactionTimeout);

            // make sure the data was read
            if (read != readLength)
            {
                Debug.Print("Could not read from the slave device");
                //throw new Exception("Could not read from device.");
            }
        }
        
        protected void WriteToRegister(byte register, byte value, bool repeatStartCondition = true)
        {
            if (repeatStartCondition)
            {
                //Debug.Print("WriteToRegister, single byte5 - repeatStart");
                byte address1 = (byte)((register & 0xff00) >> 8);
                //1111111100000000 mask + shift            
                byte address2 = (byte)(register & 0xff);//0000000011111111 mask
                this.Write(new byte[] { address1, address2, value });
            }
            else
                this.Write(new byte[] { register, value });
            //this.Write(new byte[] { register, value });
        }
        protected void WriteToRegister(byte register, byte[] values)
        {
            // create a single buffer, so register and values can be send in a single transaction
            byte[] writeBuffer = new byte[values.Length + 1];
            writeBuffer[0] = register;
            Array.Copy(values, 0, writeBuffer, 1, values.Length);

            this.Write(writeBuffer);
        }
        protected void ReadFromRegister(byte register, byte[] readBuffer, bool repeatStartCondition = true)
        {
            if(repeatStartCondition)
            {
                byte address1 = (byte)((register & 0xff00) >> 8);
                //1111111100000000 mask + shift            
                byte address2 = (byte)(register & 0xff);//0000000011111111 mask
                this.Write(new byte[] { address1, address2 });
            }
            else
                this.Write(new byte[] { register });
            //this.Read(readBuffer);
            this.Read(readBuffer);
        }
        protected byte ReadRegisterByte(byte register, bool repeatStartCondition = true)
        {
            var buff = new byte[1];
            this.ReadFromRegister(register, buff, repeatStartCondition);
            return buff[0];
        }

        I2CDevice.I2CReadTransaction CreateReadTransaction(byte[] buffer, uint internalAddress, byte internalAddressSize)
        {
            I2CDevice.I2CReadTransaction readTransaction = I2CDevice.CreateReadTransaction(buffer);
            Type readTransactionType = typeof(I2CDevice.I2CReadTransaction);

            FieldInfo fieldInfo = readTransactionType.GetField("Custom_InternalAddress", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(readTransaction, internalAddress);

            fieldInfo = readTransactionType.GetField("Custom_InternalAddressSize", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(readTransaction, internalAddressSize);

            return readTransaction;
        }
    }
}
