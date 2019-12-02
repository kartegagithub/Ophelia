using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class ColumnCollection : AnyColumnCollection
	{
		private AnyColumnCollection oCollapsedColumns;
		private AnyColumnCollection oExpandedColumns;
		private Style oStyle;
		public AnyColumnCollection CollapsedColumns {
			get { return this.oCollapsedColumns; }
		}
		public AnyColumnCollection ExpandedColumns {
			get { return this.oExpandedColumns; }
		}
		public virtual string GetFieldTitle(string MemberName)
		{
			string Word = null;
			if (!string.IsNullOrEmpty(MemberName)) {
				if (MemberName.IndexOf(".") > -1) {
					string[] Words = MemberName.Split(".");
					if (Words[Words.Length - 1] == "Name" || Words[Words.Length - 1] == "Count") {
						Word = Words[Words.Length - 2];
					} else {
						if (Words[Words.Length - 1] == "ID" && Words.Count == 2) {
							Word = MemberName.Replace(".ID", "");
						} else {
							Word = Words[Words.Length - 1];
						}
					}
				} else {
					Word = MemberName;
				}
				if (this.DataGrid != null && this.DataGrid.Page != null && this.DataGrid.Page.Client != null && this.DataGrid.Page.Client.Dictionary != null) {
					return this.DataGrid.Page.Client.Dictionary.GetWord("Concept." + Word).Replace("Concept.", "");
				} else {
					return "";
				}
			}
			return "";
		}
		public override bool Remove(string MemberName)
		{
			this.ExpandedColumns.Remove(MemberName);
			//Me.CollapsedColumns.Remove(MemberName)
			return base.Remove(MemberName);
		}
		public ComboBoxColumn AddSimpleGridComboColumn(string MemberName, ICollection DataSource, int Width = 100, bool UseDictionary = true)
		{
			return this.AddSimpleGridComboColumn(MemberName, this.GetFieldTitle(MemberName), DataSource, Width);
		}
		public ComboBoxColumn AddSimpleGridComboColumn(string MemberName, string Name, ICollection DataSource, int Width = 100)
		{
			ComboBoxColumn Column = base.Add(new ComboBoxColumn(this, Name, MemberName));
			Column.Style.Width = Width;
			Column.DataSource = DataSource;
			return Column;
		}
		public CheckBoxColumn AddCheckBoxColumn(string Name, string MemberName, int Width = 100)
		{
			Column Column = this.CreateCheckBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		public NumberBoxColumn AddNumberBoxColumn(string MemberName)
		{
			return this.AddNumberBoxColumn(MemberName, 100);
		}
		public NumberBoxColumn AddNumberBoxColumn(string MemberName, int Width)
		{
			return this.AddNumberBoxColumn(this.GetFieldTitle(MemberName), MemberName, Width);
		}
		public NumberBoxColumn AddNumberBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateNumberBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		public InfinityNumberBoxColumn AddInfinityNumberBoxColumn(string MemberName, string Message = "")
		{
			return this.AddInfinityNumberBoxColumn(this.GetFieldTitle(MemberName), MemberName, 100, Message);
		}
		public InfinityNumberBoxColumn AddInfinityNumberBoxColumn(string Name, string MemberName, int Width, string Message)
		{
			Column Column = this.CreateInfinityNumberBoxColumn(Name, MemberName, Message);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		protected virtual Base.DataGrid.InfinityNumberBoxColumn CreateInfinityNumberBoxColumn(string Name, string MemberName, string Message)
		{
			return new InfinityNumberBoxColumn(this, Name, MemberName, Message);
		}
		public TextBoxColumn AddTextBoxColumn(string MemberName)
		{
			return this.AddTextBoxColumn(MemberName, 100);
		}
		public TextBoxColumn AddTextBoxColumn(string MemberName, int Width)
		{
			return this.AddTextBoxColumn(this.GetFieldTitle(MemberName), MemberName, Width);
		}
		public TextBoxColumn AddTextBoxColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateTextBoxColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		public DateTimeColumn AddDateTimeColumn(string MemberName)
		{
			return this.AddDateTimeColumn(MemberName, 100);
		}
		public DateTimeColumn AddDateTimeColumn(string MemberName, int Width)
		{
			return this.AddDateTimeColumn(this.GetFieldTitle(MemberName), MemberName, Width);
		}
		public DateTimeColumn AddDateTimeColumn(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateDateTimeColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		public LinkColumn AddLinkColumn(string MemberName)
		{
			return this.AddLinkColumn(MemberName, "");
		}
		public LinkColumn AddLinkColumn(string MemberName, string Url)
		{
			return this.AddLinkColumn(MemberName, Url, 100);
		}
		public LinkColumn AddLinkColumn(string MemberName, string Url, int Width)
		{
			return this.AddLinkColumn(this.GetFieldTitle(MemberName), MemberName, Url, Width);
		}
		public LinkColumn AddLinkColumn(string Name, string MemberName, string Url, int Width)
		{
			Column Column = this.CreateLinkColumn(Name, MemberName);
			Column.Style.Width = Width;
			Column.LinkForEdit(Url);
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
		public virtual Column Add(string Name, string MemberName)
		{
			return this.Add(Name, MemberName, 100);
		}
		public virtual Column Add(string Name, string MemberName, int Width)
		{
			Column Column = this.CreateColumn(Name, MemberName);
			Column.Style.Width = Width;
			return this.Add(Column);
		}
		protected virtual Base.DataGrid.NumberBoxColumn CreateNumberBoxColumn(string Name, string MemberName)
		{
			return new NumberBoxColumn(this, Name, MemberName);
		}
		protected virtual Base.DataGrid.TextBoxColumn CreateTextBoxColumn(string Name, string MemberName)
		{
			return new TextBoxColumn(this, Name, MemberName);
		}
		protected virtual Base.DataGrid.FileBoxColumn CreateFileBoxColumn(string Name, string MemberName)
		{
			return new FileBoxColumn(this, Name, MemberName);
		}
		protected virtual Base.DataGrid.DateTimeColumn CreateDateTimeColumn(string Name, string MemberName)
		{
			return new DateTimeColumn(this, Name, MemberName);
		}
		protected virtual Base.DataGrid.CheckBoxColumn CreateCheckBoxColumn(string Name, string MemberName)
		{
			return new CheckBoxColumn(this, Name, MemberName);
		}
		protected virtual Base.DataGrid.LinkColumn CreateLinkColumn(string Name, string MemberName)
		{
			return new LinkColumn(this, Name, MemberName);
		}
		protected virtual object CreateColumn(string Name, string MemberName)
		{
			return new Column(this, Name, MemberName);
		}
		public ColumnCollection(DataGrid DataGrid) : base(DataGrid)
		{
			this.oCollapsedColumns = new AnyColumnCollection(DataGrid, this.Style, this.CellStyle, this.AlternativeCellStyle);
			this.oExpandedColumns = new AnyColumnCollection(DataGrid, this.Style, this.CellStyle, this.AlternativeCellStyle);
		}
	}
}
