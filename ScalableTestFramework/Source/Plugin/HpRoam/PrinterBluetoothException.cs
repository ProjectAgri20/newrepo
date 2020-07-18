using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.HpRoam
{
    [Serializable]
    internal class PrinterBluetoothException : Exception
    {
        public PrinterBluetoothException()
        {
        }

        public PrinterBluetoothException(string message) 
            : base(message)
        {
        }

        public PrinterBluetoothException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected PrinterBluetoothException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
