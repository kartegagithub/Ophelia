using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Fieldset : Container
	{

		public Fieldset()
		{
		}
		public Fieldset(string ID)
		{
			this.ID = ID;
		}
		public Fieldset(string MemberName, string Legend)
		{
			this.ID = MemberName.Replace(".", "_");
			this.Legend = Legend;
		}

		private string _legend;
		public string Legend {
			get { return _legend; }
			set { _legend = value; }
		}

		public override void OnBeforeDraw(Content Content)
		{
			Content.Add("<fieldset");

			if (!string.IsNullOrEmpty(this.ID))
				Content.Add(string.Format(" id=\"{0}\"", this.ID));

			if (this.oAttributes != null) {
				for (int i = 0; i <= this.Attributes.Count - 1; i++) {
					Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
				}
			}

			this.DrawEvents(Content);
			Content.Add(this.Style.Draw + ">");

			if (!string.IsNullOrEmpty(this.Legend)) {
				Content.Add("<legend>").Add(this.Legend).Add("</legend>");
			}

			Content.Add(this.Content.Value);
			base.DrawControls(Content);
			Content.Add("</fieldset>");

		}
	}

}
