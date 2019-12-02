﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Ophelia.Tools.RSS.Internal;

namespace Ophelia.Tools.RSS
{
    public class RssTextInput_Atom : RssTextInput
    {
        /// <summary>
        /// 
        /// </summary>
        public RssTextInput_Atom() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public RssTextInput_Atom(XElement element)
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
