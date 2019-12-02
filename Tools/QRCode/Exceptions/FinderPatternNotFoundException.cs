using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class FinderPatternNotFoundException : System.Exception
    {
        internal String sMessage = null;
        public override String Message
        {
            get
            {
                return sMessage;
            }
        }
        public FinderPatternNotFoundException(String message)
        {
            this.sMessage = message;
        }
    }
}