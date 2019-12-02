﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Ophelia.Tools.RSS.Internal;

namespace Ophelia.Tools.RSS
{
    public class Rss_0_91_Parser : RssParser
    {
        public override RssVersion Version
        {
            get { return RssVersion.Rss_0_91; }
        }
        public override RssChannel CreateRssChannel(XElement element)
        {
            return new RssChannel_0_91(element);
        }
        public override RssImage CreateRssImage(XElement element)
        {
            return new RssImage_0_91(element);
        }
        public override RssItem CreateRssItem(XElement element)
        {
            return new RssItem_0_91(element);
        }
        public override RssTextInput CreateRssTextInput(XElement element)
        {
            return new RssTextInput_0_91(element);
        }
    }
}
