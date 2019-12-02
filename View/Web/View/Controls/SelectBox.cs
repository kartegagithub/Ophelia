using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Ophelia.Web.View.Forms;
using Ophelia.Web.View.Controls.DataGrid;
namespace Ophelia.Web.View.Controls
{
	public class SelectBox : InputDataControl
	{
		private bool bCreateBlankOption = true;
		private OptionCollection oOptions = new OptionCollection(this);
		private string sMessage = "";
		private string BlankOptionText = "";
		private DataGrid.DataGrid oDataGrid;
		private string sDisplayMember = "Name";
		private string sValueMember = "ID";
		private PropertyInfo oDisplayMemberPropertyInfo;
		private PropertyInfo oValueMemberPropertyInfo;
		internal Ophelia.Web.View.Base.DataGrid.Row SelectedRow;
		private bool bDisable = false;
		private bool bMultiple = false;
		public bool Disable {
			get { return this.bDisable; }
			set { this.bDisable = value; }
		}
		public bool Multiple {
			get { return this.bMultiple; }
			set { this.bMultiple = value; }
		}
		public DataGrid.DataGrid DataGrid {
			get {
				if (oDataGrid == null) {
					oDataGrid = new DataGrid.DataGrid(this.ID + "_SelectBox_DataGrid");
					oDataGrid.Columns.Add(this.DisplayMember);
				}
				return this.oDataGrid;
			}
		}
		public virtual ICollection DataSource {
			get { return this.DataGrid.Binding.Value; }
			set { this.DataGrid.Bind(value); }
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
		private PropertyInfo DisplayMemberPropertyInfo {
			get {
				if (this.oDisplayMemberPropertyInfo == null && (this.SelectedItem != null)) {
					this.oDisplayMemberPropertyInfo = this.SelectedItem.GetType().GetProperty(this.DisplayMember);
				}
				return this.oDisplayMemberPropertyInfo;
			}
			set { this.oDisplayMemberPropertyInfo = value; }
		}
		private PropertyInfo ValueMemberPropertyInfo {
			get {
				if (this.oValueMemberPropertyInfo == null && (this.SelectedItem != null)) {
					this.oValueMemberPropertyInfo = this.SelectedItem.GetType().GetProperty(this.ValueMember);
				}
				return this.oValueMemberPropertyInfo;
			}
			set { this.oValueMemberPropertyInfo = value; }
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
		public int SelectedIndex {
			get {
				if ((this.SelectedRow != null)) {
					return this.SelectedRow.Index;
				} else {
					return -1;
				}
			}
			set { this.SelectedItem = (object)this.DataSource(value); }
		}
		public object SelectedValue {
			get {
				if ((this.SelectedItem != null)) {
					return this.ValueMemberPropertyInfo.GetValue(this.SelectedItem, null);
				}
				return null;
			}
			set {
				this.SelectedRow = null;
				if ((value != null)) {
					Ophelia.Web.View.Base.DataGrid.Row Row = null;
					foreach ( Row in this.DataGrid.Rows) {
						if (Row.ItemID == value) {
							if (this.DisplayMember.IndexOf(".") == -1) {
								if (this.oDisplayMemberPropertyInfo == null) {
									this.oDisplayMemberPropertyInfo = Row.Item.GetType.GetProperty(this.DisplayMember);
								}
								base.Value = this.DisplayMemberPropertyInfo.GetValue(Row.Item, null);
							} else {
								View.Base.Binders.Binding Binding = new View.Base.Binders.Binding();
								Binding.Item = Row.Item;
								Binding.MemberName = this.DisplayMember;
								base.Value = Binding.Value;
							}
							this.SelectedRow = Row;
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				} else {
					base.Value = "";
				}
			}
		}
		public object SelectedItem {
			get {
				if ((this.SelectedRow != null)) {
					return this.SelectedRow.Item;
				}
				return null;
			}
			set {
				if ((value != null) && value.GetType().IsClass) {
					if (this.oValueMemberPropertyInfo == null && (value != null)) {
						this.oValueMemberPropertyInfo = value.GetType().GetProperty(this.ValueMember);
					}
					this.SelectedValue = this.ValueMemberPropertyInfo.GetValue(value, null);
				} else {
					this.SelectedValue = value;
				}
			}
		}
		public bool CreateBlankOption {
			get { return this.bCreateBlankOption; }
			set { this.bCreateBlankOption = value; }
		}
		public string Message {
			get { return this.sMessage; }
			set { this.sMessage = value; }
		}
		public override string Value {
			get { return base.Value; }
			set {
				base.Value = value;
				this.SelectedRow = null;
				Ophelia.Web.View.Base.DataGrid.Row Row = null;
				foreach ( Row in this.DataGrid.Rows) {
					if (this.oValueMemberPropertyInfo == null) {
						this.oValueMemberPropertyInfo = Row.Item.GetType.GetProperty(this.ValueMember);
					}
					if (Convert.ToString(this.ValueMemberPropertyInfo.GetValue(Row.Item, null)) == Convert.ToString(value)) {
						if (this.oDisplayMemberPropertyInfo == null) {
							this.oDisplayMemberPropertyInfo = Row.Item.GetType.GetProperty(this.DisplayMember);
						}
						base.Value = this.DisplayMemberPropertyInfo.GetValue(Row.Item, null);
						this.SelectedRow = Row;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public OptionCollection Options {
			get { return this.oOptions; }
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			if (this.DataSource != null) {
				this.Options.Clear();
				this.DataGrid.Bind();
				this.Options.SelectedValue = "";
				for (int i = 0; i <= this.DataGrid.Rows.Count - 1; i++) {
					this.Options.Add(this.DataGrid.Rows(i).ItemID.ToString(), this.DataGrid.Rows(i).Cells(this.DisplayMember).Text());
					if (this.SelectedRow != null) {
						int result = 0;
						if (int.TryParse(this.DataGrid.Rows(i).ItemID, out result)) {
							if (this.DataGrid.Rows(i).ItemID > 0) {
								if (this.DataGrid.Rows(i).ItemID == this.SelectedRow.ItemID) {
									Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
								}
							} else {
								if (object.ReferenceEquals(this.DataGrid.Rows(i), this.SelectedRow)) {
									Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
								}
							}
						} else {
							if (!string.IsNullOrEmpty(this.DataGrid.Rows(i).ItemID)) {
								if (this.DataGrid.Rows(i).ItemID == this.SelectedRow.ItemID) {
									Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
								}
							} else {
								if (object.ReferenceEquals(this.DataGrid.Rows(i), this.SelectedRow)) {
									Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
								}
							}
						}
					}
				}
			} else {
				if (!string.IsNullOrEmpty(this.Value)) {
					this.Options.SelectedValue = this.Value;
				}
			}
			if (string.IsNullOrEmpty(this.ID))
				this.ID = "SelectboxControl";
			if (this.ReadOnly) {
				if (this.Options.SelectedOption == null) {
					Label OptionLabel = new Label("SelectedOption");
					OptionLabel.SetStyle(Style);
					Content.Add(OptionLabel.Draw);
				} else {
					Label OptionLabel = new Label("SelectedOption");
					OptionLabel.Value = this.Options.SelectedOption.Text;
					OptionLabel.Style.Top = 3;
					OptionLabel.SetStyle(Style);
					Content.Add(OptionLabel.Draw);
					HiddenBox SelectedValue = new HiddenBox(this.ID);
					SelectedValue.Value = this.Options.SelectedOption.Value;
					Content.Add(SelectedValue.Draw);
				}
			} else {
				Content.Add("<select name=\"" + this.Name + "\"");
				Content.Add(" id=\"" + this.ID + "\"");
				Content.Add(Style.Draw);
				if (this.oAttributes != null) {
					for (int i = 0; i <= this.Attributes.Count - 1; i++) {
						Content.Add(" " + this.Attributes.Keys(i).ToString() + "=\"" + this.Attributes.Values(i).ToString() + "\"");
					}
				}
				this.DrawEvents(Content);
				if (this.Disable)
					Content.Add(" disabled=\"disabled\"");
				if (this.Multiple)
					Content.Add(" multiple");
				Content.Add(">");
				Content.Add(this.Options.Draw);
				Content.Add("</select>");
			}
		}
		public SelectBox(string Name, string Value) : this(Name)
		{
			this.Value = Value;
		}
		public SelectBox(string Name)
		{
			this.ID = Name;
		}
	}
}
