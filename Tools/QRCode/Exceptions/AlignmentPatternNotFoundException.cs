using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class AlignmentPatternNotFoundException : System.ArgumentException
    {
        internal String sMessage = null;

        public override String Message
        {
            get
            {
                return sMessage;
            }
        }
        public AlignmentPatternNotFoundException(String message)
        {
            this.sMessage = message;
        }
    }
}