using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class CheckBoxOptionCollection : Ophelia.Application.Base.CollectionBase
	{
		private CheckBoxList oCheckBoxList;
		private Style oCheckBoxOptionStyle;
		public new CheckBoxOption this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public Style OptionStyle {
			get {
				if (this.oCheckBoxOptionStyle == null)
					this.oCheckBoxOptionStyle = new Style();
				return this.oCheckBoxOptionStyle;
			}
		}
		public CheckBoxList CheckBoxList {
			get { return this.oCheckBoxList; }
		}
		public new CheckBoxOption Add(string Value, string Text)
		{
			CheckBoxOption CheckBoxOption = new CheckBoxOption(this);
			CheckBoxOption.Value = Value;
			CheckBoxOption.Text = Text;
			CheckBoxOption.Label.SetStyle(this.OptionStyle);
			this.List.Add(CheckBoxOption);
			return CheckBoxOption;
		}
		public string SelectedOptions {
			get {
				string StringValue = "";
				for (int i = 0; i <= this.Count - 1; i++) {
					if (!string.IsNullOrEmpty(StringValue)) {
						StringValue += ",";
					}
					StringValue += this[i].Value;
				}
				return StringValue;
			}
		}
		public string Draw()
		{
			string sReturnString = "";
			int n = 0;
			for (n = 0; n <= this.Count - 1; n++) {
				sReturnString += this[n].Draw;
				if (n != this.Count - 1 && this.CheckBoxList.Direction == View.Web.Controls.CheckBoxList.CheckBoxDirection.Vertical) {
					sReturnString += "<br>";
				}
			}
			return sReturnString;
		}
		public CheckBoxOptionCollection(CheckBoxList CheckBoxList)
		{
			this.oCheckBoxList = CheckBoxList;
		}
	}
}
