using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
namespace Ophelia.Web.View.UI
{
	public class Header
	{
		private MetaTagCollection oMetaTags;
		private HeadLinkCollection oHeadLinks;
		private string sTitle = string.Empty;
		private string sContentLanguage = "TR";
		private StyleSheet oStyleSheet;
		private ScriptCollection oScriptManager;
		private XmlNamespaceCollection oXmlNamespaces;
		private string sVersionNumber;
		public string VersionNumber {
			get { return this.sVersionNumber; }
			set { this.sVersionNumber = value; }
		}
		public ScriptCollection ScriptManager {
			get {
				if (this.oScriptManager == null) {
					this.oScriptManager = new ScriptCollection();
					this.oScriptManager.VersionNumber = this.VersionNumber;
				}
				return this.oScriptManager;
			}
		}
		public XmlNamespaceCollection XmlNamespaces {
			get {
				if (this.oXmlNamespaces == null)
					this.oXmlNamespaces = new XmlNamespaceCollection();
				return this.oXmlNamespaces;
			}
		}
		public StyleSheet StyleSheet {
			get {
				if (this.oStyleSheet == null)
					this.oStyleSheet = new StyleSheet();
				return this.oStyleSheet;
			}
		}
		public MetaTagCollection MetaTags {
			get {
				if (this.oMetaTags == null)
					this.oMetaTags = new MetaTagCollection();
				return this.oMetaTags;
			}
		}
		public HeadLinkCollection Links {
			get {
				if (this.oHeadLinks == null)
					this.oHeadLinks = new HeadLinkCollection(this);
				return this.oHeadLinks;
			}
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public string ContentLanguage {
			get { return this.sContentLanguage; }
			set {
				this.sContentLanguage = value;
				this.MetaTags.Add(View.Web.UI.MetaTag.MetaTagMessageType.Information, View.Web.UI.MetaTag.MetaTagType.ContentLanguage, value);
			}
		}
		internal string DrawXmlNamespaces()
		{
			return this.XmlNamespaces.Draw();
		}
		internal string Draw()
		{
			Content Content = new Content();
			Content.Add("<title>" + Title + "</title>");
			Content.Add(this.MetaTags.Draw());
			Content.Add(this.Links.Draw(HeadLink.HeadlineDrawType.BeforeDefaultCssDrawn));
			Content.Add(this.StyleSheet.Draw());
			Content.Add(this.Links.Draw(HeadLink.HeadlineDrawType.AfterDefaultCssDrawn));
			Content.Add(this.ScriptManager.Draw());
			return Content.Value;
		}
	}
}
