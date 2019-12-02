using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class OptionCollection : Ophelia.Application.Base.CollectionBase
	{
		private SelectBox oSelectBox;
		private string sSelectedValue = "";
		public string SelectedValue {
			get { return this.sSelectedValue; }
			set { this.sSelectedValue = value; }
		}
		public SelectBox SelectBox {
			get { return this.oSelectBox; }
		}
		public Option FirstOption {
			get {
				if (this.Count > 0) {
					return this(0);
				}
				return null;
			}
		}
		public Option LastOption {
			get {
				if (this.Count > 0) {
					return this(this.Count - 1);
				}
				return null;
			}
		}
		public new Option Add(Option Option)
		{
			this.List.Add(Option);
			return Option;
		}
		public new Option Add(string Value, string Text)
		{
			Option Option = new Option(this);
			Option.Value = Value;
			Option.Text = Text;
			this.List.Add(Option);
			return Option;
		}
		public new Option Add(string Value)
		{
			Option Option = new Option(this);
			Option.Value = Value;
			Option.Text = Value;
			this.List.Add(Option);
			return Option;
		}
		public Option SelectedOption {
			get {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this.SelectedValue == this(i).Value) {
						return this(i);
					}
				}
				return null;
			}
		}
		public string Draw()
		{
			Content Content = new Content();
			Option Option = default(Option);
			if (this.SelectBox.CreateBlankOption) {
				Content.Add("<option value=\"\"></option>");
			} else if (!string.IsNullOrEmpty(this.SelectBox.Message)) {
				Content.Add("<option value=\"0\">" + this.SelectBox.Message + "</option>");
			}
			foreach ( Option in this) {
				Option.Draw(Content);
			}
			return Content.Value;
		}
		public OptionCollection(SelectBox SelectBox)
		{
			this.oSelectBox = SelectBox;
		}
	}
}
