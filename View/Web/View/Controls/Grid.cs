using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
using System.Reflection;
namespace Ophelia.Web.View.Controls
{
	public class Grid : InputDataControl
	{
		private ICollection oCollection;
		private string sDisplayMember = "Name";
		private string sValueMember = "ID";
		private DataGrid.DataGrid oBinder;
		public string ValueMember {
			get { return this.sValueMember; }
			set {
				if (!string.IsNullOrEmpty(value)) {
					this.sValueMember = value;
					this.Binder.ItemIDProperty = ValueMember;
				}
			}
		}
		public Ophelia.Web.View.Base.DataGrid.ColumnCollection Columns {
			get { return this.Binder.Columns; }
		}
		public DataGrid.DataGrid Binder {
			get {
				if (oBinder == null) {
					oBinder = new Binders.CollectionBinder(this.ID + "_Grid_Binder");
				}
				return this.oBinder;
			}
		}
		public string DisplayMember {
			get { return this.sDisplayMember; }
			set {
				if (value != this.sDisplayMember && (this.Binder != null) && (this.Binder.Columns("Name") != null)) {
					this.sDisplayMember = value;
					this.Binder.Columns("Name").MemberName = value;
					if (this.Binder.BindState == BinderState.Binded) {
						this.Binder.Rebind();
					}
				} else {
					this.sDisplayMember = value;
				}
			}
		}
		public ICollection Collection {
			get { return this.Binder.Binding.Value; }
			set { this.Binder.Binding.Value = value; }
		}
		private void BinderRowDraw(object Sender, ref Ophelia.Web.View.Base.DataGrid.RowEventArgs e)
		{
			foreach (Ophelia.Web.View.Base.DataGrid.Cell cell in e.Row.Cells) {
				if (cell.DataControl.Attributes("ItemID") != null) {
					cell.DataControl.Attributes("ItemID") = e.Row.ItemID;
				} else {
					cell.DataControl.Attributes.Add("ItemID", e.Row.ItemID);
				}
				if (object.ReferenceEquals(e.Row, this.Binder.ExtraRow) && (e.Row.Cell(0) != null && !e.Row.Cell(0).ReadOnly)) {
					this.Script.AppendLine("document.getElementById('" + e.Row.Cell(0).ID + "').focus();");
				}
			}
			//If e.Row.Cells(0).DataControl.Attributes("ItemID") IsNot Nothing Then
			//    e.Row.Cells(0).DataControl.Attributes("ItemID") = e.Row.ItemID
			//Else
			//    e.Row.Cells(0).DataControl.Attributes.Add("ItemID", e.Row.ItemID)
			//End If
			//If e.Row Is Me.Binder.ExtraRow Then
			//    Me.Script.AppendLine("document.getElementById('" & "CB_" & Me.Binder.ID & "Form_Row_ID_" & e.Row.ItemID & "_CB_" & Me.Binder.ID & "Form_Column_" & e.Row.Cells(0).Column.MemberName.Replace(".", "") & "').focus();")
			//End If
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Clear();
			//Me.Binder.HasForm = True
			//Me.Binder.Readonly = False
			//Me.Binder.Rows.AllowNew = True
			//Me.Binder.Rows.AllowEdit = True
			this.Binder.HasForm = false;
			//Me.Binder.Columns.LastColumn.Cells.ReadOnly = False
			//Me.Binder.Columns.LastColumn.ReadOnly = False
			this.Binder.BeforeRowDrawn += BinderRowDraw;
			if (this.Collection != null) {
				if (this.Binder.BindState == BinderState.Pending)
					this.Binder.Bind();
			}
			Label ContainerDiv = new Label(this.ID + "_Container");
			ContainerDiv.CloneEventsFrom(this);
			ContainerDiv.SetStyle(this.Style);

			//Dim ChangedColumnValues As New Panel(Me.ID & "_Grid_ChangedValuesContainer")
			//ChangedColumnValues.Controls.AddHiddenBox(Me.Binder.ID & "_ChangedValue_RowID", "")
			//For i As Integer = 0 To Me.Binder.Columns.Count - 1
			//    ChangedColumnValues.Controls.AddHiddenBox(Me.Binder.ID & "_ChangedValue_" & Me.Binder.Columns(i).MemberName, "")
			//Next
			//ContainerDiv.Value &= ChangedColumnValues.Draw()

			this.DrawEvents(Content);
			ContainerDiv.Value = this.Binder.Draw();
			Content.Add(ContainerDiv.Draw);
		}

		protected override void DrawEvents(Content Content)
		{
		}
		public Grid(string MemberName)
		{
			this.ID = MemberName;
		}
	}
}

