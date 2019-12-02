using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class ConditionalFilterBox : FilterBox
	{
		private string sConditionText = string.Empty;
		private CheckBox oConditionCheckBox;
		private bool bIsReverseSelectionEnabled = false;
		public string ConditionText {
			get { return this.sConditionText; }
			set { this.sConditionText = value; }
		}
		public bool IsReverseSelectionEnabled {
			get { return this.bIsReverseSelectionEnabled; }
			set { this.bIsReverseSelectionEnabled = value; }
		}
		public CheckBox ConditionCheckBox {
			get {
				if (this.oConditionCheckBox == null) {
					this.oConditionCheckBox = new CheckBox(this.ID + "_ConditionCheckBox");
				}
				return this.oConditionCheckBox;
			}
		}
		private void AddConditionalFieldCheckBoxClickedEvent()
		{
			//Me.ID & "_Info"
			if (this.Script.Function("ConditionalFilterBoxFieldCheckBoxClickedEvent") == null) {
				System.Text.StringBuilder FunctionString = new System.Text.StringBuilder();
				FunctionString.AppendLine("var controlID = element.id.replace('_ConditionCheckBox','');");
				FunctionString.AppendLine("var displayText=''");
				FunctionString.AppendLine("var hideText=''");
				FunctionString.AppendLine(" if(isReverseSelectionEnabled){");
				FunctionString.AppendLine(" displayText='none';hideText='block';}");
				FunctionString.AppendLine(" else{displayText='block';hideText='none';}");
				FunctionString.AppendLine("if (!element.checked){ ");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = hideText;");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = hideText;");
				//FunctionString.AppendLine("    document.getElementById(controlID + '_Info').style.display = hideText; ")
				FunctionString.AppendLine("    }");
				FunctionString.AppendLine(" else{");
				FunctionString.AppendLine("    document.getElementById(controlID).style.display = displayText;");
				FunctionString.AppendLine("    document.getElementById(controlID + '_ConditionText').style.display = displayText;");
				//FunctionString.AppendLine("    document.getElementById(controlID + '_Info').style.display = displayText; ")
				FunctionString.AppendLine("  }");
				this.Script.AddFunction("ConditionalFilterBoxFieldCheckBoxClickedEvent", FunctionString.ToString(), "element,isReverseSelectionEnabled", "");
			}
		}
		public override void OnBeforeDraw(Content Content)
		{
			string TempValue = "";
			Binding Binding = new Binding();
			Binding.Item = this.SelectedEntity;
			Binding.MemberName = this.DisplayMember;
			if (Binding.Value == null) {
				TempValue = "";
			} else {
				TempValue = Binding.Value.ToString;
			}
			if (string.IsNullOrEmpty(TempValue)) {
				TempValue = this.Value;
			}

			this.AddFunctions();

			Content.Add(this.FilterBoxDataSource.Draw);
			Content.Add(new HiddenBox(this.ID + "DataGridDef", this.GetDataSourceInString).Draw);
			Content.Add("<div id=\"" + this.ID + "_FBContainer\" style=\"position:relative;\">");

			TextBox Text = new TextBox(this.ID, TempValue);

			Text.CloneEventsFrom(this);
			for (int i = 0; i <= this.Attributes.Count - 1; i++) {
				Text.Attributes.Add(this.Attributes.Keys(i), this.Attributes.Values(i));
			}
			HiddenBox HiddenValue = new HiddenBox(this.ID + "SelectedID", this.SelectedEntity != null ? this.SelectedEntity.ID.ToString : "0");
			HiddenValue.OnChangeEvent = Text.OnChangeEvent;
			Text.OnChangeEvent = "";
			Content.Add(HiddenValue.Draw);
			Text.OnKeyUpEvent = "return FBKeyPress(this,event,document.getElementById('" + this.ID + "DataGridDef').value);";
			Text.OnFocusEvent = "FBFocusControl(this);";
			Text.OnBlurEvent = "HideFBGridInThread(this);";
			Text.OnKeyDownEvent = "return FBKeyDown(this,event);";
			Text.AutoComplete = false;

			if (this.Style.Dock == DockStyle.Width) {
				this.Style.Right = -30;
			} else if (this.Style.Dock == DockStyle.Fill) {
				this.Style.Right = -30;
			}

			this.AddConditionalFieldCheckBoxClickedEvent();
			Content.Add("<div id=\"" + this.ID + "_Container" + "\">");
			this.ConditionCheckBox.ID = this.ID + "_ConditionCheckBox";
			if (this.ConditionCheckBox.Value == true && !IsReverseSelectionEnabled) {
				this.ConditionCheckBox.Style.Display = DisplayMethod.Block;
			}
			this.ConditionCheckBox.Style.Left = 0;
			this.ConditionCheckBox.Style.VerticalAlignment = VerticalAlignment.Middle;
			this.ConditionCheckBox.OnClickEvent += "ConditionalFilterBoxFieldCheckBoxClickedEvent(this, " + (this.IsReverseSelectionEnabled ? 1 : 0) + ");";
			Content.Add(this.ConditionCheckBox.Draw());

			Content.Add("<span id=\"" + this.ID + "_ConditionText" + "\" ");
			if (IsReverseSelectionEnabled) {
				if (this.ConditionCheckBox.Value == true) {
					Content.Add(" style=\"display:none;\" ");
				} else {
					Content.Add(" style=\"display:block;\" ");
				}
			} else {
				if (this.ConditionCheckBox.Value == false) {
					Content.Add(" style=\"display:block;\" ");
				}
			}
			Content.Add(">");
			Content.Add(this.ConditionText);
			Content.Add("</span>");
			if (IsReverseSelectionEnabled) {
				if (this.ConditionCheckBox.Value == true) {
					this.Style.Display = DisplayMethod.Hidden;
				} else {
					this.Style.Display = DisplayMethod.Block;
				}
			} else {
				if (this.ConditionCheckBox.Value == false) {
					this.Style.Display = DisplayMethod.Hidden;
				} else {
					this.Style.Display = DisplayMethod.Block;
				}
			}

			Text.ReadOnly = this.ReadOnly;
			Text.Disabled = this.Disabled;
			Text.SetStyle(this.Style);
			Content.Add(Text.Draw);

			PopupControl Popup = new PopupControl(this.ID + "tooltip", true, ToolTipMessage);
			Popup.AutoCloseDuration = 1000;
			Label Div = new Label(this.ID + "_Info", "...");
			Popup.Hide();
			Popup.SetDependentControl(Div.ID);
			Content.Add(Popup.Draw);
			Div.Style.Class = "FilterBoxDotsStyle";
			Div.DrawingType = LabelDrawingType.Span;
			Div.Style.PositionStyle = Position.Absolute;
			Div.Style.Float = FloatType.Right;
			Div.Style.Left = 3;
			Div.Style.CursorStyle = Cursor.Default;
			Div.Style.SetPadding(3, , 3);
			Div.OnMouseOverEvent = Popup.ShowEvent;
			//If Me.ConditionCheckBox.Value = False OrElse IsReverseSelectionEnabled Then
			Div.Style.Display = DisplayMethod.Hidden;
			//Else
			//Div.Style.Display = DisplayMethod.Block
			//End If
			Content.Add(Div.Draw);
			Content.Add("<div id=\"" + this.ID + "Container\" style=\"position:absolute;display:none;z-index:9999\"></div>");
			Content.Add("</div>");
		}
		private void AddFunctions()
		{
			if (this.Script.Function("ShowHideFBGrid") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function ShowHideFBGrid = this.Script.AddFunction("ShowHideFBGrid", "", "element,show");
				ShowHideFBGrid.AppendLine("var containerelement=document.getElementById(element.id + 'Container');");
				ShowHideFBGrid.AppendLine("if (show && containerelement != undefined)");
				ShowHideFBGrid.AppendLine("    containerelement.style.display = 'block'");
				ShowHideFBGrid.AppendLine("else if (!show && containerelement != undefined)");
				ShowHideFBGrid.AppendLine("    containerelement.style.display = 'none';");
			}

			if (this.Script.Function("HideFBGridInThread") == null) {
				Script.AppendLine("var HideFBGridInThreadElement;");
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function ShowHideFBGridInThread = this.Script.AddFunction("HideFBGridInThread", "", "element");
				ShowHideFBGridInThread.AppendLine("if (FBInterval != undefined) {");
				ShowHideFBGridInThread.AppendLine("    window.clearInterval(FBInterval);");
				ShowHideFBGridInThread.AppendLine("}");
				ShowHideFBGridInThread.AppendLine("HideFBGridInThreadElement = element;");
				ShowHideFBGridInThread.AppendLine("FBInterval = window.setInterval('HideFBGridInThreadCallback(HideFBGridInThreadElement)', 400);");
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function HideFBGridInThreadCallback = this.Script.AddFunction("HideFBGridInThreadCallback", "", "element");
				HideFBGridInThreadCallback.AppendLine("if (FBInterval != undefined) {");
				HideFBGridInThreadCallback.AppendLine("    window.clearInterval(FBInterval);");
				HideFBGridInThreadCallback.AppendLine("}");
				HideFBGridInThreadCallback.AppendLine("ShowHideFBGrid(HideFBGridInThreadElement,false);");
				HideFBGridInThreadCallback.AppendLine("if (element.value == '') { document.getElementById(element.id + 'SelectedID').value= '0';};");
			}

			if (this.Script.Function("FunctionTrim") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function FunctionTrim = this.Script.AddFunction("TrimStr", "", "tString");
				FunctionTrim.AppendLine("return tString.replace(/^\\s+|\\s+$/, '');");
			}



			if (this.Script.Function("FindAnyValueOfSubCells") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function FindAnyValueOfSubCells = this.Script.AddFunction("FindAnyValueOfSubCells", "", "element");
				FindAnyValueOfSubCells.AppendLine("var tempValue = undefined;");
				FindAnyValueOfSubCells.AppendLine("for (var i = 0; i < element.childNodes.length; i++) {");
				FindAnyValueOfSubCells.AppendLine("     var subElement = element.childNodes[i];");
				FindAnyValueOfSubCells.AppendLine("     if (subElement.childNodes.length == 0) {");
				FindAnyValueOfSubCells.AppendLine("         if (subElement.toString() == '[object Text]' || (subElement.nodeName != undefined && subElement.nodeName.indexOf('text') > -1)) {");
				FindAnyValueOfSubCells.AppendLine("             if (subElement.toString() == '[object Text]' && this.TrimStr(subElement.data) == '') {continue;}");
				FindAnyValueOfSubCells.AppendLine("             else {tempValue = this.TrimStr(subElement.data);}");
				FindAnyValueOfSubCells.AppendLine("         }");
				FindAnyValueOfSubCells.AppendLine("         else if (subElement.value == undefined) { tempValue = this.TrimStr(subElement.innerHTML) }");
				FindAnyValueOfSubCells.AppendLine("         else { tempValue = this.TrimStr(subElement.value); }");
				FindAnyValueOfSubCells.AppendLine("     }");
				FindAnyValueOfSubCells.AppendLine("     else {tempValue = FindAnyValueOfSubCells(subElement); }");
				FindAnyValueOfSubCells.AppendLine("     if (tempValue != undefined){break;}");
				FindAnyValueOfSubCells.AppendLine("}");
				FindAnyValueOfSubCells.AppendLine("return tempValue;");
			}

			if (this.Script.Function("HasNavigationEvent") == null) {
				this.Script.AppendLine("var CurFBRow;");
				this.Script.AppendLine("var FBSelectedRows = new Array();");
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function HasNavigationEvent = this.Script.AddFunction("HasNavigationEvent", "", "textbox,e");
				HasNavigationEvent.AppendLine("var code = e.keyCode ? e.keyCode : e.which;");
				HasNavigationEvent.AppendLine("var currentTable = document.getElementById('CB_' + textbox.id)");
				HasNavigationEvent.AppendLine("if (currentTable ==undefined) return false;");
				HasNavigationEvent.AppendLine("var rows = currentTable.getElementsByTagName('tr');");
				HasNavigationEvent.AppendLine("if (CurFBRow < -1 || CurFBRow == undefined) {CurFBRow =-1;}");
				HasNavigationEvent.AppendLine("if (CurFBRow < -1 || (CurFBRow == -1 && rows!=undefined && rows.length > 0 && rows[0].className.indexOf('RowStyle') < 0)) CurFBRow = 0;");
				HasNavigationEvent.AppendLine("var result = false;");
				HasNavigationEvent.AppendLine("var _row;");
				HasNavigationEvent.AppendLine("for (var i = FBSelectedRows.length - 1; i >= 0; i--) {");
				HasNavigationEvent.AppendLine("     _row = FBSelectedRows[i];");
				HasNavigationEvent.AppendLine("     _row.className = _row.className.replace(' SelectedFBRow', '');");
				HasNavigationEvent.AppendLine("FBSelectedRows.pop();");
				HasNavigationEvent.AppendLine("}");
				HasNavigationEvent.AppendLine("if (code == 13)");
				HasNavigationEvent.AppendLine("{");
				HasNavigationEvent.AppendLine("    if (CurFBRow < -1 || (CurFBRow == 0 && rows!=undefined && rows.length > 0 && rows[0].className.indexOf('RowStyle') < 0)) return true;");
				HasNavigationEvent.AppendLine("    tempRow = rows[CurFBRow];");
				HasNavigationEvent.AppendLine("    return SelectFBRow(tempRow);");
				HasNavigationEvent.AppendLine("}");
				HasNavigationEvent.AppendLine("else if (code == 27){ ShowHideFBGrid(FBLastElement,false);return true;}");
				HasNavigationEvent.AppendLine("else if (code == 40)");
				HasNavigationEvent.AppendLine("{");
				HasNavigationEvent.AppendLine("         CurFBRow++;");
				HasNavigationEvent.AppendLine("         if (CurFBRow >= rows.length) CurFBRow = rows.length - 1;");
				HasNavigationEvent.AppendLine("         result = true;");
				HasNavigationEvent.AppendLine("}");
				HasNavigationEvent.AppendLine("else if (code == 38)");
				HasNavigationEvent.AppendLine("{");
				HasNavigationEvent.AppendLine("         CurFBRow--;");
				HasNavigationEvent.AppendLine("         if (CurFBRow < 0) CurFBRow = 0;");
				HasNavigationEvent.AppendLine("         if (CurFBRow < 1 && rows!=undefined && rows.length > 0 && rows[0].className.indexOf('RowStyle') < 0) CurFBRow = 1;");
				HasNavigationEvent.AppendLine("         result = true;");
				HasNavigationEvent.AppendLine("}");
				HasNavigationEvent.AppendLine("if (CurFBRow == 0 && rows!=undefined && rows.length > 0 && rows[0].className.indexOf('RowStyle') < -1) return true;");
				HasNavigationEvent.AppendLine("if (rows!=undefined && rows.length > 0 ) tempRow = rows[CurFBRow];");
				HasNavigationEvent.AppendLine("if (tempRow != undefined){");
				HasNavigationEvent.AppendLine("if (tempRow.className.indexOf('SelectedFBRow') < 0)");
				HasNavigationEvent.AppendLine("     {tempRow.className = tempRow.className + ' SelectedFBRow';FBSelectedRows.push(tempRow);}}");
				HasNavigationEvent.AppendLine("if (result) ShowHideFBGrid(FBLastElement,true);");
				HasNavigationEvent.AppendLine("return result;");
			}

			if (this.Script.Function("SelectFBRow") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function SelectFBRow = this.Script.AddFunction("SelectFBRow", "", "row");
				SelectFBRow.AppendLine("if (row !=tempRow) tempRow = row;");
				SelectFBRow.AppendLine("if (tempRow == undefined) return false;");
				SelectFBRow.AppendLine("var cells= tempRow.getElementsByTagName('td');");
				SelectFBRow.AppendLine("if (cells.length > 0)");
				SelectFBRow.AppendLine("{");
				SelectFBRow.AppendLine("     var cell = cells[0];");
				SelectFBRow.AppendLine("     FBLastElement.value = FindAnyValueOfSubCells(cell);");
				SelectFBRow.AppendLine("     var hiddenSelectedID =  document.getElementById(FBLastElement.id + 'SelectedID');");
				SelectFBRow.AppendLine("     var hiddenSelectedIDvalue = tempRow.id.toString().replace('CB_' + FBLastElement.id + 'Form_Row_ID_', '');");
				SelectFBRow.AppendLine("     if (hiddenSelectedID.value != hiddenSelectedIDvalue){");
				SelectFBRow.AppendLine("         hiddenSelectedID.value = hiddenSelectedIDvalue;");
				SelectFBRow.AppendLine("         if (hiddenSelectedID.onchange != undefined)");
				SelectFBRow.AppendLine("                hiddenSelectedID.onchange();");
				SelectFBRow.AppendLine("     }");
				SelectFBRow.AppendLine("     ShowHideFBGrid(FBLastElement,false);");
				SelectFBRow.AppendLine("return true;");
				SelectFBRow.AppendLine("}");
				SelectFBRow.AppendLine("return false;");
			}

			if (this.Script.Function("FBFocusControl") == null) {
				this.Script.AppendLine("var FBInterval;");
				this.Script.AppendLine("var FBLastValue;");
				this.Script.AppendLine("var FBTempValue;");
				this.Script.AppendLine("var FBLastElement;");
				this.Script.AppendLine("var TempCdef;");
				this.Script.AppendLine("var tempRow;");
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function FBFocusControl = this.Script.AddFunction("FBFocusControl", "", "textbox");
				FBFocusControl.AppendLine("if (tempRow != undefined) {tempRow.className = tempRow.className.replace(' SelectedFBRow', '');}");
				FBFocusControl.AppendLine("FBTempValue = textbox.value;FBLastValue=FBTempValue;");
				FBFocusControl.AppendLine("FBLastElement = textbox;CurFBRow=-1;");
			}

			if (this.Script.Function("FBKeyPress") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function FBKeyPress = this.Script.AddFunction("FBKeyPress", "", "textbox,e,Cdef,Columns");
				FBKeyPress.AppendLine("if (!HasNavigationEvent(textbox,e))");
				FBKeyPress.AppendLine("{");
				FBKeyPress.AppendLine("     if (FBInterval != undefined) {");
				FBKeyPress.AppendLine("         window.clearInterval(FBInterval);");
				FBKeyPress.AppendLine("     }");
				FBKeyPress.AppendLine("     TempCdef = Cdef;");
				FBKeyPress.AppendLine("     FBTempValue = textbox.value;");
				FBKeyPress.AppendLine("     if (FBLastElement == undefined || textbox.id !=FBLastElement.id) {FBLastValue = undefined;}");
				FBKeyPress.AppendLine("         FBLastElement = textbox;");
				FBKeyPress.AppendLine("         FBInterval = window.setInterval('SearchWithFilterBox()', 400);");
				FBKeyPress.AppendLine("}");

			}

			if (this.Script.Function("FBKeyDown") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function FBKeyDown = this.Script.AddFunction("FBKeyDown", "", "textbox,e");
				FBKeyDown.AppendLine("var code = e.keyCode ? e.keyCode : e.which;");
				FBKeyDown.AppendLine("if (code == 13 && FBLastElement != undefined)");
				FBKeyDown.AppendLine("{");
				FBKeyDown.AppendLine("    var hiddenSelectedID =  document.getElementById(FBLastElement.id + 'SelectedID');");
				FBKeyDown.AppendLine("    var tempSelectedValue = hiddenSelectedID.value;");
				FBKeyDown.AppendLine("    HasNavigationEvent(textbox,e);");
				FBKeyDown.AppendLine("    return tempSelectedValue == hiddenSelectedID.value;");
				FBKeyDown.AppendLine("}");
				FBKeyDown.AppendLine("return true;");
			}

			if (this.Script.Function("SearchWithFilterBox") == null) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.Function SearchWithFilterBox = this.Script.AddFunction("SearchWithFilterBox");
				SearchWithFilterBox.AppendLine("if ((FBLastValue != FBTempValue || FBLastValue == undefined) && this.TrimStr(FBTempValue) != ''){");
				SearchWithFilterBox.AppendLine("    window.clearInterval(FBInterval);");
				SearchWithFilterBox.AppendLine("    FBLastValue = FBTempValue;CurFBRow = -1;");
				SearchWithFilterBox.AppendLine("    ReloadFilterBox(FBLastElement.id,TempCdef); ");
				SearchWithFilterBox.AppendLine("}else");
				SearchWithFilterBox.AppendLine("{");
				SearchWithFilterBox.AppendLine("    window.clearInterval(FBInterval);");
				SearchWithFilterBox.AppendLine("    if (FBTempValue == '')");
				SearchWithFilterBox.AppendLine("    {");
				SearchWithFilterBox.AppendLine("        var hiddenSelectedID =  document.getElementById(FBLastElement.id + 'SelectedID');");
				SearchWithFilterBox.AppendLine("        hiddenSelectedID.value = 0;");
				SearchWithFilterBox.AppendLine("    }");
				SearchWithFilterBox.AppendLine("}");
			}
		}
		public ConditionalFilterBox(string ID) : base(ID)
		{
		}
		public ConditionalFilterBox(string ID, string MemberNameToFilter) : base(ID, MemberNameToFilter)
		{
		}
		public ConditionalFilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource) : base(ID, MemberNameToFilter, DataSource)
		{
		}
		public ConditionalFilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember) : base(ID, MemberNameToFilter, DataSource, DisplayMember)
		{
		}
		public ConditionalFilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember, FilterBoxDatasource FilterBoxDataSource) : base(ID, MemberNameToFilter, DataSource, DisplayMember, FilterBoxDataSource)
		{
		}
	}
}
