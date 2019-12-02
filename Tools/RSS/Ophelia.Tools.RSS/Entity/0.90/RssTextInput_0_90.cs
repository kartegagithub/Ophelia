using System.Xml.Linq;

namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class RssTextInput_0_90 : RssTextInput
    {
        /// <summary>
        /// 
        /// </summary>
        public RssTextInput_0_90()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssTextInput_0_90(XElement element)
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