using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Base.DataGrid
{
	public class LinkColumn : Column
	{
		public new Ophelia.Web.View.Controls.Link DataControl {
			get { return base.DataControl; }
		}
		public override Cell CreateCell(Row Row)
		{
			LinkCell LinkCell = new LinkCell(Row, this);
			LinkCell.ReadOnly = this.ReadOnly;
			return this.Cells.Add(LinkCell);
		}
		protected override void SetDataControl()
		{
			base.SetDataControl();
			this.oDataControl = new Link(this.MemberName);
		}
		public override bool LinkForEdit(string Url, string EditLinkMemberName = "")
		{
			this.DataControl.Url = Url;
			return base.LinkForEdit(Url, EditLinkMemberName);
		}
		public LinkColumn(ColumnCollection ColumnCollection, string Name, string MemberName) : base(ColumnCollection, Name, MemberName)
		{
		}
	}
}
