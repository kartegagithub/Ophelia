﻿using System.Xml.Linq;


namespace Ophelia.Tools.RSS
{
    /// <summary>
    /// 
    /// </summary>
    public class RssGuid
    {
        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsPermaLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RssGuid()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssGuid(XElement element)
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
        protected void Parse(XElement element)
        {
            ID = element.Value;
            IsPermaLink = element.CastAttributeToString("isPermaLink").ToBoolean();
        }
    }
}