using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Diagnostics
{
    public interface ILogger
    {
        void Information(string message);
        void Error(Exception ex);
        void Exclamation(string message);
    }
}
