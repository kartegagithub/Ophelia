using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class InfinityNumberBoxColumn : NumberBoxColumn
	{
		private string sNonNumericMessage = "";
		public string NonNumericMessage {
			get { return this.sNonNumericMessage; }
		}
		public new Ophelia.Web.View.Controls.NumberBox DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			InfinityNumberBoxCell NumberBoxCell = new InfinityNumberBoxCell(Row, this);
			NumberBoxCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(NumberBoxCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			if (string.IsNullOrEmpty(this.sNonNumericMessage)) {
				sNonNumericMessage = "Sayısal değer giriniz.";
			}
			this.oDataControl = new InfinityNumberBox(this.MemberName, sNonNumericMessage);
			this.oDataControl.Style.Width = this.Style.Width;
		}
		public InfinityNumberBoxColumn(ColumnCollection ColumnCollection, string Name, string MemberName, string Message) : base(ColumnCollection, Name, MemberName)
		{
			sNonNumericMessage = Message;
		}
	}
}
