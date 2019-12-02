using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
using Ophelia.Web.View.Base.DataGrid;
namespace Ophelia.Web.View.Controls.DataGrid
{
	public class TabularGrid : Base.DataGrid.DataGrid
	{
		private int nVerticalCellCount = 1;
		private int nHorizontalCellCount = 1;
		private LayoutDirection eLayoutDirection = LayoutDirection.Horizantal;
		private Structure.Structure oCellLayout;
		private int nRadioButtonColumnIndex = -1;
		private int nCheckBoxColumnIndex = -1;
		public int CheckBoxColumnIndex {
			get { return this.nCheckBoxColumnIndex; }
			set {
				this.nCheckBoxColumnIndex = value;
				if (value > -1) {
					this.RadioButtonColumnIndex = -1;
				}
			}
		}
		public int RadioButtonColumnIndex {
			get { return this.nRadioButtonColumnIndex; }
			set {
				this.nRadioButtonColumnIndex = value;
				if (value > -1) {
					this.CheckBoxColumnIndex = -1;
				}
			}
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			Script.AddTrimFunction();
		}
		public string ValidateRadioOptionsOnScript(string Message)
		{
			return this.Script.ValidateRadioOption(this.ID + "RadioBox", Message);
		}
		public LayoutDirection LayoutDirection {
			get { return this.eLayoutDirection; }
			set { this.eLayoutDirection = value; }
		}
		public int VerticalCellCount {
			get {
				if (this.nVerticalCellCount < 1)
					return 1;
				return this.nVerticalCellCount;
			}
		}
		public int HorizontalCellCount {
			get {
				if (this.nHorizontalCellCount < 1)
					return 1;
				return this.nHorizontalCellCount;
			}
		}
		public Structure.Structure CellLayout {
			get { return this.oCellLayout; }
		}
		protected override void Customize(string Title)
		{
			base.Customize(Title);
		}
		public override void Bind()
		{
			this.BindingEndIndex = this.VerticalCellCount * this.HorizontalCellCount - 1;
			base.Bind();
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Bind();
			if (this.HasForm) {
				Content.Add("<div>");
				Content.Add("<form name=\"CB_" + this.ID + "Form\" ID=\"CB_" + this.ID + "Form\" method=\"POST\" action=\"" + this.Request.RawUrl + "\">");
			}
			Structure.Structure LayoutStructure = new Structure.Structure("CB_" + this.ID + "LayoutStructure", this.VerticalCellCount, this.HorizontalCellCount);
			Structure.Cells.Cell CellStructureCell = null;
			int DrawnColumnIndex = -1;
			int DrawnRowIndex = -1;
			int ColumnCountCounterOnDrawingRow = -1;
			bool ShowRadioBox = false;
			bool ShowCheckBox = false;
			for (int n = 0; n <= this.Rows.Count - 1; n++) {
				ShowCheckBox = false;
				ShowRadioBox = false;
				if (this.LayoutDirection == View.Web.LayoutDirection.Horizantal) {
					if ((n % this.HorizontalCellCount) == 0) {
						DrawnColumnIndex = 0;
						DrawnRowIndex += 1;
					} else {
						DrawnColumnIndex += 1;
					}
				} else {
					if ((n % this.VerticalCellCount) == 0) {
						DrawnRowIndex = 0;
						DrawnColumnIndex += 1;
					} else {
						DrawnRowIndex += 1;
					}
				}
				Structure.Structure TempCellStructure = this.CellLayout.CloneStructure();
				Base.DataGrid.Row Row = this.Rows(n);
				ColumnCountCounterOnDrawingRow = -1;
				this.OnBeforeRowDrawn(new RowEventArgs(Row));
				for (int i = 0; i <= this.CellLayout.Rows.Count - 1; i++) {
					for (int j = 0; j <= this.CellLayout.Columns.Count - 1; j++) {
						CellStructureCell = TempCellStructure.Rows(i).Cells(j, true);
						if (CellStructureCell.DependentCell == null) {
							ColumnCountCounterOnDrawingRow += 1;
							if (ColumnCountCounterOnDrawingRow == this.CheckBoxColumnIndex && !ShowCheckBox) {
								CheckBox CheckBox = new CheckBox(this.Rows(i).Cells(j).ID + "CheckBox");
								this.ConfigureSubControls(CheckBox);
								ShowCheckBox = true;
								CheckBox.Value = Row.ItemID;
								if (this.Rows(n).IsSelected) {
									CheckBox.Value = true;
								}
								CheckBox.OnClickEvent = Row.SelectedBehavior;
								CellStructureCell.Content.Add(CheckBox.Draw);
								ColumnCountCounterOnDrawingRow -= 1;
								continue;
							} else if (ColumnCountCounterOnDrawingRow == this.RadioButtonColumnIndex && !ShowRadioBox) {
								RadioBox RadioBox = new RadioBox(this.ID + "RadioBox");
								this.ConfigureSubControls(RadioBox);
								RadioBox.Value = Row.ItemID;
								ShowRadioBox = true;
								if (this.Rows(n).IsSelected) {
									RadioBox.Checked = true;
								}
								RadioBox.OnClickEvent = Row.SelectedBehavior;
								CellStructureCell.Content.Add(RadioBox.Draw);
								ColumnCountCounterOnDrawingRow -= 1;
								continue;
							}
							if (Row.Cells.Count <= ColumnCountCounterOnDrawingRow) {
								continue;
							}
							Row.Cells(ColumnCountCounterOnDrawingRow).DataControl.ID = Row.Cells(ColumnCountCounterOnDrawingRow).ID;
							Row.Cells(ColumnCountCounterOnDrawingRow).DataControl.Value = Row.Cells(ColumnCountCounterOnDrawingRow).Text;
							Row.Cells(ColumnCountCounterOnDrawingRow).DataControl.ReadOnly = this.ReadOnly;
							Row.Cells(ColumnCountCounterOnDrawingRow).DataControl.Validators.ValidationScript = this.Script;
							this.ConfigureSubControls(Row.Cells(ColumnCountCounterOnDrawingRow).DataControl);
							CellStructureCell.Content.Add(Row.Cells(ColumnCountCounterOnDrawingRow).DataControl.Draw());
						}
					}
				}
				LayoutStructure.Rows(DrawnRowIndex).Cells(DrawnColumnIndex).Controls.Add(TempCellStructure);
				LayoutStructure.Rows(DrawnRowIndex).Cells(DrawnColumnIndex).SetStyle(this.Rows.RowStyle);
			}
			this.ConfigureSubControls(LayoutStructure);
			Content.Add(LayoutStructure.Draw);
			if (this.HasForm) {
				Content.Add("</form>");
				Content.Add("</div>");
			}
		}
		private void SetConfiguration(int VerticalCellCount, int HorizontalCellCount, LayoutDirection LayoutDirection, int CellLayoutRowCount, int CellLayoutColumnCount)
		{
			this.nVerticalCellCount = VerticalCellCount;
			this.nHorizontalCellCount = HorizontalCellCount;
			this.eLayoutDirection = LayoutDirection;
			this.oCellLayout = new Structure.Structure("CellLayout", CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(string Title, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Title, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(string Title, string MemberName, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Title, MemberName, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(object Data, string MemberName, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Data, MemberName, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(object Data, string Title, string MemberName, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Data, Title, MemberName, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(string Title, ICollection Data, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Title, Data, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
		public TabularGrid(ICollection Data, string ID, int VerticalCellCount = 1, int HorizontalCellCount = 1, LayoutDirection LayoutDirection = View.Web.LayoutDirection.Horizantal, int CellLayoutRowCount = 1, int CellLayoutColumnCount = 1) : base(Data, ID)
		{
			this.SetConfiguration(VerticalCellCount, HorizontalCellCount, LayoutDirection, CellLayoutColumnCount, CellLayoutRowCount);
		}
	}
}
