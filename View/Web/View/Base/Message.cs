using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View
{
	public class Message
	{
		private string Text = "";
		private string Color = "Black";
		private Controls.Label oControl;
		private Controls.Label Control {
			get {
				if (this.oControl == null)
					this.oControl = new Controls.Label("Message");
				return this.oControl;
			}
		}
		public bool IsSet {
			get { return (string.IsNullOrEmpty(Text) ? false : true); }
		}
		public void Send(string Text, string Color, DisplayMessageType DisplayMessageType = DisplayMessageType.ClearMessage)
		{
			if (DisplayMessageType == DisplayMessageType.Append) {
				this.Text += Text;
			} else if (DisplayMessageType == DisplayMessageType.AppendAsNewLine) {
				this.Text += "<br>" + Text;
			} else {
				this.Text = Text;
			}
			this.Color = Color;
		}
		public void SendSuccess(string Text, DisplayMessageType DisplayMessageType = DisplayMessageType.ClearMessage)
		{
			this.Send(Text, "Green", DisplayMessageType);
		}
		public void SendFail(string Text, DisplayMessageType DisplayMessageType = DisplayMessageType.ClearMessage)
		{
			this.Send(Text, "Red", DisplayMessageType);
		}
		public string Display(string Text, bool Fail = false, DisplayMessageType DisplayMessageType = DisplayMessageType.ClearMessage)
		{
			if (Fail) {
				this.SendFail(Text, DisplayMessageType);
			} else {
				this.SendSuccess(Text, DisplayMessageType);
			}
			return this.Draw();
		}
		public void Display(ref Controls.IContainer ParentControl, string Text, bool Fail = false, DisplayMessageType DisplayMessageType = DisplayMessageType.ClearMessage)
		{
			if (ParentControl != null) {
				if (Fail) {
					this.SendFail(Text, DisplayMessageType);
				} else {
					this.SendSuccess(Text, DisplayMessageType);
				}
				ParentControl.Controls.Add(this.GetControl());
			}
		}
		public void Reset()
		{
			this.Text = "";
			this.Color = "Black";
		}
		public Controls.Label GetControl(bool NeedTopSpace = true)
		{
			if (!string.IsNullOrEmpty(this.Text)) {
				this.Control.Style.Font.Color = this.Color;
				if (NeedTopSpace) {
					this.Control.Value = "<br>" + this.Text + "<br><br>";
				} else {
					this.Control.Value = "<br>" + this.Text + "<br><br>";
				}
				return this.Control;
			}
			return null;
		}
		public string Draw(bool NeedTopSpace = true)
		{
			if (!string.IsNullOrEmpty(this.Text)) {
				if (NeedTopSpace) {
					return "<SPAN STYLE=\"color:" + this.Color + ";\"><br>" + this.Text + "</SPAN><br><br>";
				} else {
					return "<SPAN STYLE=\"color:" + this.Color + ";\">" + this.Text + "</SPAN><br><br>";
				}
			} else {
				return "";
			}
		}
		public override string ToString()
		{
			return this.Text;
		}
		public enum DisplayMessageType
		{
			ClearMessage = 1,
			Append = 2,
			AppendAsNewLine = 3
		}
	}
}
