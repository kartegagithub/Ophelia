using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class HeadLink
	{
		private ReleationShipType eType;
		private string sSource;
		private HeadLinkCollection oCollection;
		private HeadlineDrawType eDrawType = HeadlineDrawType.AfterDefaultCssDrawn;
		public ReleationShipType Type {
			get { return this.eType; }
			set { this.eType = value; }
		}
		public HeadlineDrawType DrawType {
			get { return this.eDrawType; }
			set { this.eDrawType = value; }
		}
		public string Source {
			get { return this.sSource; }
			set { this.sSource = value; }
		}
		public string Draw()
		{
			Content Content = new Content();
			if (!string.IsNullOrEmpty(this.Source)) {
				if (this.Type != ReleationShipType.Canonical && !string.IsNullOrEmpty(this.oCollection.Header.VersionNumber)) {
					this.Source = this.Source + this.Source.Contains("?") ? "&" : "?" + "version=" + this.oCollection.Header.VersionNumber;
				}
				switch (this.Type) {
					case ReleationShipType.FavIcon:
						Content.Add("<link rel=\"icon\" href=\"").Add(this.Source).Add("\">");
						Content.Add("<link rel=\"shortcut icon\" href=\"").Add(this.Source).Add("\" type=\"").Add(Functions.GetMimeTypeByFileName(this.Source)).Add("\">");
						break;
					case ReleationShipType.StyleSheet:
						Content.Add("<link href=\"").Add(this.Source).Add("\" rel=\"stylesheet\" type=\"text/css\">");
						break;
					case ReleationShipType.Canonical:
						Content.Add("<link rel=\"canonical\" href=\"").Add(this.Source).Add("\">");
						break;
				}
			}
			return Content.Value;
		}
		public bool IsEqualTo(HeadLink HeadLink)
		{
			if (this.Type != HeadLink.Type)
				return false;
			if (this.Source != HeadLink.Source)
				return false;
			return true;
		}
		public HeadLink(HeadLinkCollection Collection)
		{
			this.oCollection = Collection;
		}
		public HeadLink(HeadLinkCollection Collection, string Source, ReleationShipType Type) : this(Collection)
		{
			this.Source = Source;
			this.Type = Type;
		}
		public enum ReleationShipType : int
		{
			FavIcon = 1,
			StyleSheet = 2,
			Canonical = 3
		}
		public enum HeadlineDrawType : int
		{
			AfterDefaultCssDrawn = 1,
			BeforeDefaultCssDrawn = 2
		}
	}
}
