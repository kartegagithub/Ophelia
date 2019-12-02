using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Tools.QRCode.Codec.Reader
{
    internal class ModulePitch
    {
        public int Top;
        public int Left;
        public int Bottom;
        public int Right;

        private QRCodeImageReader enclosingInstance;
        public QRCodeImageReader EnclosingInstance
        {
            get
            {
                return this.enclosingInstance;
            }
        }
        public ModulePitch(QRCodeImageReader enclosingInstance)
        {
            this.InitBlock(enclosingInstance);
        }
        private void InitBlock(QRCodeImageReader enclosingInstance)
        {
            this.enclosingInstance = enclosingInstance;
        }
    }
}
