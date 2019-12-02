using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia
{
    public enum ApplicationEnvironment : int
    {
        Local = 0,
        Demo = 1,
        Test = 2,
        Stage = 3,
        Slave = 4,
        Alfa = 5,
        Beta = 6,
        Load = 7,
        Live = 8
    }
}
