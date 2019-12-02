using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Link : DataControl
	{
		private bool bNewWindow;
		private string sUrl;
		private string sText;
		private RelationshipType[] eRel;
		private TargetType eTarget;
		public string Url {
			get { return this.sUrl; }
			set { this.sUrl = value; }
		}
		public virtual string Text {
			get { return this.sText; }
			set { this.sText = value; }
		}
		public bool NewWindow {
			get { return this.bNewWindow; }
			set {
				this.bNewWindow = value;
				if (value)
					this.eTarget = TargetType.Blank;
			}
		}
		public RelationshipType[] Rel {
			get { return this.eRel; }
			set { this.eRel = value; }
		}
		public TargetType Target {
			get { return this.eTarget; }
			set { this.eTarget = value; }
		}
		public void AddImage(string URL)
		{
			Image Image = new Image("", URL);
			this.Value = Image.Draw;
		}
		public void AddImage(string URL, string DefaultValue, bool AfterDrawn = true, string AlternateText = "")
		{
			Image Image = new Image("", URL);
			if (!string.IsNullOrEmpty(AlternateText))
				Image.AlternateText = System.Web.HttpContext.Current.Server.HtmlEncode(AlternateText);
			if (AfterDrawn) {
				this.Value = DefaultValue + Image.Draw;
			} else {
				this.Value = Image.Draw + DefaultValue;
			}
		}
		public Link SetUrl(string URL)
		{
			this.Url = URL;
			return this;
		}
		public Link()
		{
			this.bNewWindow = false;
			this.sText = string.Empty;
			this.sUrl = string.Empty;
			this.eTarget = TargetType.None;
		}
		public Link(string MemberName) : this()
		{
			this.ID = MemberName;
		}
		public Link(string MemberName, string Url, string Value) : this(MemberName)
		{
			this.sUrl = Url;
			this.Value = Value;
		}
		public Link(string MemberName, string Url, string Value, bool NewWindow) : this(MemberName, Url, Value)
		{
			this.NewWindow = NewWindow;
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Add("<a");
			if (!string.IsNullOrEmpty(base.ID))
				Content.Add(" id=\"").Add(base.ID).Add("\"");

			if (this.Target != TargetType.None) {
				Content.Add(" target=\"_" + this.Target.ToString().ToLower()).Add("\"");
			}

			if (this.oAttributes != null) {
				for (int i = 0; i <= this.oAttributes.Count - 1; i++) {
					Content.Add(" " + this.oAttributes.Keys(i).ToString() + "=\"" + this.oAttributes.Values(i).ToString() + "\"");
				}
			}

			Content.Add(base.Style.Draw);
			base.DrawEvents(Content);
			if (!string.IsNullOrEmpty(this.Url))
				Content.Add(" href=\"").Add(this.Url).Add("\"");
			if (!string.IsNullOrEmpty(base.Title))
				Content.Add(" title=\"").Add(base.Title).Add("\"");
			if (this.Rel != null && this.Rel.Length > 0)
				Content.Add(" rel=\"").Add(string.Join(",", this.Rel.Select(x => x.ToString().ToLower()).ToArray())).Add("\"");
			Content.Add(">");
			Content.Add(base.Value);
			Content.Add("</a>");
		}
		public enum RelationshipType
		{
			None = 0,
			Alternate = 1,
			StyleSheet = 2,
			Start = 3,
			Next = 4,
			Prev = 5,
			Contents = 6,
			Index = 7,
			Glossary = 8,
			Copyright = 9,
			Chapter = 10,
			Section = 11,
			SubSection = 12,
			Appendix = 13,
			Bookmark = 14,
			NoFollow = 15,
			License = 16,
			Tag = 17,
			Help = 18,
			Friend = 19,
			External = 20
		}
		public enum TargetType
		{
			None = 0,
			Blank = 1,
			Self = 2,
			Parent = 3,
			Top = 4
		}
	}
}
