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
	public class CheckBoxList : InputDataControl
	{
		private CheckBoxOptionCollection oCheckboxes = new CheckBoxOptionCollection(this);
		private Ophelia.Web.View.Base.DataGrid.Row SelectedRow;
		private PropertyInfo oValueMemberPropertyInfo;
		private PropertyInfo oDisplayMemberPropertyInfo;
		private byte eDirection = 1;
		private ICollection oCollection;
		private object oSelectedValue;
		private string sDisplayMember = "Name";
		private string sValueMember = "ID";
		private string sSelectedItemValueMember = "ID";
		private DataGrid.DataGrid oDataGrid;
		private DataGrid.DataGrid oSelectedItemsDataGrid;
		private bool bShortenControlWhenCollectionIsLarge = true;
		private int iTextCharacterSize = -1;
		public string TextCharacterSize {
			get { return this.iTextCharacterSize; }
			set { this.iTextCharacterSize = value; }
		}
		public bool ShortenControlWhenCollectionIsLarge {
			get { return this.bShortenControlWhenCollectionIsLarge; }
			set { this.bShortenControlWhenCollectionIsLarge = value; }
		}
		public string ValueMember {
			get { return this.sValueMember; }
			set {
				if (!string.IsNullOrEmpty(value)) {
					this.sValueMember = value;
					this.DataGrid.ItemIDProperty = ValueMember;
				}
			}
		}
		public string SelectedItemValueMember {
			get { return this.sSelectedItemValueMember; }
			set {
				if (!string.IsNullOrEmpty(value)) {
					this.sSelectedItemValueMember = value;
					this.SelectedItemsDataGrid.ItemIDProperty = value;
				}
			}
		}
		public CheckBoxDirection Direction {
			get { return this.eDirection; }
			set { this.eDirection = value; }
		}
		internal DataGrid.DataGrid DataGrid {
			get {
				if (oDataGrid == null) {
					oDataGrid = new DataGrid.DataGrid(this.ID + "_SelectBox_DataGrid");
					oDataGrid.Columns.Add(this.DisplayMember);
				}
				return this.oDataGrid;
			}
		}
		internal DataGrid.DataGrid SelectedItemsDataGrid {
			get {
				if (oSelectedItemsDataGrid == null) {
					oSelectedItemsDataGrid = new DataGrid.DataGrid(this.ID + "_SelectBox_SelectedItemsDataGrid");
					oSelectedItemsDataGrid.Columns.Add(this.DisplayMember);
				}
				return this.oSelectedItemsDataGrid;
			}
		}
		public ICollection SelectedItems {
			get { return this.SelectedItemsDataGrid.Binding.Value; }
			set { this.SelectedItemsDataGrid.Binding.Value = value; }
		}
		public string DisplayMember {
			get { return this.sDisplayMember; }
			set {
				if (value != this.sDisplayMember && (this.DataGrid != null) && (this.DataGrid.Columns("Name") != null)) {
					this.sDisplayMember = value;
					this.DataGrid.Columns("Name").MemberName = value;
					if (this.DataGrid.BindState == BinderState.Binded) {
						this.DataGrid.Rebind();
					}
				} else {
					this.sDisplayMember = value;
				}
			}
		}
		public ICollection Collection {
			get { return this.DataGrid.Binding.Value; }
			set { this.DataGrid.Binding.Value = value; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Clear();
			if (this.Collection != null) {
				this.Options.Clear();
				if (this.DataGrid.BindState == BinderState.Pending)
					this.DataGrid.Bind();
				if (this.SelectedItemsDataGrid.BindState == BinderState.Pending)
					this.SelectedItemsDataGrid.Bind();
				for (int i = 0; i <= this.DataGrid.Rows.Count - 1; i++) {
					this.Options.Add(this.DataGrid.Rows(i).ItemID.ToString(), this.DataGrid.Rows(i).Cells(this.DisplayMember).Text());
					this.Options(i).TextCharacterSize = this.TextCharacterSize;
					this.Options(i).SetStyle(OptionStyle);
					this.Options(i).Disabled = this.Disabled;
					this.Options(i).ReadOnly = this.ReadOnly;
					this.Options(i).Checked = false;
					for (int k = 0; k <= this.Attributes.Count - 1; k++) {
						this.Options(i).Attributes.Add(this.Attributes.Keys(k).ToString(), this.Attributes.Values(k).ToString());
					}
					for (int j = 0; j <= this.SelectedItemsDataGrid.Rows.Count - 1; j++) {
						if (this.DataGrid.Rows(i).ItemID == this.SelectedItemsDataGrid.Rows(j).ItemID) {
							this.Options(i).Checked = true;
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				}
				if (ShortenControlWhenCollectionIsLarge && this.DataGrid.Rows.Count > 10) {
					if (this.Style.Height == int.MinValue) {
						this.Style.Height = 100;
					}
					this.Style.OverflowY = OverflowType.Auto;
				}
			}
			string SelectedItemIDs = "";
			for (int i = 0; i <= this.SelectedItemsDataGrid.Rows.Count - 1; i++) {
				if (SelectedItemIDs.Length > 0)
					SelectedItemIDs += ",";
				SelectedItemIDs += this.SelectedItemsDataGrid.Rows(i).ItemID;
			}
			Label Label = new Label(this.ID + "_Container");
			HiddenBox HiddenCheckBox = new HiddenBox(this.ID);
			//Ajax requestlerde DecideElementValueScript metodunun checkbox'ı yakalayabilmesi için ID'si benimle aynı olan input gerekli.
			HiddenCheckBox.Style.Display = DisplayMethod.Hidden;
			HiddenCheckBox.Value = SelectedItemIDs;
			Label.CloneEventsFrom(this);
			Label.SetStyle(this.Style);
			this.DrawEvents(Content);
			Label.Value = HiddenCheckBox.Draw + this.Options.Draw;
			Content.Add(Label.Draw);
		}

		protected override void DrawEvents(Content Content)
		{
		}
		public CheckBoxOptionCollection Options {
			get { return this.oCheckboxes; }
		}
		public Style OptionStyle {
			get { return this.Options.OptionStyle; }
		}
		public CheckBoxList(string MemberName)
		{
			this.ID = MemberName;
		}
		public enum CheckBoxDirection : byte
		{
			Horizontal = 0,
			Vertical = 1
		}
	}
}

