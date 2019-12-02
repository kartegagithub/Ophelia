using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Forms
{
	public class Grid
	{
		private BaseForm oForm;
		private TitleConfiguration oTitles = new TitleConfiguration();
		private LinkConfiguration oLinks = new LinkConfiguration();
		private Ophelia.Application.Base.CollectionBase oRows;
		private Ophelia.Application.Base.CollectionBase oColumns;
		private Hashtable oSectionTable = new Hashtable();
		public Section Sections {
			get { return this.oSectionTable[Row + "-" + Column]; }
		}
		public TitleConfiguration Titles {
			get { return this.oTitles; }
		}
		public LinkConfiguration Links {
			get { return this.oLinks; }
		}
		public new BaseForm Form {
			get { return this.oForm; }
		}
		public Ophelia.Application.Base.CollectionBase Rows {
			get { return this.oRows; }
		}
		public Ophelia.Application.Base.CollectionBase Columns {
			get { return this.oColumns; }
		}
		public void AddColumn(string ColumnName)
		{
			this.oColumns.Insert(this.oColumns.Count, ColumnName);
		}
		public void SetGridSection(string Row, string Column, Section Section)
		{
			this.oSectionTable[Row + "-" + Column] = Section;
		}
		public void AddRow()
		{
			this.oRows.Insert(this.oRows.Count, this.oRows.Count + 1);
		}
		public Grid(BaseForm Form)
		{
			this.oForm = Form;
			this.oRows = new Ophelia.Application.Base.CollectionBase();
			this.oColumns = new Ophelia.Application.Base.CollectionBase();
		}
	}
}
