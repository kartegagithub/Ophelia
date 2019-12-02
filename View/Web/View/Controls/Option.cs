using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Option
	{
		private OptionCollection oCollection;
		private string sText = "";
		private string sValue = "";
		private Style oStyle;
		private Hashtable oAttributes;
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
				}
				return this.oStyle;
			}
		}
		public Hashtable Attributes {
			get {
				if (this.oAttributes == null) {
					this.oAttributes = new Hashtable();
				}
				return this.oAttributes;
			}
		}
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public string Value {
			get { return this.sValue; }
			set { this.sValue = value; }
		}
		public OptionCollection Collection {
			get { return this.oCollection; }
		}
		internal void Draw(Ophelia.Web.View.Content Content)
		{
			Content.Add("<option " + this.Style.Draw + " value=\"" + this.Value + "\"");
			if (this.oAttributes != null) {
				for (int i = 0; i <= this.oAttributes.Count - 1; i++) {
					Content.Add(" " + this.oAttributes.Keys(i).ToString + "=\"" + this.oAttributes.Values(i).ToString + "\"");
				}
			}
			if (this.Collection.SelectedValue == this.Value) {
				Content.Add(" selected");
			}
			Content.Add(">" + this.Text);
			Content.Add("</option>");
		}
		public Option(OptionCollection Collection)
		{
			this.oCollection = Collection;
		}
	}
}
