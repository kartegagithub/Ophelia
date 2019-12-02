using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.ServerSide.ScriptManager
{
	public class ScriptCollection
	{
		private ArrayList oScripts = new ArrayList();
		private bool bIgnoreSecurity = false;
		private string sApplicationBase = "/";
		private Script oGeneralScript;
		private string sVersionNumber;
		private bool bUseCustomJqueryLibrary = false;
		public string VersionNumber {
			get { return this.sVersionNumber; }
			set { this.sVersionNumber = value; }
		}
		public bool UseCustomJqueryLibrary {
			get { return this.bUseCustomJqueryLibrary; }
			set { this.bUseCustomJqueryLibrary = value; }
		}
		public int Count {
			get { return this.Scripts.Count; }
		}
		public ArrayList Scripts {
			get { return this.oScripts; }
		}
		public Script this[int index] {
			get {
				if (this.oScripts.Count > index) {
					return this.oScripts[index];
				}
				return null;
			}
		}
		public Script FirstScript {
			get { return this.oGeneralScript; }
		}
		private void AddGeneralScripts()
		{
			this.oGeneralScript = this.Add("GeneralScript");
		}
		public Script this[string Name] {
			get {
				for (int i = this.Scripts.Count - 1; i >= 0; i += -1) {
					if (this[i].Name == Name) {
						return this[i];
					}
				}
				return null;
			}
		}
		public string ApplicationBase {
			get { return this.sApplicationBase; }
			set { this.sApplicationBase = value; }
		}
		public bool IgnoreSecurity {
			get { return this.bIgnoreSecurity; }
			set { this.bIgnoreSecurity = value; }
		}
		public Script Add(string Name, string Path, int Index = -1, bool CheckedIsExists = false)
		{
			Script Script = this.Add(Name);
			if (Script != null) {
				Script.Path = Path;
			}
			return Script;
		}
		public Script AddGeneralJQueryLibrary()
		{
			Script Script = this.Add("jquery161min");
			if (Script != null) {
				Script.Path = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jquery161min.js", UI.Current.FileUsageType);
			}
			return Script;
		}
		public Script AddToolTipJQueryLibrary()
		{
			Script Script = this.Add("jqueryBtmin");
			if (Script != null) {
				Script.Path = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jqueryBtmin.js", UI.Current.FileUsageType);
			}
			return Script;
		}
		public Script Add(string Name, int Index = -1, bool CheckedIsExists = false)
		{
			if (!this.UseCustomJqueryLibrary || !Name.Equals("jquery161min", StringComparison.InvariantCulture)) {
				if (this[Name] == null) {
					Script Script = new Script(Name, this);
					return this.Add(Script, Index, CheckedIsExists);
				}
				return this[Name];
			}
			return null;
		}
		public Script Add(Script Script, int Index = -1, bool CheckedIsExists = false)
		{
			if (CheckedIsExists) {
				if ((this.Scripts[Script.Name] != null)) {
					return this.Scripts[Script.Name];
				}
			}
			if (Index == -1 || this.oScripts.Count < Index) {
				this.oScripts.Add(Script);
			} else {
				this.oScripts.Insert(Index, Script);
			}
			return Script;
		}
		private string GetVersionNumber()
		{
			return this.VersionNumber;
		}
		public string Draw()
		{
			Content Content = new Content();
			Content UrlContent = new Content();
			for (int i = 0; i <= this.Scripts.Count - 1; i++) {
				if (!string.IsNullOrEmpty(this[i].Path)) {
					if (this[i].Path.Contains("?")) {
						this[i].Path += "&version=" + this.GetVersionNumber();
					} else {
						this[i].Path += "?version=" + this.GetVersionNumber();
					}
					UrlContent.Add(this[i].Draw(false));
				} else {
					Content.Add(this[i].Draw(false));
				}
			}
			return UrlContent.Value + Content.Value;
		}
		public ScriptCollection()
		{
			this.AddGeneralScripts();
		}
	}
}
