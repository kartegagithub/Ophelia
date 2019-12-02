using System.Xml.Linq;
using System.Collections.Generic;

namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RssXmlObject : Dictionary<string, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlns"></param>
        /// <returns></returns>
        public abstract XElement CreateElement(XNamespace xmlns);
    }
}