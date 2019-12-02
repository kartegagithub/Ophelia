using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class ComboBox : InputDataControl
	{
		private string sSelectIcon = "";
		private string sOptionsBackgroundColor = "";
		private string sOptionsBorderColor = "";
		private string sOnOpenEvent = "";
		private string sOnCloseEvent = "";
		private string sBlankOptionText = "";
		private bool bShowBlankOption = true;
		private ArrayList oItems = new ArrayList();
		private ArrayList oDataSource = new ArrayList();
		private object oSelectedItem = null;
		private int nSelectedIndex = -1;
		public object SelectedItem {
			get { return this.oSelectedItem; }
			set { this.oSelectedItem = value; }
		}
		public int SelectedIndex {
			get { return this.nSelectedIndex; }
			set { this.nSelectedIndex = value; }
		}
		public ArrayList Items {
			get { return this.oItems; }
		}
		public ArrayList DataSource {
			get { return this.oDataSource; }
			set { this.oDataSource = value; }
		}
		public string ShowBlankOption {
			get { return this.bShowBlankOption; }
			set { this.bShowBlankOption = value; }
		}
		public string BlankOptionText {
			get { return this.sBlankOptionText; }
			set { this.sBlankOptionText = value; }
		}
		public string SelectIcon {
			get { return this.sSelectIcon; }
			set { this.sSelectIcon = value; }
		}
		public string OnOpenEvent {
			get { return this.sOnOpenEvent; }
			set { this.sOnOpenEvent = value; }
		}
		public string OnCloseEvent {
			get { return this.sOnCloseEvent; }
			set { this.sOnCloseEvent = value; }
		}
		public string OptionsBackgroundColor {
			get { return this.sOptionsBackgroundColor; }
			set { this.sOptionsBackgroundColor = value; }
		}
		public string OptionsBorderColor {
			get { return this.sOptionsBorderColor; }
			set { this.sOptionsBorderColor = value; }
		}
		private void SetDefaultStyle(Content Content)
		{
			Content.Add("<style>.sbHolder{background-color: #" + this.Style.BackgroundColor + "; " + (this.Style.Font.Size > -1 ? "font-size: " + this.Style.Font.Size + this.Style.Font.GetFontUnit + " " : "") + "height: 31px; position: relative; width: " + this.Style.Width + "px;" + !string.IsNullOrEmpty(this.Style.BackgroundImage) ? "background: url(" + this.Style.BackgroundImage + ") 0 0 no-repeat;" : "" + "}" + ".sbSelector{display: block; height: 30px; left: 0; line-height: 30px; outline: none; overflow: hidden; position: absolute; text-indent: 10px; top: 0; width: 170px;}" + ".sbSelector:link, .sbSelector:visited, .sbSelector:hover{ " + (!string.IsNullOrEmpty(this.Style.Font.Color) ? "color: #" + this.Style.Font.Color + ";" : "") + " outline: none; text-decoration: none;}" + ".sbToggle{" + (!string.IsNullOrEmpty(this.SelectIcon) ? "background: url(" + this.SelectIcon + ") 0 -116px no-repeat;" : "") + "display: block; height: 30px; outline: none; position: absolute; right: 0; top: 0; width: 30px;}" + ".sbToggle:hover{" + (!string.IsNullOrEmpty(this.SelectIcon) ? "background: url(" + this.SelectIcon + ") 0 -167px no-repeat;" : "") + "}" + ".sbToggleOpen{" + (!string.IsNullOrEmpty(this.SelectIcon) ? "background: url(" + this.SelectIcon + ") 0 -16px no-repeat;" : "") + "}" + ".sbToggleOpen:hover{ " + (!string.IsNullOrEmpty(this.SelectIcon) ? "background: url(" + this.SelectIcon + ") 0 -66px no-repeat;" : "") + "}" + ".sbHolderDisabled{ background-color: #3C3C3C;}" + ".sbHolderDisabled .sbHolder{}" + ".sbHolderDisabled .sbToggle{}" + ".sbOptions{ background-color: #" + this.OptionsBackgroundColor + "; " + (!string.IsNullOrEmpty(this.OptionsBorderColor) ? "border: solid 1px #" + this.OptionsBorderColor + ";" : "") + " list-style: none; left: -1px; margin: 0; padding: 0; position: absolute; top: 30px; width: " + this.Style.Width + "px; z-index: 1; overflow-y: auto;}" + ".sbOptions li{ padding: 0 7px;}" + ".sbOptions a{ " + (!string.IsNullOrEmpty(this.OptionsBorderColor) ? "border-bottom: dotted 1px #" + this.OptionsBorderColor + ";" : "") + " display: block; outline: none; padding: 7px 0 7px 3px;}" + ".sbOptions a:link, .sbOptions a:visited{ " + (!string.IsNullOrEmpty(this.Style.Font.Color) ? "color: #" + this.Style.Font.Color + ";" : "") + " text-decoration: none;}" + ".sbOptions a:hover{ " + (!string.IsNullOrEmpty(this.Style.Font.Color) ? "color: #" + this.Style.Font.Color + ";" : "") + "}" + ".sbOptions li.last a{ border-bottom: none;}</style>");
		}
		public ComboBox(string Name)
		{
			this.ID = Name;
		}
		public ComboboxItem AddItem(string Text, string Value)
		{
			ComboboxItem Item = new ComboboxItem(Text, Value);
			this.DataSource.Add(Item);
			return Item;
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.SetDefaultStyle(Content);
			string ApplicationPath = "/";
			if (this.Page != null) {
				ApplicationPath = this.Page.ContentManager.ApplicationPath;
			}

			this.Page.Header.ScriptManager.Add("jquery161min", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jquery161min.js", this.Page.FileUsageType));
			this.Page.Header.ScriptManager.Add("jqueryselectbox", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryselectbox.js", this.Page.FileUsageType));

			string SelectedValue = "";
			if (string.IsNullOrEmpty(this.ID))
				this.ID = "SelectboxControl";
			Content.Add("<select");
			Content.Add(" id='" + this.ID + "'");
			Content.Add(" name='" + this.ID + "'>");
			if (this.ShowBlankOption)
				Content.Add("<option value='' " + (this.SelectedIndex == -1 || this.SelectedItem == null ? "selected" : "") + ">" + this.BlankOptionText + "</option>");
			bool Selected = false;
			if (this.Items.Count > 0) {
				for (int i = 0; i <= this.Items.Count - 1; i++) {
					Selected = false;
					if (i == this.SelectedIndex || Convert.ToString(this.Items[i]) == Convert.ToString(this.SelectedItem)) {
						Selected = true;
						SelectedValue = i;
					}
					Content.Add("<option value='" + i + "' " + (Selected ? "selected" : "") + ">" + this.Items[i] + "</option>");
				}
			} else {
				foreach (ComboboxItem Item in this.DataSource) {
					Selected = false;
					if (Convert.ToString(Item.Value) == Convert.ToString(this.SelectedItem)) {
						Selected = true;
						SelectedValue = Item.Value;
					}
					Content.Add("<option value='" + Item.Value + "' " + (Selected ? "selected" : "") + ">" + Item.Text + "</option>");
				}
			}
			Content.Add("<input type='hidden' name='" + this.ID + "_Value' id='" + this.ID + "_Value' value='" + SelectedValue + "'>");
			Content.Add("</select>");
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			if (!this.ReadOnly) {
				Script.AppendLine("$(document).ready(function() {");
				Script.AppendLine("$('#" + this.ID + "').selectbox({" + "onOpen: function (inst) {" + this.OnOpenEvent + "}," + "onClose: function (inst) {" + this.OnCloseEvent + "}," + "onChange: function (val, inst) {$('#" + this.ID + "_Value').val(val); " + this.OnChangeEvent + "}," + "effect: 'slide'" + "})");
				Script.AppendLine("});");
			}
		}
	}
	public class ComboboxItem
	{
		private string sText = 0;
		private string sValue = "";
		public string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public string Value {
			get { return this.sValue; }
			set { this.sValue = value; }
		}
		public ComboboxItem(string Text, string Value)
		{
			this.sText = Text;
			this.sValue = Value;
		}
	}
}

