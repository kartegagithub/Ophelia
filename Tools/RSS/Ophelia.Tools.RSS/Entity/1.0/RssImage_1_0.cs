using System.Xml.Linq;


namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class RssImage_1_0 : RssImage_0_90
    {
        /// <summary>
        /// 
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RssImage_1_0()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssImage_1_0(XElement element)
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
            About = element.CastAttributeToString("rdf", "about");
        }
    }
}