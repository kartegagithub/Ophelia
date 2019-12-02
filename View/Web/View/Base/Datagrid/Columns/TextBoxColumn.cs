using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class TextBoxColumn : Column
	{
		public new Ophelia.Web.View.Controls.TextBox DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			TextBoxCell TextBoxCell = new TextBoxCell(Row, this);
			TextBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(TextBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new TextBox(this.MemberName);
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public TextBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}
