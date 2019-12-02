using System.Xml.Linq;

namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class RssChannel_0_90 : RssChannel
    {
        /// <summary>
        /// 
        /// </summary>
        public RssChannel_0_90()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssChannel_0_90(XElement element)
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
        protected new void Parse(XElement element)
        {
            
        }
    }
}