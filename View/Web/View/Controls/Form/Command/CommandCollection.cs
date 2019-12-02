using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Form
{
	public class CommandCollection : Application.Base.CollectionBase
	{
		private Form oForm;
		private Style oStyle;
		public new Command this[int Index] {
			get { return base.Item(Index); }
		}
		public new Command this[string MemberName] {
			get {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this[i].Button.ID == MemberName) {
						return base.Item(i);
					}
				}
				return null;
			}
		}
		public Form Form {
			get { return this.oForm; }
		}
		public bool HasAutoDrawCommand {
			get {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this[i].AutoDraw) {
						return true;
					}
				}
				return false;
			}
		}
		public Command LastCommand {
			get {
				if (this.Count == 0)
					return null;
				return this[this.Count - 1];
			}
		}
		public Style Style {
			get {
				if (this.oStyle == null) {
					this.oStyle = new Style();
					this.oStyle.HorizontalAlignment = HorizontalAlignment.Right;
				}
				return this.oStyle;
			}
		}
		public Command Add(string MemberName, bool AutoDraw = false, bool UseDictionary = true)
		{
			Command Command = new Command(MemberName, this);
			Command.AutoDraw = AutoDraw;
			Command.Button.ParentControl = Form;
			Command.UseDictionary = UseDictionary;
			Command.Button.Container = Form.Container;
			return this.Add(Command);
		}
		public Command Add(string MemberName, string ImageSource, bool AutoDraw = false, bool UseDictionary = true)
		{
			Command Command = this.Add(MemberName, AutoDraw);
			Command.Button.ImageSource = ImageSource;
			Command.UseDictionary = UseDictionary;
			return Command;
		}
		public Command Add(Command Command)
		{
			if (Command != null) {
				base.List.Add(Command);
				return Command;
			}
			return null;
		}
		public CommandCollection(Form Form)
		{
			this.oForm = Form;
		}
	}
}
