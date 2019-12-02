using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public abstract class Container : ComplexWebControl, IContainer
	{
		private WebControlCollection oControls;
		private ServerSide.ScriptManager.ScriptCollection oScripts;
		private Content oContent;
		public WebControlCollection Controls {
			get {
				if (this.oControls == null) {
					this.oControls = new WebControlCollection(this);
				}
				return this.oControls;
			}
		}
		public ServerSide.ScriptManager.ScriptCollection Scripts {
			get {
				if (this.oScripts == null) {
					this.oScripts = new ServerSide.ScriptManager.ScriptCollection();
				}
				return this.oScripts;
			}
		}
		ServerSide.ScriptManager.ScriptCollection IContainer.ScriptCollection {
			get { return Scripts; }
		}
		public bool HasContent {
			get { return this.Controls.Count > 0 || !string.IsNullOrEmpty(this.Content.Value); }
		}
		public Content Content {
			get {
				if (this.oContent == null) {
					this.oContent = new Content();
				}
				return this.oContent;
			}
		}
		public string InnerHtml()
		{
			Content Content = new Content();
			this.DrawControls(Content);
			return Content.Value;
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.DrawControls(Content);
		}
		protected void DrawControls(Content Content)
		{
			for (int i = 0; i <= this.Controls.Count - 1; i++) {
				this.ConfigureSubControls(this.Controls(i));
				Content.Add(this.Controls(i).Draw);
			}
		}
	}
}
