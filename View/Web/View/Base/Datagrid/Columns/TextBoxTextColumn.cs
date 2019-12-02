using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class TextBoxTextColumn : Column
	{
		public new Ophelia.Web.View.Controls.TextBoxText DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			TextBoxTextCell TextBoxTextCell = new TextBoxTextCell(Row, this);
			TextBoxTextCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(TextBoxTextCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new TextBoxText(this.MemberName);
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public TextBoxTextColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}
