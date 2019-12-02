using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ophelia.Tools.RSS
{
    public interface IRssParser
    {
        RssFeed Parse(String xml);
    }
}
