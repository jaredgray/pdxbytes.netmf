using System;
using Microsoft.SPOT;
using System.Threading;
using System.Text;
using System.IO.Ports;
using Microsoft.SPOT.Hardware;
using System.Collections;

namespace pdxbytes.Devices.Bluetooth
{
    public enum DeviceStatusCodes
    {
        Initializing = 0,
        Ready = 1,
        Inquiring = 2,
        Connecting = 3,
        Connected = 4
    }
    public enum WorkingModes
    {
        Slave = 0,
        Master = 1
    }
    public class BluetoothDeviceConfig
    {
        public BluetoothDeviceConfig()
        {
            this.AllowPairedDevicesToConnect = true;
            this.AutoConnectEnabled = true;
            this.PinCode = "0000";
            this.WaitBetweenSentCommands = 500;
        }
        public string DeviceName { get; set; }
        public bool AutoConnectEnabled { get; set; }
        public bool AllowPairedDevicesToConnect { get; set; }
        public WorkingModes WorkingMode { get; set; }
        public string PinCode { get; set; }
        /// <summary>
        /// in milliseconds, the amount of time to wait in-between commands sent to the device
        /// </summary>
        public int WaitBetweenSentCommands { get; set; }
    }
    public class BluetoothDevice : IDisposable
    {
        public delegate void MessageRecievedEventHandler(string data, DateTime time);
        public event MessageRecievedEventHandler DataRecieved;

        public delegate void DeviceStatusChangedEventHandler(BluetoothDevice device, DeviceStatusCodes status);
        public event DeviceStatusChangedEventHandler DeviceStatusChanged;

        #region Constructor(s) and Dispose

        /// <summary>
        /// Default constructor.
        /// </summary>
        private BluetoothDevice()
        {
            ConnectionStatus = ConnectionState.Disconnected;
        }
#if XbeeSPI
        XBeeSPI xbee;
        static OutputPort tx;
        static InputPort rx;
#endif

        private bool IsSetup = false;

        StringBuilder DebugData = new StringBuilder();

        StringBuilder StreamData = new StringBuilder();

        /// <summary>
        /// Create a new instance of the RovingNetworks RN-42 class nad open a connection
        /// using the specified connection setting.
        /// </summary>
        /// <param name="port">Serial port to use to talk to the Bluetooth device.</param>
        /// <param name="baudRate">Baud rate for the device.</param>
        /// <param name="parity">Parity settings for the device.</param>
        /// <param name="dataBits">Number of data bits for the device.</param>
        /// <param name="stopBits">Number of stop bits for the device.</param>
        public BluetoothDevice(string port, BaudRate baudRate, Parity parity, int dataBits, StopBits stopBits, string deviceName = null)
        {
#if XbeeSPI
            uint rate = (uint)baudRate;
            this.xbee = new XBeeSPI(Pins.GPIO_NONE, rate);
#else
            this.Port = port;
            this.CurrentBaud = BaudRate.Baudrate9600;
            this.DesiredBaud = baudRate;
            this.Parity = parity;
            this.DataBits = DataBits;
            this.StopBits = stopBits;
            this.DeviceName = deviceName;


            this.DeviceConfiguration = new BluetoothDeviceConfig()
            {
                DeviceName = deviceName,
                AutoConnectEnabled = false
            };
            _bluetoothModule = new SerialPort(port, (int)BaudRate.Baudrate9600, parity, dataBits, stopBits);

            ConnectionStatus = ConnectionState.Data;
            _bluetoothModule.DataReceived += new SerialDataReceivedEventHandler(bluetoothModule_DataReceived);
            _bluetoothModule.Open();
#endif
        }

        #region IDisposable members

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Call to GC.SupressFinalize will take this object
            // off the finalization queue and prevent multiple
            // executions.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initiate object disposal.
        /// </summary>
        /// <param name="disposing">Flag used to determine if the method is being called by the runtime (false) or programmatically (true).</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _bluetoothModule.DataReceived -= bluetoothModule_DataReceived;
                    _bluetoothModule.Close();
                    _bluetoothModule.Dispose();
                    _bluetoothModule = null;
                }
                _disposed = true;   // Done - prevent accidental or intentional Dispose calls.
            }
        }

        #endregion

        #endregion
        
        #region Configuration

        public string Port { get; set; }
        public Parity Parity { get; set; }
        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }
        public BaudRate CurrentBaud { get; set; }
        public BaudRate DesiredBaud { get; set; }
        public string DeviceName { get; set; }

        #endregion

        #region Constants

        /// <summary>
        /// Size of the buffer for the com port.
        /// </summary>
        private const int BUFFER_SIZE = 1024;

        #endregion

        #region Enums

        /// <summary>
        /// Determine which mode the module is in.
        /// </summary>
        public enum ConnectionState
        {
            /// <summary>
            /// We have not yet made a connection to the bluetooth device.
            /// </summary>
            Disconnected,
            /// <summary>
            /// Bluetooth module is in Command mode.
            /// </summary>
            Command,
            /// <summary>
            /// Bluetooth module is in data mode (default).
            /// </summary>
            Data
        }

        #endregion

        #region Private variables

        /// <summary>
        /// Serial port used to communicate with the bluetooth module.
        /// </summary>
        private SerialPort _bluetoothModule;

        /// <summary>
        /// Buffer for the data from the Bluetooth device.
        /// </summary>
        private byte[] _buffer = new byte[BUFFER_SIZE];

        /// <summary>
        /// Used to track when Dispose is called.
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region Properties

        /// <summary>
        /// Current connection type.
        /// </summary>
        public ConnectionState ConnectionStatus { get; private set; }

        public DeviceStatusCodes DeviceStatus { get; set; }

        /// <summary>
        /// to use later.. for now it's just here.
        /// </summary>
        public BluetoothDeviceConfig DeviceConfiguration { get; set; }

        #endregion

        #region public Methods

        public void Connect()
        {
            this.Setup();
        }

        /// <summary>
        /// Send a command to the bluetooth module and return the single line result
        /// from the module.
        /// </summary>
        /// <param name="command">Cammnd to send ot the module.</param>
        /// <exception cref="InvalidOperationException">This exception is thrown when the device is not in command mode.</exception>
        /// <returns>Result text from the bluetooth module.</returns>
        public void SendCommand(string command)
        {
#if XbeeSPI
            if (null == this.xbee)
                return;
#endif
            if (null == _bluetoothModule)
                return;

#if XbeeSPI
            this.xbee.Write(command);
            return;
#else
            string payload = command; // "\r\n" + command + "\r\n";
            byte[] cmd = Encoding.UTF8.GetBytes(payload);
            _bluetoothModule.Write(cmd, 0, cmd.Length);
#endif
            //Thread.Sleep(this.DeviceConfiguration.WaitBetweenSentCommands);

        }

        #region Command set

        Hashtable Responses = new System.Collections.Hashtable()
        {
            { "OKsetname", "" },
            { "OKname", "" },
            { "OKsetpin", "" }
        };

        #region SetBaudRate 

        Hashtable BaudLookup = new Hashtable()
        {
            { BaudRate.Baudrate1200, "AT+BAUD1" },
            { BaudRate.Baudrate2400, "AT+BAUD2" },
            { BaudRate.Baudrate4800, "AT+BAUD3" },
            { BaudRate.Baudrate9600, "AT+BAUD4" },
            { BaudRate.Baudrate19200, "AT+BAUD5" },
            { BaudRate.Baudrate38400, "AT+BAUD6" },
            { BaudRate.Baudrate57600, "AT+BAUD7" },
            { BaudRate.Baudrate115200, "AT+BAUD8" },
            { BaudRate.Baudrate230400, "AT+BAUD9" }
        };

        public virtual void SetBaudRate(BaudRate rate)
        {
            /*
            1---------1200 
    2---------2400 
    3---------4800 
    4---------9600 (Default) 
    5---------19200 
    6---------38400 
    7---------57600 
    8---------115200 
    9---------230400 
    A---------460800 -- unsupported
    B---------921600  -- unsupported
    C---------1382400  -- unsupported
                */
            if (_bluetoothModule.BaudRate != (int)rate)
            {
                if (BaudLookup.Contains(rate))
                {
                    var baud = BaudLookup[rate];
                    this.SendCommand(baud.ToString());
                    _bluetoothModule.Close();
                    _bluetoothModule.BaudRate = (int)rate;
                    _bluetoothModule.Open();
                }
                else
                {
                    Debug.Print("desiredBaud is out of range of supported bauds");
                }
            }

        }

        #endregion

        #region ChangeDeviceName

        public virtual void ChangeDeviceName(string name)
        {
            if (name.IsNullOrEmpty())
                return;

            this.SendCommand("AT+NAME" + name);
        }

        #endregion

        #region ChangePin
        //AT+PINxxxx

        public virtual void ChangePin(string pin)
        {
            if (pin.IsNullOrEmpty())
                return;
            this.SendCommand("AT+PIN" + pin);
        }

        #endregion

        #region Broadcast 

        /// <summary>
        /// returns a boolean state if the device was in a state that it could be put into broadcast mode. if not this returns false.
        /// </summary>
        /// <returns></returns>
        public virtual void Broadcast()
        {
            if (!this.IsSetup)
                this.Setup();
            Thread.Sleep(2000);
            this.SendCommand("+INQ=1");
            Thread.Sleep(2000);
        }

        public virtual void StopBroadcast()
        {
            if (this.DeviceStatus == DeviceStatusCodes.Inquiring)
                this.SendCommand("+INQ=0");
        }
        
        #endregion

        #endregion

        /// <summary>
        /// Send a command to the bluetooth module where a multi-line response is expected.
        /// </summary>
        /// <remarks>
        /// This method assumes that all of the responses from a command will be returned within the 
        /// specified timeout period.  Any responses after the timeout will not be returned.
        /// </remarks>
        /// <param name="command">Command to send to the bluetooth module.</param>
        /// <param name="wait">Timeout (in milliseconds) between sending the command and expecting the result.</param>
        /// <returns>ArrayList of strings containing the response from the bluetooth module.</returns>
        public void SendCommand(string command, int wait)
        {
            SendCommand(command);
            Thread.Sleep(wait);
        }

        public void Write(string message)
        {
            if(this.ConnectionStatus == ConnectionState.Data)
            {
                var bytes = UTF8Encoding.UTF8.GetBytes(message);
                _bluetoothModule.Write(bytes, 0, bytes.Length);
            }
        }

        #endregion

        #region private Methods

        private void OnStatusChanged()
        {
            var handler = this.DeviceStatusChanged;
            if (null != handler)
                handler(this, this.DeviceStatus);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Receive data from the bluetooth module and place into the data buffer.
        /// </summary>
        /// <param name="sender">Serial port generating the event.</param>
        /// <param name="e">Type of data received over the serial port.</param>
        private void bluetoothModule_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReadDataInternal(sender, e);
            }
            catch (Exception)
            {
            }
        }
        private void ReadDataInternal(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = sender as SerialPort;
            if (null == port || !port.CanRead)
                return;
            DateTime recievedAt = DateTime.Now;
            bool call = false;
            if (e.EventType == SerialData.Chars)
            {
                int amount;
                byte[] buffer = new byte[BUFFER_SIZE];

                amount = port.Read(buffer, 0, BUFFER_SIZE);
                this.StreamData.Append(Encoding.UTF8.GetChars(buffer));
                for (int i = 0; i < amount; i++)
                {
                    if (buffer[i] == '\r')
                    {
                        call = true;
                        break;
                    }
                }

                var s = this.StreamData.ToString();
                if (Responses.Contains(s))
                    this.StreamData.Clear();

                if (call)
                {
                    var result = this.StreamData.ToString().Trim('\r', '\n', 'O', 'K');

                    if (result == "END")
                        ConnectionStatus = ConnectionState.Data;
                    else if (result == "CMD")
                        ConnectionStatus = ConnectionState.Command;
                    else if (result.IndexOf("+BTSTATE:") == 0)
                    {
                        try
                        {
                            int status = int.Parse(result.ToString().Substring(9));
                            this.DeviceStatus = (DeviceStatusCodes)status;
                            this.OnStatusChanged();
                        }
                        catch { }
                    }

                    var handler = this.DataRecieved;
                    if (null != handler)
                    {
                        try
                        {
                            handler(result, recievedAt);
                        }
                        catch { }
                        this.StreamData.Clear();
                    }
                }
            }
        }

        #endregion

        #region private methods

        protected virtual void Setup()
        {
            // change baud if necessary
            this.SetBaudRate(this.DesiredBaud);

            // change name if necessary
            this.ChangeDeviceName(this.DeviceName);

            //this.SendCommand("AT+BAUD6");
            //var buffer = new byte[1024];
            // var amount = _bluetoothModule.Read(buffer, 0, buffer.Length);
            //this.SendCommand("+STWMOD=" + (int)this.DeviceConfiguration.WorkingMode);//set the bluetooth work in slave mode
            ////rslt = this.SendCommand("+STBD=KRAKER");//set the bluetooth name as "whatever"
            //this.SendCommand("+STNA=" + this.DeviceConfiguration.DeviceName);//set the bluetooth name as "whatever"
            //this.SendCommand("+STAUTO=" + (this.DeviceConfiguration.AutoConnectEnabled ? "1" : "0"));// Auto-connection enabled
            //this.SendCommand("+STOAUT=" + (this.DeviceConfiguration.AllowPairedDevicesToConnect ? "1" : "0"));// Permit Paired device to connect me
            //this.SendCommand("+STPIN=" + this.DeviceConfiguration.PinCode);//set a password for others to use to connect
            this.IsSetup = true;
            Debug.Print("Setup complete");
            Thread.Sleep(1000);
        }

        #endregion
    }
}
