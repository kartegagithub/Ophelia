using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class InvalidVersionException : VersionInformationException
    {
        internal String sMessage;
        public override String Message
        {
            get
            {
                return sMessage;
            }
        }

        public InvalidVersionException(String message)
        {
            this.sMessage = message;
        }
    }
}