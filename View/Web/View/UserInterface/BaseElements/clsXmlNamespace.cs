using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class XmlNamespace
	{
		private string sNamespace = "";
		private string sTitle = "";
		public string Namespace {
			get { return this.sNamespace; }
			set { this.sNamespace = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public bool IsEqualTo(XmlNamespace XmlNamespace)
		{
			if (this.Namespace != XmlNamespace.Namespace)
				return false;
			if (this.Title != XmlNamespace.Title)
				return false;
			return true;
		}
		public string Draw()
		{
			Ophelia.Web.View.Content Content = new Ophelia.Web.View.Content();
			if (!string.IsNullOrEmpty(this.Namespace)) {
				Content.Add(" xmlns");
				if (!string.IsNullOrEmpty(this.Title)) {
					Content.Add(":" + this.Title);
				}
				Content.Add("=");
				Content.Add("\"" + this.Namespace + "\" ");
			}
			return Content.Value;
		}
	}
}
