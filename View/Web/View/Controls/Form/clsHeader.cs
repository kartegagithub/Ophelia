using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Form
{
	public class Header
	{
		private Form oForm;
		private Structure.Structure oControl;
		private string sTitle = "";
		private Style oStyle;
		private bool bDrawDefaultRegionAnyway = false;
		private string sFormMessage = "";
		private string sFormMessageClass = "";
		public string FormMessageClass {
			get { return this.sFormMessageClass; }
			set { this.sFormMessageClass = value; }
		}
		public string FormMessage {
			get { return this.sFormMessage; }
			set { this.sFormMessage = value; }
		}
		public bool DrawDefaultRegionAnyway {
			get { return this.bDrawDefaultRegionAnyway; }
			set { this.bDrawDefaultRegionAnyway = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
					this.oStyle.BackgroundColor = "#dcddde";
					this.oStyle.Padding = 5;
				}
				return this.oStyle;
			}
		}
		public Structure.Structure Control {
			get {
				if (this.oControl == null) {
					this.oControl = new Structure.Structure("Header", 2, 1);
					this.oControl.CellPadding = 2;
					this.oControl.CellSpacing = 2;
					this.oControl.Style.HorizontalAlignment = SectionAlignment.Left;
					this.oControl.Style.Font.Color = "black";
					this.oControl.Style.Font.Weight = Forms.FontWeight.Bold;
					this.oControl.Style.Dock = DockStyle.Fill;
				}
				return this.oControl;
			}
		}
		public string Draw()
		{
			if (!string.IsNullOrEmpty(this.Title) || !string.IsNullOrEmpty(this.FormMessage) || this.DrawDefaultRegionAnyway) {
				if (string.IsNullOrEmpty(this.FormMessage)) {
					this.Control(0, 0).RowSpan = 2;
					this.Control(0, 0).Content.Add(this.Title);
				} else {
					this.Control(0, 0).Content.Add(this.FormMessage);
					this.Control(0, 0).Style.Class = this.FormMessageClass;
					this.Control(1, 0).Content.Add(this.Title);
				}
				return this.Control.Draw;
			}
			return "";
		}
		public Form Form {
			get { return this.oForm; }
		}
		public Header(Form Form)
		{
			this.oForm = Form;
		}
	}
}
