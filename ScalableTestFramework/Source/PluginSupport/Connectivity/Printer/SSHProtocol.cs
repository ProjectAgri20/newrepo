using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Routrek.SSHC;


namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{

    /// <summary>
    /// Base class for SSH client class
    /// </summary>
    public class SSHReader : ISSHConnectionEventReceiver, ISSHChannelEventReceiver
    {
        private SSHConnection _connection;

        /// <summary>
        /// Variable to check if the channel is ready
        /// </summary>
        public bool _ready;

        /// <summary>
        /// Event to notify when data is passed
        /// </summary>
        public event RcvdData notify;

        private SSHChannel _channel;
        /// <summary>
        /// String that will capture the string received from the channel
        /// </summary>
        public string rcvdText;

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// Get set property for connection
        /// </summary>
        public SSHConnection connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// Get set property for the channel
        /// </summary>
        public SSHChannel Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                _channel = value;
            }
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// Method to call the event each time data flows from the channel
        /// </summary>
        public void OnData(byte[] data, int offset, int length)
        {
            rcvdText = System.Text.Encoding.ASCII.GetString(data, offset, length);
            notify(this, rcvdText);
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnDebugMessage(bool always_display, byte[] data)
        {
            //Framework.Logger.LogInfo("DEBUG: " + Encoding.ASCII.GetString(data));
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnIgnoreMessage(byte[] data)
        {
            //Framework.Logger.LogInfo("Ignore: " + Encoding.ASCII.GetString(data));
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnAuthenticationPrompt(string[] msg)
        {
            //Framework.Logger.LogInfo("Auth Prompt " + msg[0]);            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnError(Exception error, string msg)
        {
            //Framework.Logger.LogError("ERROR: " + msg);            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// Method to disconnect the connection
        /// </summary>
        public void OnChannelClosed()
        {
            //Framework.Logger.LogInfo("Channel closed");
            _connection.Disconnect("");
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// Method to close the channel
        /// </summary>
        public void OnChannelEOF()
        {
            _channel.Close();
            //Framework.Logger.LogInfo("Channel EOF");            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnExtendedData(int type, byte[] data)
        {
            //Framework.Logger.LogInfo("EXTENDED DATA");
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnConnectionClosed()
        {
            //Framework.Logger.LogInfo("Connection closed");            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnUnknownMessage(byte type, byte[] data)
        {
            //Framework.Logger.LogInfo("Unknown Message " + type);            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnChannelReady()
        {
            _ready = true;
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnChannelError(Exception error, string msg)
        {
            //Framework.Logger.LogInfo("Channel ERROR: " + msg);            
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void OnMiscPacket(byte type, byte[] data, int offset, int length)
        {

        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public PortForwardingCheckResult CheckPortForwardingRequest(string host, int port, string originator_host, int originator_port)
        {
            PortForwardingCheckResult r = new PortForwardingCheckResult();
            r.allowed = true;
            r.channel = this;
            return r;
        }

        /// <summary>
        /// Implementing the interface members as the class implements from two interfaces
        /// </summary>
        public void EstablishPortforwarding(ISSHChannelEventReceiver rec, SSHChannel channel)
        {
            _channel = channel;
        }
    }

    /// <summary>
    /// SSH Client class which connects to the Wabis machine
    /// </summary>
    public class SSHProtocol : SSHReader
    {
        /// <summary>
        /// Variable for the Wabis machine name
        /// </summary>
        private IPAddress _ipAddress;
        private static string escapeCharsPattern = "\\[[0-9;?]*[^0-9;]";
        private static SSHProtocol instance = null;

        SSHReader reader = new SSHReader();
        SSHConnection _conn;
        SSHConnectionParameter connParameter = new SSHConnectionParameter();
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
        string finalresult;



        /// <summary>
        /// Will check if the Wabis machine is connected or not
        /// </summary>
        /// <returns>true if connected else false</returns>
        public bool isConnected
        {
            get { return _conn.Available; }

        }

        /// <summary>
        /// Constructor for SSH Client
        /// </summary>
        /// <param name="username">Username of the Wabis machine</param>
        /// <param name="password">Password of the Wabis machine</param>
        /// <param name="ipAddress">Wabis machine name</param>
        public SSHProtocol(string username, string password, IPAddress ipAddress)
        {
            connParameter.UserName = username;
            connParameter.Password = password;
            _ipAddress = ipAddress;
        }

        public string Receive(string data1, string data2)
        {
            throw new NotImplementedException();
        }
        public object Get(string oid, ref string errorStatus)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Singleton instance of the SSHClient class.
        ///</summary>
        public static SSHProtocol Instance
        {
            get
            {
                if (instance == null)
                {
                    //Framework.Logger.LogInfo("Instance is null");
                }
                return instance;
            }
        }

        /// <summary>
        /// Method to connect to the Wabis machine
        /// </summary>
        /// <returns>true if connected else false</returns>
        public bool Connect()
        {
            connParameter.Protocol = Routrek.SSHC.SSHProtocol.SSH2;
            connParameter.AuthenticationType = AuthenticationType.Password;
            connParameter.WindowSize = 0x1000;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            try
            {
                s.Connect(new IPEndPoint(_ipAddress, 22));
            }
            catch
            {
                s.Close();
                return false;
            }

            try
            {
                _conn = SSHConnection.Connect(connParameter, reader, s);
                reader.connection = _conn;
                Routrek.SSHC.SSHChannel ch = _conn.OpenShell(reader);
                reader.Channel = ch;
                SSHConnectionInfo ci = _conn.ConnectionInfo;
                reader.notify += new RcvdData(PrintResult);
                if (_conn.AuthenticationResult.ToString() == "Success")
                    return true;
                else
                    return false;
            }
            catch (Exception connectionException)
            {
                Framework.Logger.LogError("Fail to connect to SSH Server, error: " + connectionException.Message);
                throw connectionException;
            }


        }

        /// <summary>
        /// Method to disconnect from the Wabis machine
        /// </summary>
        /// <returns>true if disconnected otherwise false</returns>
        public bool Disconnect()
        {
            try
            {
                reader.OnChannelClosed();
                return true;
            }
            catch (Exception disconnectException)
            {
                Framework.Logger.LogError("Error while disconnecting the server, error: " + disconnectException.Message);
                return false;
            }
        }

        /// <summary>
        /// Method that will send the command through the channel
        /// </summary>
        /// <param name="command">command</param> 
        /// <returns>returns the response</returns>
        public string Send(string command)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            reader.Channel.Transmit(Encoding.ASCII.GetBytes(command));
            Thread.Sleep(TimeSpan.FromSeconds(5));
            return reader.rcvdText;
        }

        /// <summary>
        /// Method that will remove escape sequence characters from the input string.
        /// </summary>
        /// <param name="rawData">data that has to be formatted</param> 
        /// <returns>returns the formatted string</returns>
        public static string ClearTerminalCharacters(string rawData)
        {
            rawData = rawData.Replace("(B)0", "");
            rawData = Regex.Replace(rawData, escapeCharsPattern, "");
            rawData = rawData.Replace(((char)15).ToString(), "");
            rawData = Regex.Replace(rawData, ((char)27) + "=*", "");
            return rawData;
        }

        /// <summary>
        /// Method that will clear the special characters from the input string.
        /// </summary>
        /// <param name="rawData">data that has to be formatted</param> 
        /// <param name="cmd">data that has to be formatted</param> 
        /// <returns>returns the formatted string</returns>
        public string ClearSpecialCharacters(string rawData, string cmd)
        {
            int startPos = rawData.IndexOf(cmd) + cmd.Length;
            int endPos = rawData.IndexOf("~ [ NONE ]", startPos, StringComparison.CurrentCultureIgnoreCase);
            if (endPos != -1)
                return rawData.Substring(startPos, endPos - startPos);
            else
                return rawData.Substring(startPos);
        }


        /// <summary>
        /// This method will be fired whenever a data goes through the channel
        /// </summary>
        public void PrintResult(object sender, string rawData)
        {
            string formattedresponse = ClearTerminalCharacters(rawData);
            System.Console.Write(formattedresponse);
            if (formattedresponse.Contains("with value:"))
            {
                string strResult = GetResponse(formattedresponse);
            }
        }

        /// <summary>
        /// This method parses the input string to know the value for PMLGet and PMLSet
        /// </summary>
        /// <param name="formattedResponse">data that is recieved from PMLGet or PMLSet</param>
        /// <returns>returns the value that is required by PMLGet and PMLSet</returns>
        public string GetResponse(string formattedResponse)
        {
            string[] str = formattedResponse.Split(':');
            string parsedresponse = str[1];
            finalresult = parsedresponse.Trim();
            return finalresult;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }

    /// <summary>
    /// Delegate for having control over the data that is returned back from the channel
    /// </summary>
    public delegate void RcvdData(object sender, String data);

}