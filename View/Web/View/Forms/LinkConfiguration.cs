using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class LinkConfiguration : FontConfiguration
	{
		private FontConfiguration oHover;
		private FontConfiguration oVisited;
		public FontConfiguration Hover {
			get { return this.oHover; }
		}
		public FontConfiguration Visited {
			get { return this.oVisited; }
		}
		public LinkConfiguration()
		{
			this.oHover = new FontConfiguration();
			this.oVisited = new FontConfiguration();
			this.IsLink = true;
		}
	}
}
