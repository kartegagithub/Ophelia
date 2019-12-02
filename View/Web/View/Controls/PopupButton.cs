using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class PopupButton : Button
	{
		private PopupControl oPopup;
		public PopupControl Popup {
			get { return this.oPopup; }
			set { this.oPopup = value; }
		}
		private void Configure(string Content, Container ContentControl, PopupControl.PopupAutoClosingType AutoClosingType)
		{
			this.oPopup = new PopupControl(this.ID);
			if (!string.IsNullOrEmpty(Content))
				this.Popup.ContentRegion.Content.Add(Content);
			if (ContentControl != null)
				this.Popup.ContentRegion.Controls.Add(ContentControl);
			this.oPopup.AutoClosingType = AutoClosingType;
			this.oPopup.Hide();
			this.OnClickEvent = Popup.ShowEvent;
		}
		public PopupButton(string ID) : base(ID)
		{
			this.Configure("", null, PopupControl.PopupAutoClosingType.Hide);
		}
		public override void OnBeforeDraw(Content Content)
		{
			base.OnBeforeDraw(Content);
			Content.Add(this.Popup.Draw);
		}
		public PopupButton(string ID, string Content, PopupControl.PopupAutoClosingType AutoClosingType) : base(ID)
		{
			this.Configure(Content, null, AutoClosingType);
		}
		public PopupButton(string ID, Container Content, PopupControl.PopupAutoClosingType AutoClosingType) : base(ID)
		{
			this.Configure("", Content, AutoClosingType);
		}
		public PopupButton(string ID, string Value, Container Content, PopupControl.PopupAutoClosingType AutoClosingType) : base(ID, Value)
		{
			this.Configure("", Content, AutoClosingType);
		}
		public PopupButton(string ID, string Value, string Content, PopupControl.PopupAutoClosingType AutoClosingType) : base(ID, Value)
		{
			this.Configure(Content, null, AutoClosingType);
		}
		public PopupButton(string ID, string ImageSource, ButtonType Type) : base(ID, ImageSource, Type)
		{
			this.Configure("", null, PopupControl.PopupAutoClosingType.Hide);
		}
		public PopupButton(string ID, string ImageSource, ButtonType Type, string Content, PopupControl.PopupAutoClosingType AutoClosingType) : this(ID, ImageSource, Type)
		{
			this.Configure(Content, null, AutoClosingType);
		}
		public PopupButton(string ID, string ImageSource, ButtonType Type, Container Content, PopupControl.PopupAutoClosingType AutoClosingType) : this(ID, ImageSource, Type)
		{
			this.Configure("", Content, AutoClosingType);
		}
	}
}
