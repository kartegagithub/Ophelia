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
	public class DataGrid : Base.DataGrid.DataGrid
	{
		private Header oHeader;
		private Footer oFooter;
		private AjaxFunction oUpdateWithAjaxFunction;
		private bool bCheckBoxes = false;
		private bool bRadioBoxes = false;
		internal Hashtable BlankRows = new Hashtable();
		private bool bHideColumnsHeader = false;
		private bool bUpdateWithAjax = false;
		private bool bShowTotals = false;
		private bool bShowRowNumbers = false;
		private bool bHasDeleteRow = false;
		private Style oTotalsRowStyle;
		private Style oContainerStyle;
		private string oEditLinkImage;
		private int nDrawnTableColumnCount;
		private bool bAlwaysShowHeader = false;
		private DataGridData.TreeViewSelectionPattern eSelectionPattern = DataGridData.TreeViewSelectionPattern.BottomOfPath;
		private bool bRowHasData = false;
		private bool bAutoExpandAllNodes = false;
		private bool bRowContextMenuIsEnabled = false;
		public bool RowContextMenuIsEnabled {
			get { return this.bRowContextMenuIsEnabled; }
			set { this.bRowContextMenuIsEnabled = value; }
		}
		public bool AlwaysShowHeader {
			get { return this.bAlwaysShowHeader; }
			set { this.bAlwaysShowHeader = value; }
		}
		public bool AutoExpandAllNodes {
			get { return this.bAutoExpandAllNodes; }
			set { this.bAutoExpandAllNodes = value; }
		}
		public bool RowHasData {
			get { return this.bRowHasData; }
			set { this.bRowHasData = value; }
		}
		public bool ShowTotals {
			get { return this.bShowTotals; }
			set { this.bShowTotals = value; }
		}
		public bool ShowRowNumbers {
			get { return this.bShowRowNumbers; }
			set { this.bShowRowNumbers = value; }
		}
		public bool HideColumnsHeader {
			get { return this.bHideColumnsHeader; }
			set { this.bHideColumnsHeader = value; }
		}
		private bool HasDeleteRow {
			get { return this.bHasDeleteRow; }
			set { this.bHasDeleteRow = value; }
		}
		public bool UpdateWithAjax {
			get { return this.bUpdateWithAjax; }
			set { this.bUpdateWithAjax = value; }
		}
		public DataGridData.TreeViewSelectionPattern SelectionPattern {
			get { return this.eSelectionPattern; }
			set { this.eSelectionPattern = value; }
		}
		public new DataGridStyle Style {
			get { return base.Style; }
		}
		public Style ContainerStyle {
			get {
				if (this.oContainerStyle == null)
					this.oContainerStyle = new Style();
				return this.oContainerStyle;
			}
		}
		public Style TotalsRowStyle {
			get {
				if (this.oTotalsRowStyle == null)
					this.oTotalsRowStyle = new Style();
				return this.oTotalsRowStyle;
			}
		}

		protected virtual void BindAllDataForDocuments()
		{
		}
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			Script.AppendLine("function OnChangeValue(CellID,ChangedIDArea)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("var RowID = Trim(ChangedIDArea.value);" + Constants.vbCrLf);
			Script.AppendLine("if (RowID != ''){RowID += ',';}" + Constants.vbCrLf);
			Script.AppendLine("var b= CellID.id.replace('CheckBox','').split('_');");
			Script.AppendLine("var Found = false;");
			Script.AppendLine("var TempRowID = b[0] + '_' + b[1] + '_' + b[2] + '_' + b[3] + '_' + b[4];");
			Script.AppendLine("RowID += TempRowID;");
			Script.AppendLine("ChangedIDArea.value = RowID;" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);

			Script.AppendLine("function MarkChildControls(CellID,ChangedIDArea, Type)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("if (Type == '1') {" + Constants.vbCrLf);
			Script.AppendLine("var currentSortOrder = $('#' + CellID.id).attr('sortorder');" + Constants.vbCrLf);
			Script.AppendLine("for(i=0;i<=$('input:checkbox[sortorder^=' + currentSortOrder + ']').length-1;i++)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("if($('#' + $('input:checkbox[sortorder^=' + currentSortOrder + ']')[i].id).attr('sortorder').length > currentSortOrder.length)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("$('#' + $('input:checkbox[sortorder^=' + currentSortOrder + ']')[i].id).attr('checked',$('#' + CellID.id)[0].checked);" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AppendLine("for(k=0;k<=$('input:checkbox[sortorder^=' + currentSortOrder + ']').length-1;k++)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("if($('#' + $('input:checkbox[sortorder^=' + currentSortOrder + ']')[k].id).attr('sortorder').length > currentSortOrder.length)" + Constants.vbCrLf);
			Script.AppendLine("{" + Constants.vbCrLf);
			Script.AppendLine("OnChangeValue($('input:checkbox[sortorder^=' + currentSortOrder + ']')[k], ChangedIDArea);");
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AppendLine("}" + Constants.vbCrLf);
			Script.AddTrimFunction();
		}
		public AjaxFunction UpdateWithAjaxFunction {
			get {
				if (this.oUpdateWithAjaxFunction == null) {
					if (this.Script.Function("Save" + this.ID) == null) {
						this.oUpdateWithAjaxFunction = this.Script.AddAjaxFunction("Save" + this.ID, "", "", "", false, true);
					} else {
						this.oUpdateWithAjaxFunction = this.Script.Function("Save" + this.ID);
					}
				}
				return this.oUpdateWithAjaxFunction;
			}
		}
		public override Style CreateStyle()
		{
			return new DataGridStyle(this);
		}
		public Footer Footer {
			get { return this.oFooter; }
		}
		public Header Header {
			get { return this.oHeader; }
		}
		public int CurrentCount {
			get { return (this.ExtraRow == null ? this.Rows.Count : this.Rows.Count - 1); }
		}
		public bool CheckBoxes {
			get { return this.bCheckBoxes; }
			set { this.bCheckBoxes = value; }
		}
		public bool RadioBoxes {
			get { return this.bRadioBoxes; }
			set { this.bRadioBoxes = value; }
		}
		private void ConfigureCellStyle()
		{
			//IE7 Hacking
			if (this.Columns.CellStyle.PaddingRight == int.MinValue)
				this.Columns.CellStyle.PaddingRight = 10;
			if (this.Columns.CellStyle.PaddingLeft == int.MinValue)
				this.Columns.CellStyle.PaddingLeft = 3;
			if (this.Columns.AlternativeCellStyle.PaddingRight == int.MinValue)
				this.Columns.AlternativeCellStyle.PaddingRight = 10;
			if (this.Columns.AlternativeCellStyle.PaddingLeft == int.MinValue)
				this.Columns.AlternativeCellStyle.PaddingLeft = 3;
			if (this.Columns.Style.PaddingRight == int.MinValue)
				this.Columns.Style.PaddingRight = 10;
			if (this.Columns.Style.PaddingLeft == int.MinValue)
				this.Columns.Style.PaddingLeft = 3;
			//IE7 Hacking
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " INPUT", "Border:0;");
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " .EvenRow td ", this.Columns.CellStyle.Draw(true));
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " .EvenRow td a", this.Columns.CellStyle.Draw(true, "font"));
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " .OddRow td ", (this.Columns.AlternativeCellStyle.IsCustomized ? this.Columns.AlternativeCellStyle.Draw(true) : this.Columns.CellStyle.Draw(true)));
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " .OddRow td a", (this.Columns.AlternativeCellStyle.IsCustomized ? this.Columns.AlternativeCellStyle.Draw(true, "font") : this.Columns.CellStyle.Draw(true, "font")));
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " img", "padding-top:2px; padding-left:2px; border :0;");
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + "_Columns td", this.Columns.Style.Draw(true));
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + "_Footer td ", this.Footer.Style.Draw(true, , "border"));
			this.StyleSheet.AddCustomRule("TABLE#CB_" + this.ID + " td", "vertical-align:middle;");
			this.StyleSheet.AddCustomRule("#CB_" + this.ID + " .checkboxarea", "padding-left:3px;padding-right:3px;white-space:nowrap;");
		}
		internal override int DrawnTableColumnCount {
			get {
				int ColumnsCount = this.nDrawnTableColumnCount;
				if (this.Rows.AllowDelete)
					ColumnsCount += 1;
				if (this.ShowRowNumbers)
					ColumnsCount += 1;
				if (this.Rows.AllowEdit && !string.IsNullOrEmpty(this.Rows.EditLinkUrl))
					ColumnsCount += 1;
				return ColumnsCount;
			}
		}
		private void DrawColumns(Content Content)
		{
			if (!this.HideColumnsHeader) {
				dynamic CssClass = this.Columns.Style.Class;
				Content.Add("<colgroup>");
				if (!this.IsMobileGrid && this.HasRowEditButton)
					Content.Add("<col width=\"20px\"></col>");
				if (this.ShowRowNumbers)
					Content.Add("<col width=\"20px\" style=\"background-color:#cccccc\"></col>");
				for (int j = 0; j <= this.Columns.ExpandedColumns.Count - 1; j++) {
					Content.Add("<col ");
					if (!string.IsNullOrEmpty(CssClass)) {
						CssClass += " ";
					}
					CssClass += this.Columns.ExpandedColumns(j).Style.Class();
					this.Columns.ExpandedColumns(j).Style.HorizontalAlignment = this.Columns.ExpandedColumns(j).Style.HorizontalAlignment;
					if (j == this.Columns.Count - 1 && this.Rows.AllowDelete) {
						Content.Add(" colspan=\"2\" ");
					}
					if (this.Columns.ExpandedColumns(j).Sortable == true)
						this.Columns.ExpandedColumns(j).Style.Width += 20;
					Content.Add(this.Columns.ExpandedColumns(j).Style.Draw(false) + "></col>");
					this.Page.Header.StyleSheet.AddCustomRule("#CB_" + this.ID + " tr td#" + this.Columns.ExpandedColumns(j).ID, this.Columns.ExpandedColumns(j).CellStyle.Draw(true));
				}
				if (this.IsMobileGrid && this.HasRowEditButton)
					Content.Add("<col width=\"20px\"></col>");
				Content.Add("</colgroup>");
				string ColumnClass = "";
				if (this.Columns.Style.Class != string.Empty)
					ColumnClass = "class='" + this.Columns.Style.Class + "' ";
				if (this.IsMobileGrid == true)
					Content.Add("<thead>");
				Content.Add("<tr ID=\"CB_" + this.ID + "_Columns\" " + ColumnClass + ">");
				if (!this.IsMobileGrid && this.HasRowEditButton)
					Content.Add("<td>&nbsp;</td>");
				if (this.ShowRowNumbers)
					Content.Add("<td>&nbsp;</td>");
				int ColspanCount = 0;
				for (int j = 0; j <= this.Columns.ExpandedColumns.Count - 1; j++) {
					string ColumnCellClass = "";
					if (this.Columns.ExpandedColumns.CellStyle.Class != string.Empty)
						ColumnCellClass = "class='" + this.Columns.ExpandedColumns.CellStyle.Class + " " + this.Columns.ExpandedColumns(j).CellStyle.Class + "' ";
					Content.Add("<td  " + ColumnCellClass + this.Columns.ExpandedColumns(j).GetID);
					if (j == this.Columns.ExpandedColumns.Count - 1) {
						if (this.Rows.AllowDelete && this.IsMobileGrid && this.HasRowEditButton) {
							Content.Add(" colspan=\"3\" ");
						} else if (this.Rows.AllowDelete) {
							Content.Add(" colspan=\"2\" ");
						} else if (this.IsMobileGrid && this.HasRowEditButton) {
							Content.Add(" colspan=\"2\" ");
						}
					}
					Content.Add(" >");
					this.OnBeforeColumnDrawn(new ColumnEventArgs(this.Columns.ExpandedColumns(j)));
					Content.Add(this.Columns.ExpandedColumns(j).Name);
					if (this.Columns.ExpandedColumns(j).GetType.Name == "CheckBoxColumn" && ((CheckBoxColumn)this.Columns.ExpandedColumns(j)).AllowSelectAll) {
						Content.Add(((CheckBoxColumn)this.Columns.ExpandedColumns(j)).SelectAllControl.Draw);
					}
					Content.Add("</td>");
				}
				Content.Add("</tr>");
				if (this.IsMobileGrid == true)
					Content.Add("</thead>");
			} else {
				Content.Add("<colgroup>");
				for (int j = 0; j <= this.Columns.ExpandedColumns.Count - 1; j++) {
					dynamic CssClass = this.Columns.ExpandedColumns.Style.Class + " " + this.Columns.ExpandedColumns(j).Style.Class();
					Content.Add("<col " + this.Columns.ExpandedColumns(j).Style.Draw(false));
					Content.Add("></col>");
				}
				Content.Add("</colgroup>");
			}
		}
		public string EditLinkImage {
			get {
				if (oEditLinkImage == null) {
					oEditLinkImage = ServerSide.ImageDrawer.GetImageUrl("WebEdit");
				}
				return oEditLinkImage;
			}
			set { this.oEditLinkImage = value; }
		}
		private byte nIsMobileGrid = 0;
		public bool IsMobileGrid {
			get {
				if (this.nIsMobileGrid == 0) {
					if (this.Page.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Mobile.MobilePage"))) {
						this.nIsMobileGrid = 1;
					} else {
						this.nIsMobileGrid = 2;
					}
				}
				return this.nIsMobileGrid == 1;
			}
			set {
				if (value) {
					this.nIsMobileGrid = 1;
				} else {
					this.nIsMobileGrid = 2;
				}
			}
		}
		private void DrawRowEditButton(Row Row, Content Content, bool IsFirstColumn = true)
		{
			if (!this.IsMobileGrid && IsFirstColumn) {
				if (this.HasRowEditButton && (!object.ReferenceEquals(Row, this.ExtraRow))) {
					Link EditLink = new Link(Row.ID + "_EditLink");
					EditLink.Style.Class = "EditLink";
					EditLink.Url = Row.EditLinkUrl;
					this.ConfigureSubControls(EditLink);
					Image EditLinkImage = new Image("", this.EditLinkImage);
					this.ConfigureSubControls(EditLinkImage);
					EditLink.Value = EditLinkImage.Draw;
					Content.Add("<td>" + EditLink.Draw() + "</td>");
				} else if (this.HasRowEditButton) {
					Image NewLinkImage = new Image("", ServerSide.ImageDrawer.GetImageUrl("WebNew"));
					this.ConfigureSubControls(NewLinkImage);
					Content.Add("<td>" + NewLinkImage.Draw() + "</td>");
				}
			} else if (this.IsMobileGrid && !IsFirstColumn) {
				Link SelectLink = new Link(Row.ID + "_SelectLink");
				SelectLink.Style.Class = "SelectLink";
				SelectLink.Url = Row.EditLinkUrl;
				this.ConfigureSubControls(SelectLink);
				Image SelectLinkImage = new Image("", ServerSide.ImageDrawer.GetImageUrl("SelectItemIcon"));
				this.ConfigureSubControls(SelectLinkImage);
				SelectLink.Value = SelectLinkImage.Draw;
				Content.Add("<td>" + SelectLink.Draw() + "</td>");
			}
		}
		private bool IsTreeViewScriptsAdded = false;
		private void DrawGetTreeViewScripts(Content Content)
		{
			if (!this.IsTreeViewScriptsAdded) {
				IsTreeViewScriptsAdded = true;
				string ApplicationPath = "/";
				if (this.Page != null) {
					ApplicationPath = this.Page.ContentManager.ApplicationPath;
				}
				//to do: Page nothing gelmez. 
				// If Me.Page IsNot Nothing Then
				this.Page.ScriptManager.Add("jquery161min", UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jquery161min.js", this.Page.FileUsageType), 0, true);
				//Else
				//    Dim Script As New Script("jquery161min", Nothing)
				//    Script.Path = UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jquery161min.js", Me.Page.FileUsageType)
				//    Content.Add(Script.Draw())
				//End If
				Function TreeViewFunction = this.Script.AddFunction("ArrangeTreeViewVisibility", "", "ID,CssID,ForceToCollapse");
				TreeViewFunction.AppendLine("var i;");
				TreeViewFunction.AppendLine("var subElementCssID;");
				TreeViewFunction.AppendLine("var subElementID;");
				TreeViewFunction.AppendLine("var elementlength = $(CssID).length;");
				TreeViewFunction.AppendLine("if (elementlength > 0 && $(CssID)[0].style.visibility !='collapse')");
				TreeViewFunction.AppendLine("{");
				TreeViewFunction.AppendLine("document.getElementById(ID).src = '" + this.CollapseImageUrl + "';");
				TreeViewFunction.AppendLine("for (var i=0;i<elementlength;i++)");
				TreeViewFunction.AppendLine("{");
				TreeViewFunction.AppendLine("$(CssID)[i].style.visibility = 'collapse';");
				TreeViewFunction.AppendLine("$(CssID)[i].style.display = 'none';");
				TreeViewFunction.AppendLine("var classes = $(CssID)[i].className.split(' ')");

				TreeViewFunction.AppendLine("$(\"[class*='\" + CssID.replace('.','') +\"']\").hide();");
				TreeViewFunction.AppendLine("$(\"[class*='\" + CssID.replace('.','') +\"']\").css('visibility','collapse');");
				TreeViewFunction.AppendLine("$(\"[class*='\" + CssID.replace('.','') +\"'] img\").each(function(index){ ");
				TreeViewFunction.AppendLine("  if ($(this).parent().attr('class') =='checkboxarea'){ $(this).attr('src','" + this.CollapseImageUrl + "'); }");
				TreeViewFunction.AppendLine(" });");
				//TreeViewFunction.AppendLine("$(""[class*='"" + CssID.replace('.','') +""'] img"").attr('src','" & Me.CollapseImageUrl & "');")

				TreeViewFunction.AppendLine("var tempsortorder = classes[classes.length-1].replace('sov_','');");
				TreeViewFunction.AppendLine("var tempid = 'id_' + tempsortorder;");
				TreeViewFunction.AppendLine("var tempstyle= '.he' + tempsortorder;");
				TreeViewFunction.AppendLine("ArrangeTreeViewVisibility( tempid,tempstyle,true);");
				TreeViewFunction.AppendLine("}");
				TreeViewFunction.AppendLine("}");
				TreeViewFunction.AppendLine("else if (!ForceToCollapse)");
				TreeViewFunction.AppendLine("{");
				TreeViewFunction.AppendLine("document.getElementById(ID).src = '" + this.ExpandImageUrl + "';");
				TreeViewFunction.AppendLine("for (var i=0;i<elementlength;i++)");
				TreeViewFunction.AppendLine("{");
				TreeViewFunction.AppendLine("$(CssID)[i].style.visibility = 'visible';");
				TreeViewFunction.AppendLine("$(CssID)[i].style.display = '';");
				TreeViewFunction.AppendLine("if ((i+1)<10) {subElementCssID = CssID + '00' + (i+1).toString();subElementID = ID + '00' + (i+1).toString(); }");
				TreeViewFunction.AppendLine("else if ((i+1)<100) {subElementCssID = CssID + '0' + (i+1).toString();subElementID = ID + '0' + (i+1).toString();}");
				TreeViewFunction.AppendLine("else {subElementCssID = CssID  + (i+1).toString(); subElementID = ID +  (i+1).toString();}");
				TreeViewFunction.AppendLine("if ($(subElementCssID).length == 0)  {" + "document.getElementById(ID).src = '" + this.ExpandImageUrl + "';" + "}");
				TreeViewFunction.AppendLine("else  {" + "document.getElementById(ID).src = '" + this.ExpandImageUrl + "';" + "}");

				TreeViewFunction.AppendLine("}");
				TreeViewFunction.AppendLine("}");
			}
		}
		private string sExpandImageUrl = "";
		private string sCollapseImageUrl = "";
		public string ExpandImageUrl {
			get {
				if (string.IsNullOrEmpty(this.sExpandImageUrl))
					this.sExpandImageUrl = View.Web.Controls.ServerSide.ImageDrawer.GetImageUrl("Expand");
				return this.sExpandImageUrl;
			}
			set { this.sExpandImageUrl = value; }
		}
		public string CollapseImageUrl {
			get {
				if (string.IsNullOrEmpty(this.sCollapseImageUrl))
					this.sCollapseImageUrl = View.Web.Controls.ServerSide.ImageDrawer.GetImageUrl("Collapse");
				return this.sCollapseImageUrl;
			}
			set { this.sCollapseImageUrl = value; }
		}
		private string sMarkChildControlsType = "1";
		public string MarkChildControlsType {
			get { return sMarkChildControlsType; }
			set { sMarkChildControlsType = value; }
		}
		private bool DrawSelectionItem(Row Row, Content Content)
		{
			if (this.CheckBoxes || this.RadioBoxes || this.HierarchicDisplay) {
				int PaddingLeft = 0;
				PaddingLeft = (this.CalculateRankNumber(Row) == 1 ? 10 : (this.CalculateRankNumber(Row) + 1) * 12);
				Content.Add("<table style=\"width:100%;height:100%;\" class=\"SCCell\">");
				Content.Add("<tr>");
				if (this.HierarchicDisplay) {
					this.DrawGetTreeViewScripts(Content);
					Content.Add("<td");
					if (this.HasChildNode(Row)) {
						Content.Add(" width=\"10px\" class=\"checkboxarea\">");
						Controls.Image Image = new Controls.Image("", "");
						Image.Style.Left = PaddingLeft;
						Image.ImageSource = this.ExpandImageUrl;
						Image.Style.Padding = 0;
						Image.Style.PaddingTop = 3;
						Image.ID = "id_" + this.GetHierarchicalSortOrder(Row);
						Image.OnClickEvent = "ArrangeTreeViewVisibility('" + Image.ID + "','.he" + this.GetHierarchicalSortOrder(Row) + "',false);";
						Content.Add(Image.Draw);
						if (this.CalculateRankNumber(Row) == 1 && !this.AutoExpandAllNodes) {
							this.Script.AppendLine("$(document).ready(function() {ArrangeTreeViewVisibility('" + Image.ID + "','.he" + this.GetHierarchicalSortOrder(Row) + "',true);})");
						}
					} else {
						Content.Add(" width=\"10px\" class=\"checkboxarea checkboxleftarea\">");
						Controls.Image Image = new Controls.Image("", "");
						Image.Style.Opacity = 0;
						Image.Style.Filter = "alpha(opacity=0)";
						Image.Style.Left = PaddingLeft;
						Image.ImageSource = this.ExpandImageUrl;
						Image.Style.Padding = 0;
						Image.Style.PaddingTop = 3;
						Content.Add(Image.Draw);
						//Content.Add(" width=""" & IIf(Me.CalculateRankNumber(Row) = 1, 10, (Me.CalculateRankNumber(Row) + 1) * 12) & "px"" class=""checkboxarea checkboxleftarea"">&nbsp")
					}
					Content.Add("</td>");
				}
				if (this.CheckBoxes) {
					if (Row.CanBeSelected) {
						Content.Add("<td width=\"10px\" class=\"checkboxarea\">");
						CheckBox CheckBox = new CheckBox(Row.ID + "CheckBox");
						if (this.HierarchicDisplay) {
							CheckBox.SortOrder = this.GetHierarchicalSortOrder(Row);
						}
						if (Row.IsSelected) {
							CheckBox.Value = true;
						}
						CheckBox.OnClickEvent = "OnChangeValue(" + CheckBox.ID + ", CB_" + this.ID + "FormChangedItems);";
						if (this.HierarchicDisplay) {
							if (this.SelectionPattern != DataGridData.TreeViewSelectionPattern.None) {
								switch (this.SelectionPattern) {
									case DataGridData.TreeViewSelectionPattern.BottomOfPath:
										CheckBox.OnClickEvent += " MarkChildControls(" + CheckBox.ID + ", CB_" + this.ID + "FormChangedItems," + this.MarkChildControlsType + "); ";
										break;
									case DataGridData.TreeViewSelectionPattern.TopOfPath:
										break;
									//Geliştirme daha sonra yapılacaktır.
								}
							}
						}
						CheckBox.OnClickEvent += Row.SelectedBehavior;
						Content.Add(CheckBox.Draw());
						Content.Add("</td>");
					}
				} else if (this.RadioBoxes) {
					if (Row.CanBeSelected) {
						Content.Add("<td width=\"10px\" class=\"checkboxarea\">");
						RadioBox RadioBox = new RadioBox(this.ID + "RadioBox");
						if (this.HierarchicDisplay) {
							RadioBox.SortOrder = this.GetHierarchicalSortOrder(Row);
						}
						RadioBox.Value = Row.ItemID;
						if (Row.IsSelected) {
							RadioBox.Checked = true;
						}
						RadioBox.Style.PositionStyle = Position.Relative;
						RadioBox.OnClickEvent = "OnChangeValue(document.getElementById('" + RadioBox.ID + "'), CB_" + this.ID + "FormChangedItems);" + Row.SelectedBehavior;
						Content.Add(RadioBox.Draw());
						Content.Add("</td>");
					}
				}
				Content.Add("<td>");
				return true;
			}
			return false;
		}
		public override void Bind()
		{
			base.Bind();
		}
		private int iMinimumRankNumber = -1;
		private int CalculateRankNumber(Row Row)
		{
			int Length = this.GetHierarchicalSortOrder(Row).Length;
			if (iMinimumRankNumber == -1 || Length / 3 < iMinimumRankNumber) {
				this.iMinimumRankNumber = Length / 3;
			}
			return (Length / 3 - this.iMinimumRankNumber) + 1;
		}
		protected virtual string GetHierarchicalSortOrder(Row Row)
		{
			return 0;
		}
		protected bool HasChildNode(Row Row)
		{
			bool Result = false;
			if (Row.Collection.Item(Row.Index + 1) != null) {
				Result = this.GetHierarchicalSortOrder(Row.Collection.Item(Row.Index + 1)).StartsWith(this.GetHierarchicalSortOrder(Row));
			}
			return Result;
		}
		private void DrawTotalsRow(Content Content)
		{
			if (this.ShowTotals) {
				this.StyleSheet.AddCustomRule("#CB_" + this.ID + "_TotalsRow td ", this.TotalsRowStyle.Draw(true));
				Content.Add("<tr id=\"CB_" + this.ID + "_TotalsRow\">");
				for (int i = 0; i <= this.DrawnTableColumnCount - 1; i++) {
					int DataColumnIndex = i;
					if (this.Rows.AllowEdit && !string.IsNullOrEmpty(this.Rows.EditLinkUrl))
						DataColumnIndex -= 1;
					if (i == 0 && (this.CheckBoxes || this.RadioBoxes || this.Rows.AllowEdit)) {
						Content.Add("<td style=\"font-size:1pt;\">&nbsp;</td>");
					} else if (i == 1 && (this.CheckBoxes || this.RadioBoxes) && this.Rows.AllowEdit) {
						Content.Add("<td style=\"font-size:1pt;\">&nbsp;</td>");
					} else if (i == this.DrawnTableColumnCount - 1 && this.Rows.AllowDelete) {
						Content.Add("<td style=\"font-size:1pt;\">&nbsp;</td>");
					} else if (this.Columns(DataColumnIndex).GetType.FullName == "Ophelia.Web.View.Base.DataGrid.NumberBoxColumn" || this.Columns(DataColumnIndex).GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Base.DataGrid.NumberBoxColumn"))) {
						NumberBoxColumn NumberBoxColumn = this.Columns(DataColumnIndex);
						if (string.IsNullOrEmpty(NumberBoxColumn.TotalCellStyle.Draw())) {
							Content.Add("<td" + NumberBoxColumn.GetID + NumberBoxColumn.Style.Draw() + ">");
						} else {
							Content.Add("<td" + NumberBoxColumn.TotalCellStyle.Draw() + ">");
						}
						//ConfigureTotalPanel

						if (NumberBoxColumn.HasTotals) {
							string TotalValue = NumberBoxColumn.TotalValue.ToString();
							if (NumberBoxColumn.ShowAlwaysDecimalPart) {
								TotalValue = Strings.FormatNumber(TotalValue, NumberBoxColumn.DecimalDigits, TriState.True, TriState.False, TriState.True);
							}
							string DecimalDigitCharCount = "";
							for (int index = 0; index <= NumberBoxColumn.DecimalDigits - 1; index++) {
								DecimalDigitCharCount += "#";
							}
							TotalValue = NumberBoxColumn.TotalValue > 999 && !NumberBoxColumn.ShowAlwaysDecimalPart ? NumberBoxColumn.TotalValue.ToString("0,0." + DecimalDigitCharCount) : (NumberBoxColumn.ShowAlwaysDecimalPart ? Strings.FormatNumber(NumberBoxColumn.TotalValue, NumberBoxColumn.DecimalDigits, TriState.True, TriState.False, TriState.True) : NumberBoxColumn.TotalValue.ToString());

							NumberBoxColumn.DataControl.Value = TotalValue;
							NumberBoxColumn.DataControl.ReadOnly = true;
							if (!string.IsNullOrEmpty(this.Columns(DataColumnIndex).Suffix))
								NumberBoxColumn.DataControl.Value += " " + this.Columns(DataColumnIndex).Suffix;
							this.ConfigureSubControls(NumberBoxColumn.DataControl);
							//ConfigureTotalPanel
							Content.Add(NumberBoxColumn.DataControl.Draw);
						}
						Content.Add("</td>");
					} else {
						Content.Add("<td style=\"font-size:1pt;\">&nbsp;</td>");
					}
				}
				Content.Add("</tr>");
			}
		}
		private string GetRowClassName(Row Row)
		{
			string RowOrder = this.GetHierarchicalSortOrder(Row);
			string ReturnValue = Row.GetClassName();
			if (Row.Collection.UseCustomizedSelectedRowStyle && Row.IsSelected) {
				ReturnValue = Row.GetSelectedRowClassName();
			}
			if (this.HierarchicDisplay) {
				if (!string.IsNullOrEmpty(ReturnValue))
					ReturnValue += " ";
				if (RowOrder.Length > 2) {
					ReturnValue += "he" + RowOrder.Substring(0, RowOrder.Length - 3);
				}
				ReturnValue += " sov_" + RowOrder;
			}
			if (!string.IsNullOrEmpty(this.Rows.RowStyle.Class)) {
				ReturnValue += " " + this.Rows.RowStyle.Class;
			}
			if (!string.IsNullOrEmpty(Row.Style.Class)) {
				ReturnValue += " " + Row.Style.Class;
				Row.Style.Class = "";
			}
			if (this.RowHasData && !string.IsNullOrEmpty(Row.ExtraData)) {
				ReturnValue += " ShowDataRow";
			}
			return " class=\"" + ReturnValue + "\"" + Row.Style.Draw;
		}

		protected virtual void OnAfterFormDraw(Content Content)
		{
		}
		private void SetDrawnTableColumnCount()
		{
			if (this.Rows.Count > 0) {
				Row Row = this.Rows(0);
				for (int i = 0; i <= Row.Cells.Count - 1; i++) {
					this.nDrawnTableColumnCount += 1;
					Column Column = Row.Cells(i).Column;
					if (Column.DrawHiddenColumn)
						this.nDrawnTableColumnCount -= 1;
				}
			}
		}
		public static Panel DrawExtraRowDataPanel(string RowDisplayedID, WebControl Control)
		{
			Panel Panel = new Panel(RowDisplayedID + "_data");
			Panel.Controls.Add(Control);
			Panel.Script.AppendLine("document.getElementById('" + RowDisplayedID + "_rowdata" + "').style.display='table-row';var classes =document.getElementById('" + RowDisplayedID + "').className; if (classes == undefined) {classes = '';}  document.getElementById('" + RowDisplayedID + "').className = classes + ' ShowDataRow'; ");
			return Panel;
		}
		public object ArrangeForShowRowDataEvent(string FunctionSignature, string RemoveSignature = "")
		{
			return "var dataelement = document.getElementById(this.id + '_rowdata'); " + "if (dataelement.style.display=='table-row'){" + "dataelement.style.display='none'; var classes =this.className; classes = classes.replace(' ShowDataRow',''); this.className = classes;" + RemoveSignature + "}" + "else{" + FunctionSignature + "}";
		}
		protected void DrawRows(Ophelia.Web.View.Content Content, ref Hashtable CheckBoxColumns, ref string AjaxParametersString)
		{
			Row Row = null;
			for (int i = 0; i <= this.Rows.Count - 1; i++) {
				Row = this.Rows(i);
				if (!this.HasDeleteRow && Row.CanBeDeleted)
					this.HasDeleteRow = true;

				Row.CanBeSelected = this.RadioBoxes || this.CheckBoxes;
				this.OnBeforeRowDrawn(new RowEventArgs(Row));

				Content.Add("<tr " + Row.GetID() + " " + this.GetRowClassName(Row));
				if (!string.IsNullOrEmpty(Row.OnRowClick))
					Content.Add(" onclick=\"" + Row.OnRowClick + "\"");
				Content.Add(">");
				this.DrawRowEditButton(Row, Content);
				if (this.ShowRowNumbers || this.RowContextMenuIsEnabled) {
					Panel RowNumberContainer = new Panel("");
					RowNumberContainer.Style.Class = "RowNumberContainer";
					RowNumberContainer.Style.Width = 20;
					RowNumberContainer.Style.PositionStyle = Position.Relative;
					if (i == 0) {
						this.StyleSheet.AddClassBasedRule("RowIDClassName", "text-align:left;vertical-align:middle;background-color:#ccc;");
						if (this.RowContextMenuIsEnabled) {
							this.StyleSheet.AddClassBasedRule("RowIDClassName:hover", "padding:0;cursor:pointer;background-color:#E6E6E6;color:#999;");
						}
					}

					Panel RowNumberPanel = new Panel(Row.ID + "_RowNumber");
					RowNumberPanel.Style.Class = "RowNumberDiv";
					if ((this.Binding.Value.GetType.BaseType.Name == "EntityCollection" || this.Binding.Value.GetType.BaseType.Name == "HierarchicalCollection") && this.Binding.Value.Definition.PageSize > 0) {
						RowNumberPanel.Content.Add(((this.Binding.Value.Definition.PageSize * (this.Binding.Value.Definition.Page - 1)) + i + 1).ToString());
					} else {
						RowNumberPanel.Content.Add((i + 1).ToString());
					}

					Content.Add("<td ");
					Content.Add(this.GetCellAttributeValue(null));
					Content.Add(" class=\"RowIDClassName\" ");
					if (Row.ContextMenu.Commands.Count > 0) {
						Content.Add(" onmouseout=\"document.getElementById('" + Row.ID + "_ContextMenu').style.display='none'; document.getElementById('" + Row.ID + "_RowNumber').style.display='block';\"  ");
						Content.Add("onmouseover=\"document.getElementById('" + Row.ID + "_RowNumber').style.display ='none';document.getElementById('" + Row.ID + "_ContextMenu').style.display = 'inline-block';\" ");
					}
					Content.Add(">");
					RowNumberContainer.Controls.Add(RowNumberPanel);
					RowNumberContainer.Controls.Add(Row.ContextMenu);
					Content.Add(RowNumberContainer.Draw());
					Content.Add("</td>");
				}
				Column CurrentColumn = null;
				Cell CurrentCell = null;
				for (int j = 0; j <= this.Columns.ExpandedColumns.Count - 1; j++) {
					CurrentColumn = this.Columns.ExpandedColumns(j);
					if (string.IsNullOrEmpty(CurrentColumn.MemberName)) {
						CurrentCell = this.Rows(i).Cells(CurrentColumn.Index);
					} else {
						CurrentCell = this.Rows(i).Cells(CurrentColumn.MemberName);
					}

					if (this.UpdateWithAjax)
						AjaxParametersString += CurrentCell.DataControl.ID + "-";
					Content.Add("<td");

					Content.Add(this.GetCellAttributeValue(this.Columns.ExpandedColumns(j)));

					string DisplayValue = "";
					if (CurrentColumn.DrawHiddenColumn) {
						DisplayValue = "display:none;";
						this.StyleSheet.AddCustomRule("#" + CurrentColumn.ID, DisplayValue);
						Content.Add(" style='" + DisplayValue + "'");
					}
					if (i == 0) {
						this.StyleSheet.AddClassBasedRule("c" + CurrentColumn.ID, CurrentCell.Column.CellStyle.Draw(true));
					}
					Content.Add(" class=\"" + "c" + CurrentColumn.ID + " " + CurrentColumn.CellStyle.Class + "\" " + CurrentCell.Style.Draw + " >");
					bool SelectionItemIsDrawn = false;
					if (j == 0) {
						SelectionItemIsDrawn = this.DrawSelectionItem(Row, Content);
					}

					this.ConfigureSubControls(CurrentCell.DataControl);
					CurrentCell.DataControl.ID = CurrentCell.ID;
					if (CurrentCell.DataControl.GetType.Name == "DateTimePicker") {
						CurrentCell.DataControl.Style.WidthInPercent = 100;
					} else if (CurrentCell.DataControl.GetType.Name != "CheckBox") {
						CurrentCell.DataControl.Style.Dock = DockStyle.Width;
					}
					CurrentCell.DataControl.Value = CurrentCell.Text;
					if (object.ReferenceEquals(Row, this.ExtraRow)) {
						CurrentCell.DataControl.ReadOnly = CurrentCell.Column.ReadOnly;
					} else {
						CurrentCell.DataControl.ReadOnly = CurrentCell.ReadOnly;
					}
					CurrentCell.DataControl.Validators.ValidationScript = this.Script;
					string ExtraOnChangeEvent = "";
					if (CurrentCell.DataControl.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.InputDataControl"))) {
						ExtraOnChangeEvent = ((InputDataControl)CurrentCell.DataControl).OnChangeEvent;
						((InputDataControl)CurrentCell.DataControl).OnChangeEvent = "OnChangeValue(document.getElementById('" + CurrentCell.ID + "'), document.getElementById('CB_" + this.ID + "FormChangedItems'));" + ExtraOnChangeEvent;
					}
					if (CurrentCell.DataControl.GetType.FullName.Equals("Ophelia.Web.View.Controls.FileBoxWithAjax")) {
						CurrentCell.Column.MemberName = CurrentCell.Column.MemberName.Replace(".FileName", "");
						((Ophelia.Web.View.Controls.FileBoxWithAjax)CurrentCell.DataControl).FileID = ((Ophelia.Application.Base.Entity)((Ophelia.Application.Base.Entity)CurrentCell.Row.Item).Data.GetPropertyValue(CurrentCell.Column.MemberName)).ID.ToString();
						CurrentCell.Column.MemberName = CurrentCell.Column.MemberName + ".FileName";
					}
					DataControl DrawnWebControl = null;
					if (CurrentCell.DataControl.ReadOnly && !string.IsNullOrEmpty(CurrentColumn.EditLink) && (!object.ReferenceEquals(this.Rows(i), this.ExtraRow))) {
						Link Link = new Link(Row.ID + "_EditEntityLink");
						this.ConfigureSubControls(Link);
						Link.Value = CurrentCell.Value;
						Label LinkLabel = new Label(Row.ID + "_EditEntity");
						this.ConfigureSubControls(LinkLabel);
						LinkLabel.Style.Width = CurrentColumn.Style.Width;
						if (CurrentColumn.EditLink.Contains("RootID") && CurrentColumn.EditLink.IndexOf("?") > -1) {
							Link.Url = CurrentColumn.EditLink.Insert(CurrentColumn.EditLink.IndexOf("?"), CurrentColumn.LinkItemID(i));
						} else {
							Link.Url = CurrentColumn.EditLink + CurrentColumn.LinkItemID(i);
						}
						LinkLabel.Value = Link.Draw;
						DrawnWebControl = LinkLabel;
					} else {
						DrawnWebControl = CurrentCell.DataControl;
					}
					if (!string.IsNullOrEmpty(CurrentColumn.Suffix))
						DrawnWebControl.Value += " " + CurrentColumn.Suffix;

					if (object.ReferenceEquals(Row, this.ExtraRow) && DrawnWebControl != null && this.Rows.AllowNew) {
						if (!CurrentColumn.HideExtraRowCell)
							Content.Add(DrawnWebControl.Draw);
					} else if (DrawnWebControl != null) {
						Content.Add(DrawnWebControl.Draw);
					}

					if (CurrentColumn.GetType.Name == "CheckBoxColumn" && ((CheckBoxColumn)CurrentColumn).AllowSelectAll) {
						if (CheckBoxColumns[((CheckBoxColumn)CurrentColumn).MemberName] == null) {
							CheckBoxColumns.Add(((CheckBoxColumn)CurrentColumn).MemberName, "");
						}
						if (!string.IsNullOrEmpty(CheckBoxColumns[((CheckBoxColumn)CurrentColumn).MemberName])) {
							CheckBoxColumns[((CheckBoxColumn)CurrentColumn).MemberName] = CheckBoxColumns[((CheckBoxColumn)CurrentColumn).MemberName] + ":";
						}
						CheckBoxColumns[((CheckBoxColumn)CurrentColumn).MemberName] += DrawnWebControl.ID;
					}

					if (CurrentColumn.DrawHiddenValues) {
						HiddenBox HiddenInput = new HiddenBox("");
						this.ConfigureSubControls(HiddenInput);
						HiddenInput.ID = CurrentCell.DataControl.ID + "Hidden";
						HiddenInput.Value = CurrentCell.Value;
						Content.Add(HiddenInput.Draw());
					}
					if (CurrentCell.DataControl.GetType.IsSubclassOf(Type.GetType("Ophelia.Web.View.Controls.InputDataControl"))) {
						((InputDataControl)CurrentCell.DataControl).OnChangeEvent = ExtraOnChangeEvent;
					}
					if (SelectionItemIsDrawn) {
						Content.Add("</td></tr></table>");
					}
					Content.Add("</td>");

				}

				this.DrawRowEditButton(Row, Content, false);

				if (object.ReferenceEquals(Row, this.ExtraRow) && this.HasDeleteRow) {
					Content.Add("<td" + this.DeleteColumnStyle.Draw() + " " + this.GetCellAttributeValue(null) + ">");
					Label DeleteLink = new Label("Dummy_DeleteLink");
					DeleteLink.Style.Class = "DeleteLink";
					this.DeleteWebControl.Style.Opacity = 0.5;
					this.DeleteWebControl.OnClickEvent = "";
					this.ConfigureSubControls(DeleteLink);
					this.ConfigureSubControls(this.DeleteWebControl);
					DeleteLink.Value = this.DeleteWebControl.Draw();
					DeleteLink.ReadOnly = true;
					Content.Add(DeleteLink.Draw);
					Content.Add("</td>");
				//Content.Add("<td" & Me.DeleteColumnStyle.Draw() & "></td>")
				} else if (Row.CanBeDeleted) {
					Content.Add("<td" + this.DeleteColumnStyle.Draw() + " " + this.GetCellAttributeValue(null) + ">");
					if (!this.UpdateWithAjax) {
						Link DeleteLink = new Link(Row.ID + "_DeleteLink");
						DeleteLink.Style.Class = "DeleteLink";
						this.ConfigureSubControls(DeleteLink);
						if (string.IsNullOrEmpty(this.DeleteWebControl.OnClickEvent))
							this.DeleteWebControl.OnClickEvent = "return " + this.ID + "_DeleteConfirmation()";
						DeleteLink.Url = Row.DeleteLinkUrl;
						this.ConfigureSubControls(this.DeleteWebControl);
						DeleteLink.Value = this.DeleteWebControl.Draw();
						Content.Add(DeleteLink.Draw);
					} else {
						Label DeleteLink = new Label(Row.ID + "_DeleteLink");
						DeleteLink.Style.Class = "DeleteLink";
						this.ConfigureSubControls(DeleteLink);
						DeleteLink.OnClickEvent = "if (confirm('" + this.Page.Client.Dictionary.GetWord("Message.DeleteConfirmation") + "')) {" + Row.DeleteLinkUrl + "}";
						this.ConfigureSubControls(this.DeleteWebControl);
						DeleteLink.Value = this.DeleteWebControl.Draw();
						Content.Add(DeleteLink.Draw);
					}
				} else if (this.Rows.AllowDelete) {
					Content.Add("<td" + this.DeleteColumnStyle.Draw() + " " + this.GetCellAttributeValue(null) + ">");
					Label DeleteLink = new Label("Dummy_DeleteLink");
					DeleteLink.Style.Class = "DeleteLink";
					this.DeleteWebControl.Style.Opacity = 0.5;
					this.DeleteWebControl.OnClickEvent = "";
					this.ConfigureSubControls(DeleteLink);
					this.ConfigureSubControls(this.DeleteWebControl);
					DeleteLink.Value = this.DeleteWebControl.Draw();
					DeleteLink.ReadOnly = true;
					Content.Add(DeleteLink.Draw);
					Content.Add("</td>");
				}
				this.DeleteWebControl.Style.Opacity = 1;
				Content.Add("</tr>");
				if (this.RowHasData) {
					if (string.IsNullOrEmpty(Row.ExtraData)) {
						Content.Add("<tr").Add(" id=\"").Add(Row.ID).Add("_rowdata\"").Add(" style=\"display:none;\"").Add(">");
						Content.Add("<td").Add(" ").Add(this.GetCellAttributeValue(null)).Add(" colspan=\"").Add((!this.Readonly && (this.Rows.AllowEdit || this.Rows.AllowNew && this.Rows.Count > 0) ? this.DrawnTableColumnCount - 1 : this.DrawnTableColumnCount)).Add("\">");
						Content.Add("<div id=\"").Add(Row.ID).Add("_data\"").Add("></div>");
						Content.Add("</td>");
						Content.Add("</tr>");
					} else {
						Content.Add("<tr").Add(" id=\"").Add(Row.ID).Add("_rowdata\"").Add(" style=\"display:table-row;\"").Add(">");
						Content.Add("<td").Add(" ").Add(this.GetCellAttributeValue(null)).Add(" colspan=\"").Add((!this.Readonly && (this.Rows.AllowEdit || this.Rows.AllowNew && this.Rows.Count > 0) ? this.DrawnTableColumnCount - 1 : this.DrawnTableColumnCount)).Add("\">");
						Content.Add("<div id=\"").Add(Row.ID).Add("_data\"").Add(">").Add(Row.ExtraData).Add("</div>");
						Content.Add("</td>");
						Content.Add("</tr>");
					}
				}
			}
		}
		public string GetCellAttributeValue(Column Column)
		{
			if (this.IsMobileGrid) {
				if (Column == null)
					return " data-title=\"" + "" + "\" ";
				return " data-title=\"" + Column.Name + "\" ";
			}
			return "";
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Bind();
			if (this.IsMobileGrid) {
				this.Page.Header.Links.Add(UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "ophelia-mobile.css", EmbeddedFileProcessingMethod.PageProcessing), UI.HeadLink.ReleationShipType.StyleSheet);
			}
			this.StyleSheet.AddClassBasedRule(this.ID + "RowStyle", this.Rows.RowStyle);
			this.StyleSheet.AddClassBasedRule(this.ID + "SelectedRowStyle", this.Rows.SelectedRowStyle);
			string AjaxParametersString = "";
			Hashtable CheckBoxColumns = new Hashtable();
			this.ConfigureCellStyle();
			this.ContainerStyle.AddClass("no-more-tables");
			Content.Add("<div id=\"" + this.ID + "Container\"" + this.ContainerStyle.Draw + ">");
			if (this.HasForm) {
				Content.Add("<form name=\"CB_" + this.ID + "Form\" ID=\"CB_" + this.ID + "Form\" method=\"POST\" action=\"" + this.Request.RawUrl + "\">");
				Content.Add("<input id=\"CB_" + this.ID + "FormIsSubmitted\" name=\"CB_" + this.ID + "FormIsSubmitted\"  value=\"1\" type=\"hidden\" >");
			}
			Content.Add("<input id=\"CB_" + this.ID + "FormChangedItems\" name=\"CB_" + this.ID + "FormChangedItems\" type=\"text\" value=\"" + "" + "\" style=\"display:none;\">");
			this.OnAfterFormDraw(Content);
			Content.Add("<table name=\"CB_" + this.ID + "\" id=\"CB_" + this.ID + "\" " + this.Style.Draw + " cellspacing=\"" + this.Style.CellSpacing + "\" cellpadding=\"" + this.Style.CellPadding + "\" >");
			this.SetDrawnTableColumnCount();
			Content.Add(this.Header.Draw());
			if (this.ToolBar.Visible) {
				Content.Add(this.ToolBar.Draw);
			}
			if (this.Rows.Count > 0 || this.AlwaysShowHeader)
				this.DrawColumns(Content);
			this.DrawRows(Content, ref CheckBoxColumns, ref AjaxParametersString);

			if (this.Rows.Count.Equals(0) && this.NoRowMessage != string.Empty) {
				Content.Add("<tr " + this.Rows.RowStyle.Draw() + "><td colspan='" + this.Columns.Count + "'>" + this.NoRowMessage + "</td></tr>");
			} else {
				this.DrawTotalsRow(Content);
			}

			if (!string.IsNullOrEmpty(Footer.DefaultRegion.Content.Value) || Footer.DefaultRegion.Controls.Count > 0 || ((this.Rows.AllowNew || this.Rows.AllowEdit) && !this.Readonly && this.HasForm && this.Rows.Count > 0)) {
				Content.Add("<tr id=\"CB_" + this.ID + "_Footer\" >");
				if (this.DrawnTableColumnCount > 1) {
					Content.Add("<td colspan=\"" + (!this.Readonly && (this.Rows.AllowEdit || this.Rows.AllowNew && this.Rows.Count > 0) ? this.DrawnTableColumnCount - 1 : this.DrawnTableColumnCount) + "\"" + this.Footer.Style.Draw + ">");
					this.ConfigureSubControls(this.Footer.Control);
					Content.Add(this.Footer.Control.Draw);
					Content.Add("</td>");
				}
				if (!this.Readonly && this.HasForm && (this.Rows.AllowNew || this.Rows.AllowEdit) && this.Rows.Count > 0) {
					Content.Add("<td" + this.SaveColumnStyle.Draw() + ">");
					if (!this.UpdateWithAjax) {
						Button HiddenButtonForSubmitAnyWay = new Button("Hidden");
						HiddenButtonForSubmitAnyWay.Style.Display = DisplayMethod.Hidden;
						this.ConfigureSubControls(HiddenButtonForSubmitAnyWay);
						Content.Add(HiddenButtonForSubmitAnyWay.Draw());
						this.SubmitButton.OnClickEvent = "document.CB_" + this.ID + "Form.submit()";
					} else {
						this.SubmitButton.OnClickEvent = "Save" + this.ID + "()";
						this.UpdateWithAjaxFunction.ControlIDs = AjaxParametersString;
					}
					this.ConfigureSubControls(this.SubmitButton);
					if (string.IsNullOrEmpty(this.SubmitButton.Value)) {
						Image SubmitButtonImage = new Image("", ServerSide.ImageDrawer.GetImageUrl("WebSave"));
						SubmitButtonImage.Style.Padding = 0;
						this.ConfigureSubControls(SubmitButtonImage);
						this.SubmitButton.Value = SubmitButtonImage.Draw();
					}
					Content.Add(this.SubmitButton.Draw());
					Content.Add("</td>");
				}
				Content.Add("</tr>");
			}
			Content.Add("</table>");
			if (this.HasForm) {
				Content.Add("</form>");
			}
			Content.Add("</div>");
			this.AddConfirmDeleteActionFunction();
			this.AddCheckBoxesSelectFunction(CheckBoxColumns, Content);
		}
		private void AddCheckBoxesSelectFunction(Hashtable CheckBoxColumns, Ophelia.Web.View.Content Content)
		{
			DictionaryEntry Entry = default(DictionaryEntry);
			foreach (DictionaryEntry Entry_loopVariable in CheckBoxColumns) {
				Entry = Entry_loopVariable;
				((CheckBoxColumn)this.Columns(Entry.Key)).SelectAllControl.OnChangeEvent = "SelectAll_ValueChanged('" + Entry.Value + "',this)";
				Content.Replace("SelectAll_ValueChanged(this)", ((CheckBoxColumn)this.Columns(Entry.Key)).SelectAllControl.OnChangeEvent);
				if (this.Script.Function("SelectAll_ValueChanged") == null) {
					Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function SelectFunction = this.Script.AddFunction("SelectAll_ValueChanged", "", "inputnames,checkbox");
					SelectFunction.AppendLine("var values = inputnames.split(':');");
					SelectFunction.AppendLine(" for(i=0; i<values.length; i++)");
					SelectFunction.AppendLine("{document.getElementById(values[i]).checked=checkbox.checked;");
					SelectFunction.AppendLine("document.getElementById(values[i]).onclick();");
					SelectFunction.AppendLine("}");
				}
			}
		}
		private string DrawValidateFunction()
		{
			string Output = "";
			//"Validate_Save" & Me.ID & vbCrLf
			//Output &= "{" & vbCrLf
			for (int i = 0; i <= this.Columns.Count - 1; i++) {
				Output += "if(!" + Columns(i).Validators.DrawValidatorString(true) + "){" + Constants.vbCrLf;
				Output += "return false;" + Constants.vbCrLf;
				Output += "}" + Constants.vbCrLf;
			}
			//Output &= "else {" & vbCrLf
			//Output &= "return true;" & vbCrLf
			//Output &= "}" & vbCrLf
			return Output;
		}
		protected virtual void AddConfirmDeleteActionFunction()
		{
			this.Script.AddConfirmFunction(this.ID + "_DeleteConfirmation", this.Page.Client.Dictionary.GetWord("Message.DeleteConfirmation"));
		}

		protected virtual void CustomizeFooter(Footer Footer)
		{
		}
		public static DataGridData GetChangedRowData(string DataGridName, HttpRequest Request)
		{
			int n = 0;
			int i = 0;
			string TempString = "";
			string ColumnMemberName = "";
			string ID = "";
			string Value = "";
			DataGridData DataGridData = new DataGridData();
			string ChangedRowIDs = "";
			if (Request("CB_" + DataGridName + "FormChangedItems") != null) {
				ChangedRowIDs = Request("CB_" + DataGridName + "FormChangedItems");
			}
			for (n = 0; n <= Request.Form.AllKeys.GetLength(0) - 1; n++) {
				ID = "";
				TempString = Request.Form.AllKeys(n);
				if (TempString.StartsWith("CB_" + DataGridName + "Form_Row_ID_")) {
					TempString = TempString.Replace("CB_" + DataGridName + "Form_Row_ID_", "");
					for (i = 0; i <= TempString.Length - 1; i++) {
						if (Information.IsNumeric(TempString[i]) || TempString[i] == "-") {
							ID += TempString[i];
						} else {
							break; // TODO: might not be correct. Was : Exit For
						}
					}
					if (ChangedRowIDs.Contains("CB_" + DataGridName + "Form_Row_ID_" + ID)) {
						if (TempString.Contains("CheckBox")) {
							ColumnMemberName = "CheckBox";
						} else {
							TempString = TempString.Remove(0, i + 1);
							ColumnMemberName = TempString.Replace("CB_" + DataGridName + "Form_Column_", "");
						}
						Value = Request.Form.GetValues(n)(0);
						DataGridData.AddData(ID, ColumnMemberName, Value);
					}
				}
			}
			return DataGridData;
		}
		public static DataGridData GetRowData(string DataGridName, HttpRequest Request)
		{
			int n = 0;
			int i = 0;
			string TempString = "";
			string ColumnMemberName = "";
			string ID = "";
			string Value = "";
			DataGridData DataGridData = new DataGridData();
			string ChangedRowIDs = "";
			if (Request("CB_" + DataGridName + "FormChangedItems") != null) {
				ChangedRowIDs = Request("CB_" + DataGridName + "FormChangedItems");
			}
			for (n = 0; n <= Request.Form.AllKeys.GetLength(0) - 1; n++) {
				ID = "";
				TempString = Request.Form.AllKeys(n);
				if (TempString.StartsWith("CB_" + DataGridName + "Form_Row_ID_")) {
					TempString = TempString.Replace("CB_" + DataGridName + "Form_Row_ID_", "");
					for (i = 0; i <= TempString.Length - 1; i++) {
						if (Information.IsNumeric(TempString[i]) || TempString[i] == "-") {
							ID += TempString[i];
						} else {
							break; // TODO: might not be correct. Was : Exit For
						}
					}
					if (TempString.Contains("CheckBox")) {
						ColumnMemberName = "CheckBox";
					} else {
						TempString = TempString.Remove(0, i + 1);
						ColumnMemberName = TempString.Replace("CB_" + DataGridName + "Form_Column_", "");
					}
					Value = Request.Form.GetValues(n)(0);
					DataGridData.AddData(ID, ColumnMemberName, Value);
				}
			}
			return DataGridData;
		}

		protected virtual void CustomizeHeader(Header Header)
		{
		}

		protected virtual void CustomizeStyle()
		{
		}
		public DataGrid(string Title, string ID) : base(Title, ID)
		{
		}
		public DataGrid(string Title, string MemberName, string ID) : base(Title, MemberName, ID)
		{
		}
		public DataGrid(object Data, string MemberName, string ID) : base(Data, MemberName, ID)
		{
		}
		public DataGrid(object Data, string Title, string MemberName, string ID) : base(Data, Title, MemberName, ID)
		{
		}
		public DataGrid(string Title, ICollection Data, string ID) : base(Title, Data, ID)
		{
		}
		public DataGrid(ICollection Data, string ID) : base(Data, ID)
		{
		}
		protected override void Customize(string Title)
		{
			base.Customize(Title);
			this.oHeader = new Header(this);
			this.Header.Title = Title;
			this.CustomizeHeader(this.oHeader);
			this.oFooter = new Footer(this);
			this.CustomizeFooter(this.oFooter);
			this.CustomizeStyle();
		}
		public DataGrid(string ID) : base(ID)
		{
		}
		public class DataGridData
		{
			private Hashtable IDs = new Hashtable();
			private Hashtable SelectedItems = new Hashtable();
			private Hashtable Rows = new Hashtable();
			private string sIDsInText = "";
			private string sSelectedItemsInText = "";
			internal void AddData(int ID, string ColumnName, string Value)
			{
				Hashtable Columns = null;
				if (IDs[ID] == null) {
					IDs.Add(ID, "Added");
					if (ColumnName.Contains("CheckBox") && SelectedItems[ID] == null) {
						SelectedItems.Add(ID, "Added");
					}
				}
				if (Rows[ID] == null) {
					Columns = new Hashtable();
					Rows.Add(ID, Columns);
				} else {
					Columns = Rows[ID];
				}
				Columns.Add(ColumnName, Value);
			}
			public Hashtable GetRowData(int ID)
			{
				return this.Rows[ID];
			}
			public string GetCellData(int ID, string ColumnMemberName)
			{
				if (this.GetRowData(ID) == null || this.GetRowData(ID)[ColumnMemberName] == null) {
					return "";
				}
				return this.GetRowData(ID)[ColumnMemberName];
			}
			public int ItemsCount {
				get { return this.IDs.Count; }
			}
			public string IDsInText {
				get {
					if (string.IsNullOrEmpty(this.sIDsInText)) {
						DictionaryEntry DictionaryEntry = default(DictionaryEntry);
						foreach (DictionaryEntry DictionaryEntry_loopVariable in IDs) {
							DictionaryEntry = DictionaryEntry_loopVariable;
							if (!string.IsNullOrEmpty(this.sIDsInText))
								this.sIDsInText += ", ";
							this.sIDsInText += DictionaryEntry.Key;
						}
					}
					return this.sIDsInText;
				}
			}
			public string SelectedItemsInText {
				get {
					if (string.IsNullOrEmpty(this.sSelectedItemsInText)) {
						DictionaryEntry DictionaryEntry = default(DictionaryEntry);
						foreach (DictionaryEntry DictionaryEntry_loopVariable in SelectedItems) {
							DictionaryEntry = DictionaryEntry_loopVariable;
							if (!string.IsNullOrEmpty(this.sSelectedItemsInText))
								this.sSelectedItemsInText += ", ";
							this.sSelectedItemsInText += DictionaryEntry.Key;
						}
					}
					return this.sSelectedItemsInText;
				}
			}
			public enum TreeViewSelectionPattern
			{
				None = 0,
				BottomOfPath = 1,
				TopOfPath = 2
			}
		}
	}
}
