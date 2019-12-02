using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Tools.QRCode.Codec.Reader
{
    internal class DecodeResult
    {
        internal int errorNumber;
        internal bool correctionSucceeded;
        internal sbyte[] decodedBytes;
        private QRCodeDecoder enclosingInstance;

        virtual public sbyte[] DecodedBytes
        {
            get
            {
                return this.decodedBytes;
            }

        }
        virtual public int ErrorNumber
        {
            get
            {
                return this.errorNumber;
            }

        }
        virtual public bool CorrectionSucceeded
        {
            get
            {
                return this.correctionSucceeded;
            }

        }
        public QRCodeDecoder Enclosing_Instance
        {
            get
            {
                return this.enclosingInstance;
            }

        }
        public DecodeResult(QRCodeDecoder enclosingInstance, sbyte[] decodedBytes, int numErrors, bool correctionSucceeded)
        {
            InitBlock(enclosingInstance);
            this.decodedBytes = decodedBytes;
            this.errorNumber = numErrors;
            this.correctionSucceeded = correctionSucceeded;
        }

        private void InitBlock(QRCodeDecoder enclosingInstance)
        {
            this.enclosingInstance = enclosingInstance;
        }
    }
}
