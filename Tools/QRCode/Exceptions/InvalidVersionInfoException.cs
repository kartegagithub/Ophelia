using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class InvalidVersionInfoException : VersionInformationException
    {
        internal String sMessage = null;
        public override String Message
        {
            get
            {
                return sMessage;
            }
        }

        public InvalidVersionInfoException(String message)
        {
            this.sMessage = message;
        }
    }
}