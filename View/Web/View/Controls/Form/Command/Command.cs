using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Controls.Form
{
	public class Command
	{
		private Button oButton;
		private bool bAutoDraw = false;
		private bool bUseDictionary = true;
		private CommandCollection oCollection;
		public CommandCollection Collection {
			get { return this.oCollection; }
		}
		public Button Button {
			get { return this.oButton; }
		}
		public Ophelia.Web.View.Style Style {
			get { return this.Button.Style; }
		}
		public string ActionName {
			get { return this.Button.ID + "_Executed"; }
		}
		public bool AutoDraw {
			get { return this.bAutoDraw; }
			set { this.bAutoDraw = value; }
		}
		public bool UseDictionary {
			get {
				if (!this.Collection.Form.UseDictionary)
					return false;
				return this.bUseDictionary;
			}
			set { this.bUseDictionary = value; }
		}
		public string Draw()
		{
			if (this.UseDictionary) {
				this.oButton.OnClickEvent += "SetAction_" + this.Collection.Form.ID + "('" + this.Button.ID + "');";
			} else {
				this.oButton.OnClickEvent += "SetAction_" + this.Collection.Form.ID + "('" + GetAvailableIDValue(this.Button.ID) + "');";
			}
			if ((this.Collection.Form.Client != null) && (this.Collection.Form.Client.Dictionary != null)) {
				if (string.IsNullOrEmpty(this.Button.Value)) {
					if (this.UseDictionary) {
						this.oButton.Value = this.Collection.Form.Client.Dictionary.GetWord("Command." + this.Button.ID);
					} else {
						this.oButton.Value = this.Button.ID;
					}
				}
			}
			return this.Button.Draw;
		}
		public Command(string Name, CommandCollection Collection)
		{
			this.oButton = new Button(Name);
			this.oCollection = Collection;
		}
		public Command(string Name, string ImageSource, CommandCollection Collection) : this(Name, Collection)
		{
			this.oButton.ImageSource = ImageSource;
		}
		public Command(string Name, Container Container, CommandCollection Collection)
		{
			this.oButton = Container.Controls.AddButton(Name);
		}
		public Command(string Name, string ImageSource, Container Container, CommandCollection Collection) : this(Name, Container, Collection)
		{
			this.oButton.ImageSource = ImageSource;
		}
	}
}
