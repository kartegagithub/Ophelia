using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class HeadScript
	{

		private string sSource;

		private bool bAddScriptTag;
		public string Source {
			get { return this.sSource; }
			set { this.sSource = value; }
		}

		public bool AddScriptTag {
			get { return this.bAddScriptTag; }
			set { this.bAddScriptTag = value; }
		}

		public string Draw()
		{
			Content Content = new Content();
			if (this.AddScriptTag) {
				Content.Add("<script");
				if (this.Source != string.Empty) {
					Content.Add(" type='text/javascript' src='" + this.Source + "' >");
				}
				Content.Add(" </script>");
			} else {
				if (this.Source != string.Empty)
					Content.Add(this.Source);
			}
			return Content.Value;
		}

		public bool IsEqualTo(HeadScript HeadScript)
		{
			if (this.Source != HeadScript.Source)
				return false;
			return true;
		}

		public HeadScript(string Source, bool AddScriptTag)
		{
			this.Source = Source;
			this.AddScriptTag = AddScriptTag;
		}


		public HeadScript()
		{
		}

	}
}
