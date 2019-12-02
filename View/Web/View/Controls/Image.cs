using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Image : Controls.DataControl
	{
		private string sUrl;
		private string sImageSource;
		private bool bNewWindow = false;
		private string sTooltip;
		private string sAlternateText;
		private string sOnLoadEvent;
		public Image(string MemberName, string ImageSource) : this(MemberName, ImageSource, "")
		{
		}
		public Image(string MemberName, string ImageSource, string AlternateText) : this(MemberName, ImageSource, AlternateText, "")
		{
		}
		public Image(string MemberName, string ImageSource, string AlternateText, string ToolTip) : this(MemberName, ImageSource, AlternateText, ToolTip, "")
		{
		}
		public Image(string MemberName, string ImageSource, string AlternateText, string ToolTip, string Url) : this(MemberName, ImageSource, AlternateText, ToolTip, Url, false)
		{
		}
		public Image(string MemberName, string ImageSource, string AlternateText, string ToolTip, string Url, bool NewWindow)
		{
			this.ID = MemberName;
			this.Url = Url;
			this.AlternateText = AlternateText;
			this.NewWindow = NewWindow;
			this.ImageSource = ImageSource;
			this.Tooltip = ToolTip;
			this.Style.Borders.Width = 0;
		}
		public string OnLoadEvent {
			get { return this.sOnLoadEvent; }
			set { this.sOnLoadEvent = value; }
		}
		public string AlternateText {
			get { return this.sAlternateText; }
			set { this.sAlternateText = value; }
		}
		public string ImageSource {
			get { return this.sImageSource; }
			set { this.sImageSource = value; }
		}
		public string Url {
			get { return this.sUrl; }
			set { this.sUrl = value; }
		}
		public bool NewWindow {
			get { return this.bNewWindow; }
			set { this.bNewWindow = value; }
		}
		public string Tooltip {
			get { return this.sTooltip; }
			set { this.sTooltip = value; }
		}
		protected override void DrawEvents(Content Content)
		{
			base.DrawEvents(Content);
			this.CheckEvent(this.OnLoadEvent);
			if (!string.IsNullOrEmpty(this.OnLoadEvent))
				Content.Add(" onload=\"").Add(this.OnLoadEvent).Add("\"");
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content NewContent = new Content();
			if (!string.IsNullOrEmpty(this.ImageSource)) {
				NewContent.Add("<img " + (!string.IsNullOrEmpty(this.Tooltip) ? " title=\"" + this.Tooltip + "\"" : ""));
				NewContent.Add(" alt=\"" + this.AlternateText + "\"");
				NewContent.Add(" " + (!string.IsNullOrEmpty(this.ID) ? "id=\"" + this.ID + "\"" : ""));
				NewContent.Add(" " + Style.Draw);
				this.DrawEvents(NewContent);
				NewContent.Add(" src=\"" + ImageSource + "\"");
				if (this.oAttributes != null) {
					for (int i = 0; i <= this.Attributes.Count - 1; i++) {
						NewContent.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
					}
				}
				NewContent.Add(">");
				if (!string.IsNullOrEmpty(this.Url)) {
					Link ImageLink = new Link(this.ID + "ImageLink");
					ImageLink.Url = this.Url;
					ImageLink.Value = NewContent.Value;
					ImageLink.NewWindow = this.NewWindow;
					Content.Add(ImageLink.Draw);
				} else {
					Content.Add(NewContent.Value);
				}
			}

		}
	}
}
