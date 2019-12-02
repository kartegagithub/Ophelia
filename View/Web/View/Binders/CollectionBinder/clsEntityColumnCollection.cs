using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Base.DataGrid;
namespace Ophelia.Web.View.Binders
{
	public class EntityColumnCollection : Ophelia.Web.View.Base.DataGrid.ColumnCollection
	{
		public new CollectionBinder DataGrid {
			get { return base.DataGrid; }
		}
		internal string GetColumnTitle(string MemberName)
		{
			string Word = null;
			if (MemberName.IndexOf(".") > -1) {
				string[] Words = MemberName.Split(".");
				if (Words[Words.Length - 1] == "Name" || Words[Words.Length - 1] == "Count") {
					Word = Words[Words.Length - 2];
				} else {
					Word = Words[Words.Length - 1];
				}
			} else {
				Word = MemberName;
			}
			if (this.DataGrid.Client == null) {
				return MemberName;
			} else {
				string ReturnValue = Strings.Replace(this.DataGrid.Dictionary.GetWord("Concept." + Word), "Concept.", "");
				if (ReturnValue == null) {
					return "";
				} else {
					return ReturnValue;
				}
			}
		}
		public FilterBoxColumn AddFilterBoxColumn(string Title, string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember)
		{
			FilterBoxColumn FilterBoxColumn = this.AddFilterBoxColumn(Title, ID, MemberNameToFilter, DataSource);
			FilterBoxColumn.DataControl.DisplayMember = DisplayMember;
			return FilterBoxColumn;
		}
		public FilterBoxColumn AddFilterBoxColumn(string Title, string ID, string MemberNameToFilter, EntityCollection DataSource)
		{
			FilterBoxColumn FilterBoxColumn = this.AddFilterBoxColumn(Title, ID, MemberNameToFilter);
			FilterBoxColumn.DataControl.DataSource = DataSource;
			return FilterBoxColumn;
		}
		public FilterBoxColumn AddFilterBoxColumn(string Title, string ID, string MemberNameToFilter)
		{
			FilterBoxColumn FilterBoxColumn = this.AddFilterBoxColumn(Title, ID);
			FilterBoxColumn.DataControl.MemberNameToFilter = MemberNameToFilter;
			FilterBoxColumn.DataControl.DisplayMember = MemberNameToFilter;
			return FilterBoxColumn;
		}
		public FilterBoxColumn AddFilterBoxColumn(string Title, string ID)
		{
			return this.Add(this.CreateFilterBoxColumn(Title, ID));
		}
		protected virtual FilterBoxColumn CreateFilterBoxColumn(string Title, string ID)
		{
			return new FilterBoxColumn(this, this.GetColumnTitle(Title), ID);
		}
		public new NumberBoxColumn AddNumberBoxColumn(string Name)
		{
			return this.AddNumberBoxColumn(Name, 100);
		}
		public new NumberBoxColumn AddNumberBoxColumn(string Name, int Width)
		{
			return this.AddNumberBoxColumn(this.GetColumnTitle(Name), Name, Width);
		}
		public new NumberBoxColumn AddNumberBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateNumberBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.DataControl.Style.Width = Width;
			return this.Add(Column);
		}
		public new TextBoxColumn AddTextBoxColumn(string Name)
		{
			return this.AddTextBoxColumn(Name, 100);
		}
		public new TextBoxColumn AddTextBoxColumn(string Name, int Width)
		{
			return this.AddTextBoxColumn(this.GetColumnTitle(Name), Name, Width);
		}
		public new TextBoxColumn AddTextBoxColumn(string Name, string MemberName)
		{
			return this.AddTextBoxColumn(this.GetColumnTitle(Name), MemberName, -1);
		}
		public new TextBoxColumn AddTextBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateTextBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.DataControl.Style.Width = Width;
			return this.Add(Column);
		}
		public new FileBoxColumn AddFileBoxColumn(string Name)
		{
			return this.AddFileBoxColumn(Name, 100);
		}
		public new FileBoxColumn AddFileBoxColumn(string Name, int Width)
		{
			return this.AddFileBoxColumn(this.GetColumnTitle(Name), Name, Width);
		}
		public new FileBoxColumn AddFileBoxColumn(string Name, string MemberName)
		{
			return this.AddFileBoxColumn(this.GetColumnTitle(Name), MemberName, -1);
		}
		public new FileBoxColumn AddFileBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateFileBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.DataControl.Style.Width = Width;
			return this.Add(Column);
		}
		public new CheckBoxColumn AddCheckBoxColumn(string Name)
		{
			return this.AddCheckBoxColumn(this.GetColumnTitle(Name), Name, 30);
		}
		public new CheckBoxColumn AddCheckBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateCheckBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		public new LinkColumn AddLinkColumn(string Name)
		{
			return this.AddLinkColumn(Name, "");
		}
		public new LinkColumn AddLinkColumn(string Name, string Url)
		{
			return this.AddLinkColumn(Name, Url, 100);
		}
		public new LinkColumn AddLinkColumn(string Name, string Url, int Width)
		{
			return this.AddLinkColumn(this.GetColumnTitle(Name), Name, Url, Width);
		}
		public new LinkColumn AddLinkColumn(string Name, string MemberName, string Url, int Width)
		{
			Column Column = this.CreateLinkColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.LinkForEdit(Url);
			return this.Add(Column);
		}
		public new DateTimeColumn AddDateTimeColumn(string Name)
		{
			return this.AddDateTimeColumn(Name, -1);
		}
		public new DateTimeColumn AddDateTimeColumn(string Name, int Width)
		{
			return this.AddDateTimeColumn(this.GetColumnTitle(Name), Name, Width);
		}
		public new DateTimeColumn AddDateTimeColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateDateTimeColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.DataControl.Style.Width = Width;
			return this.Add(Column);
		}
		public virtual Column Add(string Name)
		{
			return this.Add(Name, 100);
		}
		public virtual Column Add(string Name, int Width)
		{
			return this.Add(Name, Name, Width);
		}
		public virtual Column Add(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		protected override Base.DataGrid.TextBoxColumn CreateTextBoxColumn(string Name, string MemberName)
		{
			return new TextBoxColumn(this, this.GetColumnTitle(Name), MemberName);
		}
		protected override Base.DataGrid.FileBoxColumn CreateFileBoxColumn(string Name, string MemberName)
		{
			return new FileBoxColumn(this, this.GetColumnTitle(Name), MemberName);
		}
		protected override Base.DataGrid.CheckBoxColumn CreateCheckBoxColumn(string Name, string MemberName)
		{
			return new CheckBoxColumn(this, this.GetColumnTitle(Name), MemberName);
		}
		protected override Base.DataGrid.NumberBoxColumn CreateNumberBoxColumn(string Name, string MemberName)
		{
			return new NumberBoxColumn(this, this.GetColumnTitle(Name), MemberName);
		}
		protected override Base.DataGrid.LinkColumn CreateLinkColumn(string Name, string MemberName)
		{
			return new LinkColumn(this, this.GetColumnTitle(Name), MemberName);
		}
		protected override object CreateColumn(string Name, string MemberName)
		{
			return new Column(this, this.GetColumnTitle(Name), MemberName);
		}
		internal EntityColumnCollection(CollectionBinder DataGrid) : base(DataGrid)
		{
		}
	}
}
