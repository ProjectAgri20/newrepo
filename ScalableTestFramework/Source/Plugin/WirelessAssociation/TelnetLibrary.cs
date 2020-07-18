using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.WirelessAssociation
{
    public class TelnetLibrary : IDisposable
    {
        private enum Verbs
        {
            WILL = 251,
            WONT = 252,
            DO = 253,
            DONT = 254,
            IAC = 255
        }

        private enum Options
        {
            SGA = 3
        }

        private TcpClient _tcpSocket;

        public TelnetLibrary(string hostName, int port)
        {
            _tcpSocket = new TcpClient(hostName, port);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_tcpSocket.Connected)
                {
                    _tcpSocket.Close();
                    _tcpSocket = null;
                }
            }
        }

        /// <summary>
        /// Executes the command which sends as parameter
        /// </summary>
        /// <param name="cmd">command to execute</param>
        public void WriteLine(string cmd)
        {
            Write(cmd + Environment.NewLine);
        }

        /// <summary>
        /// Executes the command which sends as parameter
        /// </summary>
        /// <param name="cmd">command to execute</param>
        public void Write(string cmd)
        {
            if (_tcpSocket.Connected && !string.IsNullOrEmpty(cmd))
            {
                byte[] buf = ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
                _tcpSocket.GetStream().Write(buf, 0, buf.Length);
            }
        }

        /// <summary>
        /// Read the output after telnet command is executed
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            if (!_tcpSocket.Connected)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            do
            {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
            } while (_tcpSocket.Available > 0);

            return sb.ToString();
        }

        /// <summary>
        /// Check whether telnet connection is created or not
        /// </summary>
        public bool IsConnected
        {
            get { return _tcpSocket.Connected; }
        }

        /// <summary>
        /// Enables Telnet feature in Operating System
        /// </summary>
        public static void EnableTelnetFeature()
        {
            ProcessUtil.Execute("dism.exe", "/online /enable-feature /featurename:TelnetClient /NoRestart", TimeSpan.FromMinutes(5));
            Thread.Sleep(TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Disables Telnet feature in Operating System
        /// </summary>
        public static void DisableTelnetFeature()
        {
            ProcessUtil.Execute("dism.exe", "/online /disable-feature /featurename:TelnetClient /NoRestart", TimeSpan.FromMinutes(3));
            Thread.Sleep(TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Reads the output string
        /// </summary>
        /// <param name="sb"></param>
        private void ParseTelnet(StringBuilder sb)
        {
            while (_tcpSocket.Available > 0)
            {
                int input = _tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;

                    case (int)Verbs.IAC:
                        {
                            // interpret as command
                            int inputverb = _tcpSocket.GetStream().ReadByte();

                            switch (inputverb)
                            {
                                case -1:
                                    break;

                                case (int)Verbs.IAC:
                                    {
                                        //literal IAC = 255 escaped, so append char 255 to string
                                        sb.Append(inputverb);
                                    }
                                    break;

                                case (int)Verbs.DO:
                                case (int)Verbs.DONT:
                                case (int)Verbs.WILL:
                                case (int)Verbs.WONT:
                                    {
                                        // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                        int inputoption = _tcpSocket.GetStream().ReadByte();
                                        if (inputoption == -1)
                                        {
                                            break;
                                        }
                                        _tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                        if (inputoption == (int)Options.SGA)
                                        {
                                            _tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                        }
                                        else
                                        {
                                            _tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                        }
                                        _tcpSocket.GetStream().WriteByte((byte)inputoption);
                                    }
                                    break;
                            }
                        }
                        break;

                    default:
                        {
                            sb.Append((char)input);
                        }
                        break;
                }
            }
        }
    }
}