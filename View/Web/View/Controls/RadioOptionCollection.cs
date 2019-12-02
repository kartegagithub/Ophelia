using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class RadioOptionCollection : Ophelia.Application.Base.CollectionBase
	{
		private RadioButton oRadioButton;
		private Style oOptionStyle;
		private string sSelectedValue = "";
		public new RadioOption this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public string SelectedValue {
			get { return this.sSelectedValue; }
			set { this.sSelectedValue = value; }
		}
		public Style OptionStyle {
			get {
				if (this.oOptionStyle == null)
					this.oOptionStyle = new Style();
				return this.oOptionStyle;
			}
		}
		public RadioButton RadioButton {
			get { return this.oRadioButton; }
		}
		public new RadioOption Add(string Value, string Text)
		{
			RadioOption RadioOption = new RadioOption(this);
			RadioOption.Value = Value;
			RadioOption.Text = Text;
			RadioOption.Label.SetStyle(this.OptionStyle);
			this.List.Add(RadioOption);
			return RadioOption;
		}
		public RadioOption SelectedOption {
			get {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this.SelectedValue == this[i].Value) {
						return this[i];
					}
				}
				return null;
			}
		}
		public string Draw()
		{
			string sReturnString = "";
			int n = 0;
			for (n = 0; n <= this.Count - 1; n++) {
				sReturnString += this[n].Draw;
				if (n != this.Count - 1 && this.RadioButton.Direction == RadioDirection.Vertical) {
					sReturnString += "<br>";
				}
			}
			return sReturnString;
		}
		public RadioOptionCollection(RadioButton RadioButton)
		{
			this.oRadioButton = RadioButton;
		}
	}
}
