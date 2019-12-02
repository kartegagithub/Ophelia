using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class Heading : Container
	{
		private string sTitle;
		private HeadingType eType;
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Add("<").Add(this.eType.ToString().ToLower());
			if (!string.IsNullOrEmpty(base.ID))
				Content.Add(" id=\"").Add(base.ID).Add("\"");
			if (!string.IsNullOrEmpty(this.Title))
				Content.Add(" title=\"").Add(this.Title).Add("\"");
			Content.Add(base.Style.Draw());
			base.DrawEvents(Content);
			Content.Add(this.Style.Draw).Add(">").Add(this.Content.Value);
			base.DrawControls(Content);
			Content.Add("</").Add(this.eType.ToString().ToLower()).Add(">");
		}
		public HeadingType Type {
			get { return this.eType; }
			set { this.eType = value; }
		}
		public enum HeadingType
		{
			H1 = 0,
			H2 = 1,
			H3 = 2,
			H4 = 3,
			H5 = 4,
			H6 = 5
		}
		public Heading(string MemberName, string Value, HeadingType Type) : this(MemberName, Value)
		{
			this.Type = Type;
		}
		public Heading(string MemberName, string Value) : this(MemberName)
		{
			this.Content.Add(Value);
		}
		public Heading(string MemberName) : this()
		{
			base.ID = MemberName;
		}
		public Heading()
		{
			base.ID = string.Empty;
			this.eType = HeadingType.H1;
		}
	}
}

