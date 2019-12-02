using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class SectionCollection : System.Collections.CollectionBase
	{
		private Section oParent;
		private Layout oLayout;
		public Section Parent {
			get { return this.oParent; }
		}
		public Layout Layout {
			get {
				if (this.oLayout == null) {
					Section TheParent = this.Parent;
					while (!(TheParent.GetType.Name == "Layout")) {
						TheParent = TheParent.Parent;
					}
					this.oLayout = TheParent;
				}
				return this.oLayout;
			}
		}
		public Section this[int Index] {
			get { return List[Index]; }
			set { List[Index] = value; }
		}
		public Section this[string Name] {
			get {
				Section Section = default(Section);
				foreach ( Section in this) {
					if (Section.Name == Name) {
						return Section;
					}
				}
				return null;
			}
		}
		public Section NextSection {
			get {
				if (this.Count > 0) {
					int n = 0;
					for (n = 0; n <= this.Count - 1; n++) {
						if (object.ReferenceEquals(this[n], Section)) {
							if (n < this.Count - 1) {
								return this[n + 1];
							}
							return null;
						}
					}
				}
				return null;
			}
		}
		public Section PreviousSection {
			get {
				if (this.Count > 0) {
					int n = 0;
					for (n = 0; n <= this.Count - 1; n++) {
						if (object.ReferenceEquals(this[n], Section)) {
							if (n > 0) {
								return this[n - 1];
							}
							return null;
						}
					}
				}
				return null;
			}
		}
		public Section FirstSection {
			get {
				if (this.Count > 0) {
					int n = 0;
					for (n = 0; n >= this.Count - 1; n += -1) {
						if (this[n].Visible)
							return this[n];
					}
				}
				return null;
			}
		}
		public Section LastSection {
			get {
				if (this.Count > 0) {
					int n = 0;
					for (n = this.Count - 1; n >= 0; n += -1) {
						if (this[n].Visible)
							return this[n];
					}
				}
				return null;
			}
		}
		public Section Add(Section Section)
		{
			this.List.Add(Section);
			Section.Collection = this;
			return Section;
		}
		public Section Add(string Name)
		{
			Section Section = this.CreateSection();
			Section.Name = Name;
			return this.Add(Section);
		}
		public Section Add(string Name, string Title)
		{
			Section Section = this.Add(Name);
			Section.Title.Text = Title;
			return this.Add(Section);
		}
		public Section Add(string Name, string Title, string Content)
		{
			Section Section = this.Add(Name, Title);
			Section.Content.Add(Content);
			return this.Add(Section);
		}
		public Section Add(string Name, int Top)
		{
			Section Section = this.Add(Name);
			Section.Top = Top;
			return Section;
		}
		public Section Add(string Name, string Title, int Top)
		{
			Section Section = this.Add(Name, Title);
			Section.Top = Top;
			return Section;
		}
		public Section Add(string Name, string Title, string Content, int Top)
		{
			Section Section = this.Add(Name, Title, Content);
			Section.Top = Top;
			return Section;
		}
		public Section Add(string Name, int Top, int Left)
		{
			Section Section = this.Add(Name);
			Section.Top = Top;
			Section.Left = Left;
			return Section;
		}
		public Section Add(string Name, string Title, int Top, int Left)
		{
			Section Section = this.Add(Name, Title);
			Section.Top = Top;
			Section.Left = Left;
			return Section;
		}
		public Section Add(string Name, string Title, string Content, int Top, int Left)
		{
			Section Section = this.Add(Name, Title, Content);
			Section.Top = Top;
			Section.Left = Left;
			return Section;
		}
		public Section Add(string Name, int Top, int Left, int Width)
		{
			Section Section = this.Add(Name, Top, Left);
			Section.Width = Width;
			return Section;
		}
		public Section Add(string Name, string Title, int Top, int Left, int Width)
		{
			Section Section = this.Add(Name, Title, Top, Left);
			Section.Width = Width;
			return Section;
		}
		public Section Add(string Name, string Title, string Content, int Top, int Left, int Width)
		{
			Section Section = this.Add(Name, Title, Content, Top, Left);
			Section.Width = Width;
			return Section;
		}
		public Section Add(string Name, int Top, int Left, int Width, int Height)
		{
			Section Section = this.Add(Name, Top, Left);
			Section.Width = Width;
			Section.Height = Height;
			return Section;
		}
		public Section Add(string Name, string Title, int Top, int Left, int Width, int Height)
		{
			Section Section = this.Add(Name, Title, Top, Left);
			Section.Width = Width;
			Section.Height = Height;
			return Section;
		}
		public Section Add(string Name, string Title, string Content, int Top, int Left, int Width, int Height)
		{
			Section Section = this.Add(Name, Title, Content, Top, Left);
			Section.Width = Width;
			Section.Height = Height;
			return Section;
		}
		public void Remove(int Index)
		{
			this.List.Remove(Index);
		}
		public void Remove(Section Section)
		{
			this.List.Remove(Section);
		}
		public virtual Section CreateSection()
		{
			return new Section(this);
		}
		public string Draw()
		{
			string ReturnString = "";
			if (this.List.Count > 0) {
				if (this.Layout.Technique == LayoutTechnique.Tables) {
					if (!this.Parent.IsDependent) {
						if (this.Parent.Dock == DockStyle.None) {
							ReturnString += "<TABLE CELLPADDING=\"0\" CELLSPACING=\"0\">";
						} else {
							ReturnString += "<TABLE WIDTH=\"100%\" HEIGHT=\"100%\" CELLPADDING=\"0\" CELLSPACING=\"0\">";
						}
					}
					if (this.Parent.LayoutDirection == LayoutDirection.Horizantal) {
						ReturnString += "<TR>";
					}
				}
				Section Section = default(Section);
				foreach ( Section in this.List) {
					if (this.Layout.Technique == LayoutTechnique.Tables && this.Parent.LayoutDirection == LayoutDirection.Vertical) {
						//If Not Me.Parent.IsDependent Then
						ReturnString += "<TR>";
						//End If
					}
					ReturnString += Section.Draw();
					if (this.Layout.Technique == LayoutTechnique.Tables && this.Parent.LayoutDirection == LayoutDirection.Vertical) {
						//If Not Me.Parent.IsDependent Then
						ReturnString += "</TR>";
						//End If
					}
				}
				if (this.Layout.Technique == LayoutTechnique.Tables) {
					if (this.Parent.LayoutDirection == LayoutDirection.Horizantal) {
						ReturnString += "</TR>";
					}
					Section NextSection = this.Parent.Parent.Sections.NextSection(this.Parent);
					if (NextSection == null || !NextSection.IsDependent) {
						ReturnString += "</TABLE>";
					}
				}
			}
			return ReturnString;
		}
		public SectionCollection(Section Parent)
		{
			this.oParent = Parent;
		}
	}
}
