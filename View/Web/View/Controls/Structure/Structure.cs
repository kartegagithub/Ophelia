using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.Structure.Columns;
using Ophelia.Web.View.Controls.Structure.Rows;
using Ophelia.Web.View.Controls.Structure.Cells;
namespace Ophelia.Web.View.Controls.Structure
{
	public class Structure : ComplexWebControl
	{
		private ColumnCollection oColumns;
		private RowCollection oRows;
		private Style oStyle = new Style();
		private int nCellSpacing = 0;
		private int nCellPadding = 0;
		private LayoutTechnique eTechnique = LayoutTechnique.Tables;
		private Style oCellStyle;
		private bool bDrawnAsForm = false;
		private string OnSubmitFunction = string.Empty;
		private string Action = string.Empty;
		private bool bDrawFirstContainer = false;
		private bool bCanBeDrawnIDTag = true;
		private bool bIsMobilGrid = false;
		public bool IsMobilGrid {
			get { return this.bIsMobilGrid; }
			set { this.bIsMobilGrid = value; }
		}
		public bool DrawnAsForm {
			get { return this.bDrawnAsForm; }
			set { this.bDrawnAsForm = value; }
		}
		public bool CanBeDrawnIDTag {
			get { return this.bCanBeDrawnIDTag; }
			set { this.bCanBeDrawnIDTag = value; }
		}
		public LayoutTechnique Technique {
			get { return this.eTechnique; }
			set { this.eTechnique = value; }
		}
		public ColumnCollection Columns {
			get {
				if (this.oColumns == null) {
					this.oColumns = new ColumnCollection(this);
				}
				return this.oColumns;
			}
		}
		public RowCollection Rows {
			get {
				if (this.oRows == null) {
					this.oRows = new RowCollection(this);
				}
				return this.oRows;
			}
		}
		public Cell this[int Row, int Column] {
			get {
				if (Row > this.Rows.Count || Row < 0)
					return null;
				if (Column > this.Rows(Row).Cells.Count || Column < 0)
					return null;
				return this.Rows(Row)(Column);
			}
		}
		public int CellSpacing {
			get { return this.nCellSpacing; }
			set { this.nCellSpacing = value; }
		}
		public int CellPadding {
			get { return this.nCellPadding; }
			set { this.nCellPadding = value; }
		}
		public bool DrawFirstContainer {
			get { return this.bDrawFirstContainer; }
			set { this.bDrawFirstContainer = value; }
		}
		public Style CellStyle {
			get {
				if (this.oCellStyle == null)
					this.oCellStyle = new Style();
				return this.oCellStyle;
			}
		}
		public Structure CloneStructure()
		{
			Structure NewStructure = new Structure(this.ID, this.Rows.Count, this.Columns.Count);
			NewStructure.SetStyle(this.Style.Clone);
			for (int i = 0; i <= this.Rows.Count - 1; i++) {
				NewStructure.Rows(i).SetStyle(this.Rows(i).Style.Clone);
				for (int j = 0; j <= this.Columns.Count - 1; j++) {
					if (i == 0) {
						NewStructure.Columns(j).SetStyle(NewStructure.Columns(j).Style.Clone);
					}
					Cell MyCell = this.Rows(i).Cells(j, true);
					Cell NewCell = NewStructure.Rows(i).Cells(j, true);
					this.Equalize(ref NewCell, MyCell);
				}
			}
			return NewStructure;
		}
		private void Equalize(ref Cell NewCell, Cell ReferenceCell)
		{
			NewCell.ColumnSpan = ReferenceCell.ColumnSpan;
			NewCell.RowSpan = ReferenceCell.RowSpan;
			NewCell.SetStyle(ReferenceCell.Style.Clone());
			NewCell.Content.Add(ReferenceCell.Content.Value);
			NewCell.OnClickEvent = ReferenceCell.OnClickEvent;
			NewCell.VAlign = ReferenceCell.VAlign;
		}
		private string GetIDTag(string Suffix = "", bool IsTableID = false)
		{
			if (this.CanBeDrawnIDTag || (!string.IsNullOrEmpty(this.ID) && IsTableID)) {
				return " id=\"" + this.ID + Suffix + "\"";
			}
			return "";
		}
		public override void OnBeforeDraw(Content Content)
		{
			string BorderString = "";
			if (this.Style.Borders.Width > 0) {
				BorderString = this.Style.Draw(true, "border");
				BorderString = BorderString.Replace("border:", "").Replace(";", "");
				BorderString = "Border=\"" + BorderString + "\"";
			}
			if (!string.IsNullOrEmpty(this.ID))
				this.StyleSheet.AddIDBasedRule(this.ID + " td", this.CellStyle);

			if (this.DrawnAsForm) {
				Content.Add("<div" + this.GetIDTag("FContainer", true) + ">");
				Content.Add("<form name=\"" + this.ID + "Form\"" + this.GetIDTag("Form", true) + " " + this.Action + " " + this.OnSubmitFunction + " method=\"POST\">");
			}
			if (this.Technique == LayoutTechnique.Tables) {
				string WidthString = "";
				if (this.Style.Width > -1 || this.Style.WidthInPercent > -1 || this.Style.Dock == DockStyle.Width || this.Style.Dock == DockStyle.Fill) {
					if (this.Style.Width > -1) {
						WidthString = " width=\"" + this.Style.Width + "px\" ";
					} else if (this.Style.WidthInPercent > -1) {
						WidthString = " width=\"" + this.Style.WidthInPercent + "%\" ";
					}
					//Me.Style.WidthInPercent = -1
					//Me.Style.Width = -1
					if (this.Style.Dock == DockStyle.Fill) {
						WidthString = " width=\"100%\" ";
						this.Style.Dock = DockStyle.Height;
					} else if (this.Style.Dock == DockStyle.Width) {
						WidthString = " width=\"100%\" ";
						this.Style.Dock = DockStyle.None;
					}
				}
				//---------------------------------
				if (this.IsMobilGrid) {

					for (int i = 0; i <= this.Rows.Count - 1; i++) {
						Row CurrentRow = this.Rows(i);
						for (int j = 0; j <= this.Columns.Count - 1; j++) {
							Cell CurrentCell = CurrentRow.Cells(j, true);
							if (CurrentCell.DependentCell == null) {
								Content.Add(CurrentCell.Draw());
							}
						}
					}
				} else {
					Content.Add("<table").Add(this.GetIDTag("", true));
					Content.Add(" cellspacing=\"" + this.CellSpacing + "\" cellpadding=\"" + this.nCellPadding + "\"" + this.Style.Draw(, , "border,width") + " " + BorderString + WidthString + " >");
					for (int i = 0; i <= this.Rows.Count - 1; i++) {
						Row CurrentRow = this.Rows(i);
						Content.Add("<tr");
						if (!string.IsNullOrEmpty(this.ID))
							Content.Add(this.GetIDTag("_Row_" + i));
						Content.Add(CurrentRow.Style.Draw() + ">");
						if (this.CanBeDrawnIDTag)
							this.StyleSheet.AddIDBasedRule(this.ID + "_Row_" + i + " td", CurrentRow.CellStyle);

						for (int j = 0; j <= this.Columns.Count - 1; j++) {
							Cell CurrentCell = CurrentRow.Cells(j, true);
							if (CurrentCell.DependentCell == null) {
								this.ConfigureSubControls(CurrentCell);
								if (!string.IsNullOrEmpty(this.ID)) {
									if (CurrentCell.Style.IsCustomized) {
										this.StyleSheet.AddRule("td", CurrentCell.ID, "td", CurrentCell.Style, "font,alignment");
									} else {
										this.StyleSheet.AddRule("td", CurrentCell.ID, "td", CurrentCell.Column.Style, "font,alignment");
									}
								}

								Content.Add("<td");
								if (this.CanBeDrawnIDTag)
									Content.Add(" id=\"" + CurrentCell.ID + "\"");
								if (string.IsNullOrEmpty(CurrentCell.Content.Value) && CurrentCell.Style.Font.Size == -1 && CurrentCell.Controls.Count == 0)
									CurrentCell.Style.Font.Size = 1;
								if (CurrentCell.Style.IsCustomized)
									Content.Add(" " + CurrentCell.Style.Draw);
								else
									Content.Add(" " + CurrentCell.Column.Style.Draw);
								if (CurrentCell.ColumnSpan > 1)
									Content.Add(" colspan=\"" + CurrentCell.ColumnSpan + "\"");
								if (CurrentCell.RowSpan > 1)
									Content.Add(" rowspan=\"" + CurrentCell.RowSpan + "\"");
								if (CurrentCell.VAlign != Cells.Cell.VAlignEnum.None)
									Content.Add(" valign=\"" + CurrentCell.VAlign.ToString() + "\"");
								if (CurrentCell.Align != Cells.Cell.AlignEnum.None)
									Content.Add(" align=\"" + CurrentCell.Align.ToString() + "\"");
								if (!string.IsNullOrEmpty(CurrentCell.OnClickEvent))
									Content.Add(" onclick=\"" + CurrentCell.OnClickEvent + ";\"");
								Content.Add(" >").Add(CurrentCell.Draw()).Add("</td>");
							}
						}
						Content.Add("</tr>");
					}
					Content.Add("</table>");
				}


			} else if (this.Technique == LayoutTechnique.Css) {
				Content.Add("<div").Add(this.GetIDTag("", true));
				if (!string.IsNullOrEmpty(this.OnClickEvent))
					Content.Add(" onclick=\"" + this.OnClickEvent + "\"");
				Content.Add(this.Style.Draw(, , "border") + " " + BorderString + ">");
				for (int i = 0; i <= this.Rows.Count - 1; i++) {
					Row CurrentRow = this.Rows(i);
					//Ignore Row Div When Table Has One Row
					if ((this.Rows.Count > 1 && this.Columns.Count > 1) || this.bDrawFirstContainer) {
						Content.Add("<div").Add(this.GetIDTag("_Row_" + i));
						if (CurrentRow.Style.IsCustomized)
							Content.Add(" " + CurrentRow.Style.Draw());
						Content.Add(">");
						if (this.CanBeDrawnIDTag)
							this.StyleSheet.AddIDBasedRule(this.ID + "_Row_" + i + " div", this.Rows(i).CellStyle);
					// 1 X N Structure Row Style Rule
					} else if (this.Rows.Count == 1 && this.Columns.Count > 1 && !string.IsNullOrEmpty(this.ID) && this.CanBeDrawnIDTag) {
						this.StyleSheet.AddIDBasedRule(this.ID + " div", CurrentRow.Style);
						this.StyleSheet.AddIDBasedRule(this.ID + " div", CurrentRow.CellStyle);
					} else if (this.Rows.Count > 1 && this.Columns.Count == 1) {
						this.StyleSheet.AddClassBasedRule(this.ID + "_Row_" + i, CurrentRow.Style);
						this.StyleSheet.AddClassBasedRule(this.ID + "_Row_" + i, CurrentRow.CellStyle);
					}
					for (int j = 0; j <= this.Columns.Count - 1; j++) {
						Cell CurrentDependentCell = CurrentRow.Cells(j, true);
						Cell CurrentCell = CurrentRow.Cells(j);
						if (CurrentDependentCell.DependentCell == null) {
							Content.Add("<div");
							if (this.CanBeDrawnIDTag)
								Content.Add(" id=\"" + CurrentCell.ID + "\"");
							if (!string.IsNullOrEmpty(CurrentCell.OnClickEvent))
								Content.Add(" onclick=\"" + CurrentCell.OnClickEvent + "\"");
							if (this.Rows.Count > 1 && this.Rows.Count > 1) {
								if (!string.IsNullOrEmpty(CurrentCell.Style.Class))
									CurrentCell.Style.Class += " ";
								CurrentCell.Style.Class += this.ID + "_Row_" + i;
							}
							if (CurrentCell.Style.IsCustomized) {
								if (CurrentCell.Style.Width != int.MinValue) {
									if (CurrentCell.ColumnSpan > 1) {
										CurrentCell.Style.WidthInPercent = (100 / Columns.Count) * CurrentCell.ColumnSpan;
									} else {
										CurrentCell.Style.WidthInPercent = 100 / Columns.Count;
									}
								}
								Content.Add(" " + CurrentCell.Style.Draw);
							} else {
								if (CurrentCell.Style.Width != int.MinValue) {
									if (CurrentCell.ColumnSpan > 1) {
										CurrentRow.CellStyle.WidthInPercent = (100 / Columns.Count) * CurrentCell.ColumnSpan;
									} else {
										CurrentRow.CellStyle.WidthInPercent = 100 / Columns.Count;
									}

								}
								Content.Add(" " + CurrentCell.Column.Style.Draw);
							}
							Content.Add(" >").Add(CurrentCell.Draw()).Add("</div>");
						}
					}
					//Ignore Row Div When Table Has One Row
					if ((this.Rows.Count > 1 && this.Columns.Count > 1) || this.bDrawFirstContainer)
						Content.Add("</div>");
				}
				Content.Add("</div>");
			}
			if (this.DrawnAsForm)
				Content.Add("</form>").Add("</div>");
		}
		public void SetFormProperties(string OnSubmitFunction = "", string Action = "")
		{
			this.Action = string.IsNullOrEmpty(Action) ? "" : "action='" + Action + "'";
			this.OnSubmitFunction = string.IsNullOrEmpty(OnSubmitFunction) ? "" : "onsubmit='" + OnSubmitFunction + "'";
			this.DrawnAsForm = true;
		}
		public Structure(int RowsCount = 1, int ColumnsCount = 1, bool DrawAsForm = false) : this("", RowsCount, ColumnsCount, DrawAsForm, false)
		{
		}
		public Structure(string Name, int RowsCount = 1, int ColumnsCount = 1, bool DrawAsForm = false, bool CanBeDrawnIDTag = true, bool IsMobilGrid = false)
		{
			this.ID = Name;
			this.Columns.AddColumns(ColumnsCount);
			this.Rows.AddRows(RowsCount);
			this.bDrawnAsForm = DrawAsForm;
			this.CanBeDrawnIDTag = CanBeDrawnIDTag;
			this.IsMobilGrid = IsMobilGrid;
		}
	}
}
