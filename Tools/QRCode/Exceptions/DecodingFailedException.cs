using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class DecodingFailedException : System.ArgumentException
    {
        internal String sMessage = null;

        public override String Message
        {
            get
            {
                return sMessage;
            }
        }

        public DecodingFailedException(String message)
        {
            this.sMessage = message;
        }
    }
}