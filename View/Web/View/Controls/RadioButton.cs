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
	public class RadioButton : InputDataControl
	{
		private RadioOptionCollection oOptions = new RadioOptionCollection(this);
		private Ophelia.Web.View.Base.DataGrid.Row SelectedRow;
		private PropertyInfo oValueMemberPropertyInfo;
		private PropertyInfo oDisplayMemberPropertyInfo;
		private byte eDirection = 1;
		private ICollection oCollection;
		private object oSelectedValue;
		private string sDisplayMember = "Name";
		private string sValueMember = "ID";
		private DataGrid.DataGrid oDataGrid;
		private string sBlankOptionMessage = "";
		private BlankOptionLocation eBlankOptionLocation = View.Web.Controls.BlankOptionLocation.Last;
		private bool bLabelCanBeClicked = false;
		private bool bUseOptionStyleOnLabel = false;
		public event BeforeOptionDrawnEventHandler BeforeOptionDrawn;
		public delegate void BeforeOptionDrawnEventHandler(object Sender, ref RadioOption e, object OptionItem);
		private int bOptionLabelsCharCount = 0;
		public int OptionLabelsCharCount {
			get { return this.bOptionLabelsCharCount; }
			set { this.bOptionLabelsCharCount = value; }
		}
		public bool UseOptionStyleOnLabel {
			get { return this.bUseOptionStyleOnLabel; }
			set { this.bUseOptionStyleOnLabel = value; }
		}
		public bool LabelCanBeClicked {
			get { return this.bLabelCanBeClicked; }
			set { this.bLabelCanBeClicked = value; }
		}
		public BlankOptionLocation BlankOptionLocation {
			get { return this.eBlankOptionLocation; }
			set { this.eBlankOptionLocation = value; }
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
		public RadioDirection Direction {
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
		private PropertyInfo DisplayMemberPropertyInfo {
			get {
				if (this.oDisplayMemberPropertyInfo == null && (this.SelectedItem != null)) {
					this.oDisplayMemberPropertyInfo = this.SelectedItem.GetType().GetProperty(this.DisplayMember);
				}
				return this.oDisplayMemberPropertyInfo;
			}
			set { this.oDisplayMemberPropertyInfo = value; }
		}
		public string BlankOptionMessage {
			get { return this.sBlankOptionMessage; }
			set { this.sBlankOptionMessage = value; }
		}
		public ICollection Collection {
			get { return this.DataGrid.Binding.Value; }
			set { this.DataGrid.Binding.Value = value; }
		}
		public object SelectedValue {
			get { return this.oSelectedValue; }
			set {
				this.SelectedRow = null;
				if ((value != null)) {
					Ophelia.Web.View.Base.DataGrid.Row Row = null;
					if (this.DataGrid.BindState == BinderState.Pending) {
						this.DataGrid.Bind();
					}
					foreach ( Row in this.DataGrid.Rows) {
						if (Row.ItemID == value) {
							if (this.oDisplayMemberPropertyInfo == null) {
								this.oDisplayMemberPropertyInfo = Row.Item.GetType.GetProperty(this.DisplayMember);
							}
							if (this.DisplayMemberPropertyInfo != null)
								base.Value = this.DisplayMemberPropertyInfo.GetValue(Row.Item, null);
							this.SelectedRow = Row;
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				} else {
					base.Value = "";
				}
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			Content.Clear();
			if (this.Collection != null) {
				this.Options.Clear();
				if (this.BlankOptionLocation == View.Web.Controls.BlankOptionLocation.First && !string.IsNullOrEmpty(this.BlankOptionMessage)) {
					this.Options.Add("-1", this.BlankOptionMessage);
				}
				if (this.DataGrid.BindState == BinderState.Pending)
					this.DataGrid.Bind();
				this.Options.SelectedValue = "";
				for (int i = 0; i <= this.DataGrid.Rows.Count - 1; i++) {
					if (OptionLabelsCharCount > 0) {
						this.Options.Add(this.DataGrid.Rows(i).ItemID.ToString(), Strings.Left(this.DataGrid.Rows(i).Cells(this.DisplayMember).Text(), OptionLabelsCharCount));
					} else {
						this.Options.Add(this.DataGrid.Rows(i).ItemID.ToString(), this.DataGrid.Rows(i).Cells(this.DisplayMember).Text());
					}
					this.Options(i).SetStyle(OptionStyle.Clone);
					this.Options(i).Disabled = this.Disabled;
					this.Options(i).ReadOnly = this.ReadOnly;
					this.Options(i).LabelCanBeClicked = this.LabelCanBeClicked;
					this.Options(i).UseOptionStyleOnLabel = this.UseOptionStyleOnLabel;
					if (object.ReferenceEquals(this.DataGrid.Rows(i), this.SelectedRow)) {
						Options.SelectedValue = this.DataGrid.Rows(i).ItemID.ToString();
					}
					if (BeforeOptionDrawn != null) {
						BeforeOptionDrawn(this, this.Options(i), this.DataGrid.Rows(i).Item);
					}
				}
				if (this.BlankOptionLocation == View.Web.Controls.BlankOptionLocation.Last && !string.IsNullOrEmpty(this.BlankOptionMessage)) {
					this.Options.Add("-1", this.BlankOptionMessage);
				}
			} else {
				this.Options.SelectedValue = this.Value;
			}
			if (this.ReadOnly) {
				if (this.Options.SelectedOption == null) {
					Content.Add("&nbsp;");
				} else {
					Label OptionLabel = new Label("SelectedOption");
					OptionLabel.Value = this.Options.SelectedOption.Text;
					OptionLabel.Style.Top = 3;
					Content.Add(OptionLabel.Draw);
					HiddenBox SelectedValue = new HiddenBox(this.ID);
					SelectedValue.Value = this.Options.SelectedOption.Value;
					Content.Add(SelectedValue.Draw);
				}
			} else {
				Label Label = new Label(this.ID + "_Container");
				RadioBox HiddenRadioBox = new RadioBox(this.ID);
				//Ajax requestlerde DecideElementValueScript metodunun radiobutton'ı yakalayabilmesi için ID'si benimle aynı olan input gerekli.
				HiddenRadioBox.Style.Display = DisplayMethod.Hidden;
				Label.CloneEventsFrom(this);
				Label.SetStyle(this.Style);
				if (this.oAttributes != null) {
					for (int i = 0; i <= this.Attributes.Count - 1; i++) {
						Label.Attributes.Add(this.Attributes.Keys(i).ToString(), this.Attributes.Values(i).ToString());
					}
				}
				this.DrawEvents(Content);
				Label.Value = HiddenRadioBox.Draw + this.Options.Draw;
				Content.Add(Label.Draw);
			}
		}

		protected override void DrawEvents(Content Content)
		{
		}
		public RadioOptionCollection Options {
			get { return this.oOptions; }
		}
		public Style OptionStyle {
			get { return this.Options.OptionStyle; }
		}
		public RadioButton(string MemberName)
		{
			this.ID = MemberName;
		}
	}
	public enum BlankOptionLocation : byte
	{
		Last = 0,
		First = 1
	}
	public enum RadioDirection : byte
	{
		Horizontal = 0,
		Vertical = 1
	}
}

