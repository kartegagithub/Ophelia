using System;
namespace Ophelia.Tools.QRCode.Exceptions
{
    [Serializable]
    public class SymbolNotFoundException : System.ArgumentException
    {
        internal String sMessage = null;

        public override String Message
        {
            get
            {
                return sMessage;
            }
        }

        public SymbolNotFoundException(String message)
        {
            this.sMessage = message;
        }
    }
}