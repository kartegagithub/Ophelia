using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class InvalidDataBlockException : System.ArgumentException
    {
        internal String sMessage = null;

        public override String Message
        {
            get
            {
                return sMessage;
            }
        }

        public InvalidDataBlockException(String message)
        {
            this.sMessage = message;
        }
    }
}