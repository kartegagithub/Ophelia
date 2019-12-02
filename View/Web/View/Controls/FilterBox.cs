using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class FilterBox : InputDataControl
	{
		private string ParameterMemberNames;
		private FilterBoxDatasource oFilterBoxDataSource;
		//Yarım kalan kısım
		private string sDisplayMember = "Name";
		private string sValueMember = "ID";
		private string sSelectedBackgroundColor = "black";
		private string sSelectedTextColor = "white";
		private Binders.CollectionBinder oBinder;
		private string sMemberNameToFilter = "Name";
		private Entity oSelectedEntity;
		private bool SelectedEntityCanBeLoaded = true;
			//"Arama yapmak için en az 1 karakter girmelisiniz."
		private string sToolTipMessage = "";
		private string sNoRowMessage = "";
		public Ophelia.Model.Model SelectedModel;
		public FilterBoxDatasource FilterBoxDataSource {
			get { return this.oFilterBoxDataSource; }
			set { this.oFilterBoxDataSource = value; }
		}
		public string NoRowMessage {
			get { return this.sNoRowMessage; }
			set { this.sNoRowMessage = value; }
		}
		public string ToolTipMessage {
			get { return this.sToolTipMessage; }
			set { this.sToolTipMessage = value; }
		}
		public static int SelectedID {
			get { return UI.Current.QueryString.IntegerValue(ID + "SelectedID"); }
		}
		public int SelectedID {
			get { return this.Page.QueryString.IntegerValue(this.ID + "SelectedID"); }
		}
		public Entity SelectedEntity {
			get {
				if (this.SelectedID > 0 && this.oSelectedEntity == null && this.SelectedEntityCanBeLoaded) {
					bool DatasourceIsEntityCollection = this.DataSource.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.EntityCollection"));
					if (DatasourceIsEntityCollection) {
						this.oSelectedEntity = this.Page.Client.Application.GetCollection(((EntityCollection)this.DataSource).Definition).FilterBy("ID", SelectedID).FirstEntity;
					}
					this.SelectedEntityCanBeLoaded = false;
				}
				return this.oSelectedEntity;
			}
			set { this.oSelectedEntity = value; }
		}
		public string MemberNameToFilter {
			get { return this.sMemberNameToFilter; }
			set { this.sMemberNameToFilter = value; }
		}
		public Binders.CollectionBinder Binder {
			get {
				if (this.oBinder == null) {
					this.oBinder = new Binders.CollectionBinder("F_Grid");
					this.oBinder.Columns.AddTextBoxColumn(this.sDisplayMember).Style.Width = -1;
				}
				return this.oBinder;
			}
		}
		public Binders.EntityColumnCollection Columns {
			get { return this.Binder.Columns; }
		}
		public string DisplayMember {
			get { return this.sDisplayMember; }
			set {
				this.Binder.Columns.Remove(this.sDisplayMember);
				this.sDisplayMember = value;
				if (!string.IsNullOrEmpty(value)) {
					this.Binder.Columns.AddTextBoxColumn(this.sDisplayMember).Style.Width = -1;
				}
			}
		}
		public virtual ICollection DataSource {
			get { return this.Binder.Binding.Value; }
			set { this.Binder.Binding.Value = value; }
		}
		internal string GetDataSourceInString()
		{
			JSON.JSONEntityCollection BinderJS = new JSON.JSONEntityCollection("binder");
			if (this.DataSource == null) {
				return "";
			}
			JSON.JSONEntity JSEntity = null;
			JSEntity = BinderJS.Add();
			JSEntity.SetData("searchfilter", this.MemberNameToFilter);
			JSEntity.SetData("norowmessage", this.NoRowMessage);
			bool DatasourceIsEntityCollection = this.DataSource.GetType().IsSubclassOf(Type.GetType("Ophelia.Application.Base.EntityCollection"));
			JSEntity.SetData("datasourceisentitycollection", DatasourceIsEntityCollection);

			JSEntity = BinderJS.Add;
			this.DrawColumns(ref JSEntity);

			if (DatasourceIsEntityCollection) {
				JSEntity = this.EntityCollectionToJSON(((EntityCollection)this.DataSource).Definition);
				BinderJS.Add(JSEntity);
			}


			return Page.HttpContext.Server.UrlEncode(Functions.EncryptWithEncoding(System.Text.Encoding.UTF8, BinderJS.Draw(), "FilterBox"));
		}
		private void DrawColumns(ref JSON.JSONEntity JS)
		{
			JSON.JSONEntityCollection ColumnsJS = new JSON.JSONEntityCollection("columns");
			for (int i = 0; i <= this.Columns.Count - 1; i++) {
				JSON.JSONEntity ColumnJS = ColumnsJS.Add();
				if (ColumnJS.GetType.Name == "ComboBoxColumn") {
					ColumnJS.SetData("Type", "Ophelia.Web.View.Base.DataGrid.TextBoxColumn");
				} else {
					ColumnJS.SetData("Type", this.Columns(i).GetType.FullName);
				}
				ColumnJS.SetData("MemberName", this.Columns(i).MemberName);
				ColumnJS.SetData("MemberNameToBeDrawn", this.Columns(i).MemberNameToBeDrawn);
				ColumnJS.SetData("Name", this.Columns(i).Name);
				ColumnJS.SetData("ReadOnly", this.Columns(i).ReadOnly);
				ColumnJS.SetData("Suffix", this.Columns(i).Suffix);
				ColumnJS.SetData("Width", this.Columns(i).Style.Width);
				if (this.Columns(i).GetType.FullName.IndexOf("NumberBox") > -1) {
					ColumnJS.SetData("DecimalDigits", ((Base.DataGrid.NumberBoxColumn)this.Columns(i)).DecimalDigits);
				}
			}
			JS.SetData("columns", ColumnsJS);
		}
		private JSON.JSONEntity EntityCollectionToJSON(EntityCollectionDefinition Definition)
		{
			JSON.JSONEntity JSEntity = new JSON.JSONEntity("collection");
			if (DataSource == null)
				return JSEntity;
			JSEntity.SetData("EntityType", Definition.EntityDefinition.EntityType.FullName);
			JSEntity.SetData("PageSize", Definition.PageSize);
			JSEntity.SetData("Distinct", Definition.Distinct);
			JSON.JSONEntityCollection FiltersJSCollection = new JSON.JSONEntityCollection("filters");
			JSON.JSONEntity FilterJS = default(JSON.JSONEntity);
			FilterCollection Filters = Definition.Filters;
			for (int i = 0; i <= Filters.Count - 1; i++) {
				FilterJS = FiltersJSCollection.Add();
				FilterJS.SetData("Name", Filters(i).Name);
				FilterJS.SetData("AsIs", Filters(i).AsIs);
				FilterJS.SetData("Comparison", Filters(i).Comparison);
				FilterJS.SetData("Constraint", Filters(i).Constraint);
				FilterJS.SetData("Excludes", Filters(i).Excludes);
				FilterJS.SetData("Prefix", Filters(i).Prefix);
				FilterJS.SetData("Suffix", Filters(i).Suffix);
				JSON.JSONEntityCollection FiltersParametersJSCollection = new JSON.JSONEntityCollection("parameters");
				JSON.JSONEntity FiltersParameterJS = default(JSON.JSONEntity);
				for (int j = 0; j <= Filters(i).Parameters.Count - 1; j++) {
					FiltersParameterJS = FiltersParametersJSCollection.Add;
					if (Filters(i).Parameters(j).Value.GetType.FullName == "Ophelia.Application.Base.EntityCollectionDefinition") {
						JSON.JSONEntityCollection JSCollection = new JSON.JSONEntityCollection();
						JSCollection.Add(this.EntityCollectionToJSON(Filters(i).Parameters(j).Value));
						FiltersParameterJS.SetData("value", JSCollection);
					} else {
						FiltersParameterJS.SetData("Value", Filters(i).Parameters(j).Value);
					}

				}
				FilterJS.SetData("parameters", FiltersParametersJSCollection);
			}
			JSEntity.SetData("filters", FiltersJSCollection);

			JSON.JSONEntityCollection SortersJSCollection = new JSON.JSONEntityCollection("sorters");
			JSON.JSONEntity SorterJS = default(JSON.JSONEntity);
			SorterCollection Sorters = Definition.Sorters;
			for (int i = 0; i <= Sorters.Count - 1; i++) {
				SorterJS = SortersJSCollection.Add;
				SorterJS.SetData("Direction", Sorters(i).Direction);
				SorterJS.SetData("Name", Sorters(i).Name);
				SorterJS.SetData("NullsOrder", Sorters(i).NullsOrder);
			}
			JSEntity.SetData("sorters", SortersJSCollection);

			JSON.JSONEntityCollection GroupersJSCollection = new JSON.JSONEntityCollection("groupers");
			JSON.JSONEntity GrouperJS = default(JSON.JSONEntity);
			GrouperCollection Groupers = Definition.Groupers;
			for (int i = 0; i <= Groupers.Count - 1; i++) {
				GrouperJS = GroupersJSCollection.Add;
				GrouperJS.SetData("Name", Groupers(i).Name);
			}
			JSEntity.SetData("groupers", GroupersJSCollection);


			JSON.JSONEntityCollection RequestedPropertiesJSCollection = new JSON.JSONEntityCollection("requestedproperties");
			JSON.JSONEntity RequestedPropertyJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection RequestedProperties = Definition.RequestedProperties;
			for (int i = 0; i <= RequestedProperties.Count - 1; i++) {
				RequestedPropertyJS = RequestedPropertiesJSCollection.Add;
				RequestedPropertyJS.SetData("AccessType", RequestedProperties(i).AccessType);
				RequestedPropertyJS.SetData("Alias", RequestedProperties(i).Alias);
				RequestedPropertyJS.SetData("Name", RequestedProperties(i).Name);
			}
			JSEntity.SetData("requestedproperties", RequestedPropertiesJSCollection);

			JSON.JSONEntityCollection PropertiesRestrictedToJSCollection = new JSON.JSONEntityCollection("propertiesrestrictedto");
			JSON.JSONEntity PropertyRestrictedToJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection PropertiesRestrictedTo = Definition.PropertiesRestrictedTo;
			for (int i = 0; i <= PropertiesRestrictedTo.Count - 1; i++) {
				PropertyRestrictedToJS = PropertiesRestrictedToJSCollection.Add;
				PropertyRestrictedToJS.SetData("AccessType", PropertiesRestrictedTo(i).AccessType);
				PropertyRestrictedToJS.SetData("Alias", PropertiesRestrictedTo(i).Alias);
				PropertyRestrictedToJS.SetData("Name", PropertiesRestrictedTo(i).Name);
			}
			JSEntity.SetData("propertiesrestrictedto", PropertiesRestrictedToJSCollection);

			JSON.JSONEntityCollection RelatedPropertiesJSCollection = new JSON.JSONEntityCollection("relatedproperties");
			JSON.JSONEntity RelatedPropertyJS = default(JSON.JSONEntity);
			Application.Base.PropertyCollection RelatedProperties = Definition.RelatedProperties;
			for (int i = 0; i <= RelatedProperties.Count - 1; i++) {
				RelatedPropertyJS = RelatedPropertiesJSCollection.Add;
				RelatedPropertyJS.SetData("AccessType", RelatedProperties(i).AccessType);
				RelatedPropertyJS.SetData("Alias", RelatedProperties(i).Alias);
				RelatedPropertyJS.SetData("Name", RelatedProperties(i).Name);
				RelatedPropertyJS.SetData("RelatedPropertyInfoName", RelatedProperties(i).RelatedPropertyInfo.Name);
				RelatedPropertyJS.SetData("RelatedPropertyPropertyTypeName", RelatedProperties(i).RelatedPropertyInfo.ReflectedType.FullName);
			}
			JSEntity.SetData("relatedproperties", RelatedPropertiesJSCollection);
			return JSEntity;
		}
		public string SelectedBackgroundColor {
			get { return this.sSelectedBackgroundColor; }
			set { this.sSelectedBackgroundColor = value; }
		}
		public string SelectedTextColor {
			get { return this.sSelectedTextColor; }
			set { this.sSelectedTextColor = value; }
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
			Content.Add(new HiddenBox(this.ID + "DataGridDef", this.GetDataSourceInString()).Draw);
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

			Text.ReadOnly = this.ReadOnly;
			Text.Disabled = this.Disabled;
			Text.SetStyle(this.Style);
			Content.Add(Text.Draw);

			Label Div = new Label(this.ID + "_Info", "...");
			Div.Style.Class = "FilterBoxDotsStyle";
			Div.DrawingType = LabelDrawingType.Span;
			Div.Style.PositionStyle = Position.Absolute;
			Div.Style.Float = FloatType.Right;
			Div.Style.Left = 3;
			Div.Style.CursorStyle = Cursor.Default;
			Div.Style.SetPadding(3, , 3);
			if (!string.IsNullOrEmpty(this.ToolTipMessage)) {
				PopupControl Popup = new PopupControl(this.ID + "tooltip", true, ToolTipMessage);
				Popup.AutoCloseDuration = 10;
				Popup.Hide();
				Popup.SetDependentControl(Div.ID);
				Content.Add(Popup.Draw);
				Div.OnMouseOverEvent = Popup.ShowEvent;
			}
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
				FBKeyDown.AppendLine(this.OnKeyDownEvent);
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
		public FilterBox(string ID)
		{
			this.ID = ID;
			oFilterBoxDataSource = new FilterBoxDatasource();
		}
		public FilterBox(string ID, string MemberNameToFilter) : this(ID)
		{
			this.sMemberNameToFilter = MemberNameToFilter;
			this.sDisplayMember = this.sMemberNameToFilter;
		}
		public FilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource) : this(ID, MemberNameToFilter)
		{
			this.DataSource = DataSource;
		}
		public FilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember) : this(ID, MemberNameToFilter, DataSource)
		{
			this.DisplayMember = DisplayMember;
		}
		public FilterBox(string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember, FilterBoxDatasource FilterBoxDataSource) : this(ID, MemberNameToFilter, DataSource)
		{
			this.oFilterBoxDataSource = FilterBoxDataSource;
			this.DisplayMember = DisplayMember;
		}
	}
}
