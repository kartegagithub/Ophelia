using System.Xml.Linq;

namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class RssImage_0_92 : RssImage
    {
        /// <summary>
        /// 
        /// </summary>
        public RssImage_0_92() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssImage_0_92(XElement element)
            : base(element)
        {
            if (element != null)
            {
                Parse(element);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected new void Parse(XElement element)
        {
            
        }
    }
}