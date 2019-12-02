using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Binders.Fields;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders
{
	public class FieldCollection : Ophelia.Application.Base.CollectionBase
	{
		private Form.Form oForm;
		private Structure.Cells.Cell oCurrentCell;
		public Field FirstField {
			get { return this[0]; }
		}
		public Field LastField {
			get { return this[this.Count - 1]; }
		}
		public Structure.Cells.Cell CurrentSection {
			get {
				if (this.oCurrentCell == null) {
					this.oCurrentCell = this.Form.Layout.Cell(0, 0);
				}
				return this.oCurrentCell;
			}
		}
		public bool ChangeSection(int RowNumber, int ColumnNumber)
		{
			if (this.Form.Layout.Columns.Count >= ColumnNumber && this.Form.Layout.Rows.Count >= RowNumber) {
				this.oCurrentCell = this.Form.Layout.Cell(RowNumber, ColumnNumber);
				return true;
			}
			return false;
		}
		public Field this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public Field this[string Name] {
			get {
				int n = 0;
				for (n = 0; n <= this.Count - 1; n++) {
					if (this[n].MemberName == Name) {
						return this[n];
					}
				}
				return null;
			}
			set {
				int n = 0;
				for (n = 0; n <= this.Count - 1; n++) {
					if (this[n].MemberName == Name) {
						this[n] = value;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public virtual string GetFieldTitle(string MemberName)
		{
			string Word = null;
			if (!string.IsNullOrEmpty(MemberName) && this.Form.UseDictionary) {
				if (MemberName.IndexOf(".") > -1) {
					string[] Words = MemberName.Split(".");
					if (Words[Words.Length - 1] == "Name" || Words[Words.Length - 1] == "Count") {
						Word = Words[Words.Length - 2];
					} else {
						Word = Words[Words.Length - 1];
					}
				} else {
					Word = MemberName;
				}
				return this.Form.Dictionary.GetWord("Concept." + Word);
			}
			return MemberName;
		}
		public Form.Form Form {
			get { return this.oForm; }
		}
		public void SetNumberBoxFieldSuffix(string Suffix)
		{
			for (int n = 0; n <= this.Count - 1; n++) {
				if (this[n].Control.GetType.Name == "NumberBox") {
					((NumberBox)this[n].Control).Suffix = Suffix;
				}
			}
		}
		public TextBoxField AddTextBox(string MemberName)
		{
			return this.AddTextBox(MemberName, "");
		}
		public TextBoxField AddTextBox(string MemberName, string Value)
		{
			return this.AddTextBox(MemberName, Value, -1);
		}
		public TextBoxField AddTextBox(string Title, string MemberName, string Value)
		{
			return this.AddTextBox(Title, MemberName, Value, -1);
		}
		public TextBoxField AddTextBox(string MemberName, string Value, int Width)
		{
			return this.AddTextBox(this.GetFieldTitle(MemberName), MemberName, Value, Width);
		}
		public TextBoxField AddTextBox(string Title, string MemberName, string Value, int Width)
		{
			TextBoxField Field = this.CreateTextBoxField(Title, MemberName);
			Field.Control.Value = Value;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public NumberBoxField AddInfinityNumberBox(string MemberName)
		{
			return this.AddNumberBox(MemberName, "0");
		}
		public NumberBoxField AddInfinityNumberBox(string MemberName, string Value)
		{
			return this.AddNumberBox(MemberName, Value, -1);
		}
		public NumberBoxField AddInfinityNumberBox(string MemberName, string Value, int Width)
		{
			return this.AddNumberBox(this.GetFieldTitle(MemberName), MemberName, Value, Width);
		}
		public NumberBoxField AddInfinityNumberBox(string Title, string MemberName, string Value, int Width)
		{
			NumberBoxField Field = this.CreateInfinityNumberBoxField(Title, MemberName);
			Field.Control.Value = Value;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public NumberBoxField AddNumberBox(string MemberName)
		{
			return this.AddNumberBox(MemberName, "0");
		}
		public NumberBoxField AddNumberBox(string MemberName, string Value)
		{
			return this.AddNumberBox(MemberName, Value, -1);
		}
		public NumberBoxField AddNumberBox(string MemberName, string Value, int Width)
		{
			return this.AddNumberBox(this.GetFieldTitle(MemberName), MemberName, Value, Width);
		}
		public NumberBoxField AddNumberBox(string Title, string MemberName, string Value, int Width)
		{
			NumberBoxField Field = this.CreateNumberBoxField(Title, MemberName);
			Field.Control.Value = Value;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public FilterBoxField AddFilterBoxField(string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember)
		{
			FilterBoxField FilterBoxField = this.AddFilterBoxField(this.GetFieldTitle(ID), ID, MemberNameToFilter, DataSource);
			FilterBoxField.Control.DisplayMember = DisplayMember;
			return FilterBoxField;
		}
		public FilterBoxField AddFilterBoxField(string Title, string ID, string MemberNameToFilter, EntityCollection DataSource, string DisplayMember)
		{
			FilterBoxField FilterBoxField = this.AddFilterBoxField(Title, ID, MemberNameToFilter, DataSource);
			FilterBoxField.Control.DisplayMember = DisplayMember;
			return FilterBoxField;
		}
		public FilterBoxField AddFilterBoxField(string Title, string ID, string MemberNameToFilter, EntityCollection DataSource)
		{
			FilterBoxField FilterBoxField = this.AddFilterBoxField(Title, ID, MemberNameToFilter);
			FilterBoxField.Control.DataSource = DataSource;
			return FilterBoxField;
		}
		public FilterBoxField AddFilterBoxField(string Title, string ID, string MemberNameToFilter)
		{
			FilterBoxField FilterBoxField = this.AddFilterBoxField(Title, ID);
			FilterBoxField.Control.MemberNameToFilter = MemberNameToFilter;
			FilterBoxField.Control.DisplayMember = MemberNameToFilter;
			return FilterBoxField;
		}
		public FilterBoxField AddFilterBoxField(string Title, string ID)
		{
			return this.Add(this.CreateFilterBoxField(Title, ID));
		}
		public ColorPickerField AddColorPicker(string MemberName, string Value)
		{
			return this.AddColorPicker(MemberName, Value, -1);
		}
		public ColorPickerField AddColorPicker(string MemberName, string Value, int Width)
		{
			return this.AddColorPicker(this.GetFieldTitle(MemberName), MemberName, Value, Width);
		}
		public ColorPickerField AddColorPicker(string Title, string MemberName, string Value, int Width)
		{
			ColorPickerField Field = this.CreateColorField(Title, MemberName);
			Field.Control.Value = Value;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public LinkField AddLink(string MemberName)
		{
			return this.AddLink(MemberName, "");
		}
		public LinkField AddLink(string MemberName, string Value)
		{
			return this.AddLink(MemberName, Value, 80);
		}
		public LinkField AddLink(string MemberName, string Value, string Url)
		{
			return this.AddLink(MemberName, Value, Url, -1);
		}
		public LinkField AddLink(string MemberName, string Value, int Width)
		{
			return this.AddLink(MemberName, Value, "", Width);
		}
		public LinkField AddLink(string MemberName, string Value, string Url, int Width)
		{
			return this.AddLink(this.GetFieldTitle(MemberName), MemberName, Value, Url, Width);
		}
		public LinkField AddLink(string Title, string MemberName, string Value, string Url, int Width)
		{
			LinkField Field = this.CreateLinkField(Title, MemberName);
			Field.Control.Value = Value;
			Field.Control.Url = Url;
			Field.Control.Style.Width = Width;
			return this.Add(Field);
		}
		public SelectBoxField AddSelectBox(string MemberName)
		{
			return this.AddSelectBox(MemberName, null);
		}
		public SelectBoxField AddSelectBox(string MemberName, ICollection Collection)
		{
			return this.AddSelectBox(MemberName, Collection, null, -1);
		}
		public SelectBoxField AddSelectBox(string MemberName, ICollection Collection, object Value, string ValueMember = "")
		{
			return this.AddSelectBox(MemberName, Collection, Value, -1, ValueMember);
		}
		public SelectBoxField AddSelectBox(string MemberName, ICollection Collection, object Value, int Width, string ValueMember = "")
		{
			return this.AddSelectBox(this.GetFieldTitle(MemberName), MemberName, Collection, Value, false, Width, ValueMember);
		}
		public SelectBoxField AddSelectBox(string Title, string MemberName, ICollection Collection, object Value, bool CreateBlankOption, int Width, string ValueMember = "")
		{
			SelectBoxField Field = this.CreateSelectBoxField(Title, MemberName);
			Field.Control.CreateBlankOption = CreateBlankOption;
			Field.Control.ValueMember = ValueMember;
			Field.Control.DataSource = Collection;
			Field.Control.SelectedItem = Value;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public RadioButtonField AddRadioButton(string MemberName)
		{
			return this.AddRadioButton(MemberName, null);
		}
		public RadioButtonField AddRadioButton(string MemberName, Ophelia.Application.Base.CollectionBase Collection)
		{
			return this.AddRadioButton(MemberName, Collection, null, -1);
		}
		public RadioButtonField AddRadioButton(string MemberName, Ophelia.Application.Base.CollectionBase Collection, object Value)
		{
			return this.AddRadioButton(MemberName, Collection, Value, -1);
		}
		public RadioButtonField AddRadioButton(string MemberName, Ophelia.Application.Base.CollectionBase Collection, object Value, int Width)
		{
			return this.AddRadioButton(this.GetFieldTitle(MemberName), MemberName, Collection, Value, "", Width);
		}
		public RadioButtonField AddRadioButton(string Title, string MemberName, Ophelia.Application.Base.CollectionBase Collection, object Value, string BlankOptionMessage, int Width)
		{
			RadioButtonField Field = this.CreateRadioButtonField(Title, MemberName);
			Field.Control.Collection = Collection;
			Field.Control.SelectedItem = Value;
			Field.Control.BlankOptionMessage = BlankOptionMessage;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public CheckBoxListField AddCheckBoxList(string MemberName, Ophelia.Application.Base.CollectionBase Collection, object Value)
		{
			return this.AddCheckBoxList(this.GetFieldTitle(MemberName), MemberName, Collection, Value);
		}
		public CheckBoxListField AddCheckBoxList(string Title, string MemberName, Ophelia.Application.Base.CollectionBase Collection, Ophelia.Application.Base.CollectionBase Value)
		{
			CheckBoxListField Field = this.CreateCheckBoxListField(Title, MemberName);
			Field.Control.Collection = Collection;
			Field.Control.SelectedItems = Value;
			//If Width > 0 Then
			//    Field.Control.Style.Width = Width
			//End If
			return this.Add(Field);
		}
		public GridField AddGrid(string MemberName, Ophelia.Application.Base.CollectionBase Collection)
		{
			return this.AddGrid(this.GetFieldTitle(MemberName), MemberName, Collection);
		}
		public GridField AddGrid(string Title, string MemberName, Ophelia.Application.Base.CollectionBase Collection)
		{
			GridField Field = this.CreateGridField(Title, MemberName);
			Field.Control.Collection = Collection;
			return this.Add(Field);
		}
		public PasswordField AddPasswordBox(string MemberName)
		{
			return this.AddPasswordBox(this.GetFieldTitle(MemberName), MemberName);
		}
		public PasswordField AddPasswordBox(string Title, string MemberName)
		{
			return this.AddPasswordBox(Title, MemberName, 120);
		}
		public PasswordField AddPasswordBox(string Title, string MemberName, int Width)
		{
			PasswordField Field = this.CreatePasswordBoxField(Title, MemberName);
			Field.Control.Style.Width = Width;
			return this.Add(Field);
		}
		public FileBoxWithAjaxField AddFileBoxWithAjax(string Title, string MemberName, string Value, string FileID, int Width)
		{
			FileBoxWithAjaxField Field = this.CreateFileBoxWithAjaxField(Title, MemberName);
			Field.Control.FileID = FileID;
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			if (!string.IsNullOrEmpty(Value)) {
				Field.Control.Value = Value;
			}
			return this.Add(Field);
		}
		public FileBoxField AddFileBox(string MemberName)
		{
			return this.AddFileBox(MemberName, 200);
		}
		public FileBoxField AddFileBox(string MemberName, string Value, int Width)
		{
			return this.AddFileBox(this.GetFieldTitle(MemberName), MemberName, Width, Value);
		}
		public FileBoxField AddFileBox(string MemberName, string Value)
		{
			return this.AddFileBox(this.GetFieldTitle(MemberName), MemberName, -1, Value);
		}
		public FileBoxField AddFileBox(string MemberName, int Width)
		{
			return this.AddFileBox(this.GetFieldTitle(MemberName), MemberName, Width, "");
		}
		public FileBoxField AddFileBox(string Title, string MemberName, int Width, string Value)
		{
			FileBoxField Field = this.CreateFileBoxField(Title, MemberName);
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			if (!string.IsNullOrEmpty(Value)) {
				Field.Control.Value = Value;
			}
			Field.Control.ResetToolTip = this.Form.Dictionary.GetWord("Command.Reset");
			Field.Control.ShowFileToolTip = this.Form.Dictionary.GetWord("Command.ShowFile");
			return this.Add(Field);
		}
		public TimeField AddTimeField(string MemberName)
		{
			return this.AddTimeField(MemberName, this.GetFieldTitle(MemberName), DateTime.MinValue);
		}
		public TimeField AddTimeField(string MemberName, DateTime Value)
		{
			return this.AddTimeField(MemberName, this.GetFieldTitle(MemberName), Value);
		}
		public TimeField AddTimeField(string MemberName, string Title, DateTime Value)
		{
			TimeField Field = new TimeField(this, Title, MemberName);
			Field.Control.Value = Value;
			this.Add(Field);
			return Field;
		}
		public DateTimePickerField AddDateTimePicker(string MemberName)
		{
			return this.AddDateTimePicker(MemberName, DateTime.MinValue);
		}
		public DateTimePickerField AddDateTimePicker(string MemberName, DateTime Value)
		{
			return this.AddDateTimePicker(MemberName, this.GetFieldTitle(MemberName), Value);
		}
		public DateTimePickerField AddDateTimePicker(string MemberName, string Title, DateTime Value)
		{
			DateTimePickerField Field = new DateTimePickerField(this, Title, MemberName);
			Field.Control.Value = Value;
			this.Add(Field);
			return Field;
		}
		public CheckBoxField AddCheckBox(string MemberName)
		{
			return this.AddCheckBox(MemberName, false);
		}
		public CheckBoxField AddCheckBox(string MemberName, bool Value)
		{
			return this.AddCheckBox(this.GetFieldTitle(MemberName), MemberName, Value, -1);
		}
		public CheckBoxField AddCheckBox(string Title, string MemberName, bool Value, int Width)
		{
			CheckBoxField Field = this.CreateCheckBoxField(Title, MemberName);
			Field.Control.Value = Value;
			Field.Control.Style.Width = Width;
			return this.Add(Field);
		}
		public ComboBoxField AddComboBox(string MemberName)
		{
			return this.AddComboBox(MemberName, -1);
		}
		public ComboBoxField AddComboBox(string MemberName, int Width)
		{
			return this.AddComboBox(this.GetFieldTitle(MemberName), MemberName, Width);
		}
		public ComboBoxField AddComboBox(string Title, string MemberName, int Width)
		{
			ComboBoxField Field = new ComboBoxField(this, Title, MemberName);
			if (Width > 0) {
				Field.Control.Style.Width = Width;
			}
			return this.Add(Field);
		}
		public LabelField AddEmptyField(string MemberName)
		{
			LabelField Field = this.CreateLabelField("", MemberName);
			//Field.Control.Value = "&nbsp;"
			return this.Add(Field);
		}
		public Form.Form AddHeading(string MemberName, string Value)
		{
			Form.Fields.AddEmptyField("");
			Form.Fields.LastField.Controls.Clear();
			Form.Fields.LastField.ShowHeader = false;
			Form.Fields.LastField.Controls.AddHeading(MemberName, Value);
			return Form;
		}
		public ConditionalFilterBoxField AddConditionalFilterBoxField(string Title, string ID, string MemberNameToFilter, EntityCollection DataSource, string ConditionText)
		{
			ConditionalFilterBoxField ConditionalFilterBoxField = this.CreateConditionalFilterBoxField(this.GetFieldTitle(Title), ID);
			ConditionalFilterBoxField.Control.DataSource = DataSource;
			ConditionalFilterBoxField.Control.MemberNameToFilter = MemberNameToFilter;
			//If Value IsNot Nothing Then
			//    ConditionalFilterBoxField.Control.ConditionCheckBox.Value = True
			//End If
			ConditionalFilterBoxField.Control.ConditionText = ConditionText;
			return this.Add(ConditionalFilterBoxField);
		}
		public ConditionalSelectBoxField AddConditionalSelectBoxField(string Title, string MemberName, ICollection Collection, object Value, bool CreateBlankOption, int Width, string ConditionText, string ValueMember = "")
		{
			ConditionalSelectBoxField ConditionalSelectBoxField = this.CreateConditionalSelectBoxField(this.GetFieldTitle(Title), MemberName);
			ConditionalSelectBoxField.Control.CreateBlankOption = CreateBlankOption;
			ConditionalSelectBoxField.Control.ValueMember = ValueMember;
			ConditionalSelectBoxField.Control.DataSource = Collection;
			ConditionalSelectBoxField.Control.SelectedItem = Value;
			if (Value != null) {
				ConditionalSelectBoxField.Control.ConditionCheckBox.Value = true;
			}
			if (Width > 0) {
				ConditionalSelectBoxField.Control.Style.Width = Width;
			}
			ConditionalSelectBoxField.Control.ConditionText = ConditionText;
			return this.Add(ConditionalSelectBoxField);
		}
		public ConditionalNumberBoxField AddConditionalNumberBoxField(string MemberName, string Value, string ConditionText)
		{
			ConditionalNumberBoxField ConditionalNumberBoxField = this.CreateConditionalNumberBoxField(this.GetFieldTitle(MemberName), MemberName);
			ConditionalNumberBoxField.Control.Value = Value;
			ConditionalNumberBoxField.Control.ConditionText = ConditionText;
			if (Value != null) {
				ConditionalNumberBoxField.Control.ConditionCheckBox.Value = true;
			}
			return this.Add(ConditionalNumberBoxField);
		}
		public ConditionalTextBoxField AddConditionalTextBoxField(string MemberName, string Value, string ConditionText)
		{
			ConditionalTextBoxField ConditionalTextBoxField = this.CreateConditionalTextBoxField(this.GetFieldTitle(MemberName), MemberName);
			ConditionalTextBoxField.Control.Value = Value;
			ConditionalTextBoxField.Control.ConditionText = ConditionText;
			if (Value != null && !string.IsNullOrEmpty(Value)) {
				ConditionalTextBoxField.Control.ConditionCheckBox.Value = true;
			}
			return this.Add(ConditionalTextBoxField);
		}
		public HiddenField AddHiddenBox(string MemberName)
		{
			return this.AddHiddenBox(MemberName, "1");
		}
		public HiddenField AddHiddenBox(string MemberName, string Value)
		{
			HiddenField HiddenField = this.CreateHiddenField("", MemberName);
			HiddenField.Control.Value = Value;
			return this.Add(HiddenField);
		}
		public LabelField AddLabel(string MemberName)
		{
			return this.AddLabel(MemberName, "");
		}
		public LabelField AddLabel(string MemberName, string Value)
		{
			return this.AddLabel(this.GetFieldTitle(MemberName), MemberName, Value);
		}
		public LabelField AddLabel(string Title, string MemberName, string Value)
		{
			LabelField LabelField = this.CreateLabelField(Title, MemberName);
			LabelField.Control.Value = Value;
			return this.Add(LabelField);
		}
		public TextAreaField AddTextArea(string MemberName)
		{
			return this.AddTextArea(this.GetFieldTitle(MemberName), MemberName, "");
		}
		public TextAreaField AddTextArea(string MemberName, string Value, int Width, int Height)
		{
			return this.AddTextArea(this.GetFieldTitle(MemberName), MemberName, Value, Width, Height);
		}
		public TextAreaField AddTextArea(string Title, string MemberName, string Value)
		{
			return this.AddTextArea(Title, MemberName, Value, 300, 100);
		}
		public TextAreaField AddTextArea(string MemberName, string Value)
		{
			return this.AddTextArea(this.GetFieldTitle(MemberName), MemberName, Value, 300, 100);
		}
		public TextAreaField AddTextArea(string Title, string MemberName, string Value, int Width, int Height)
		{
			TextAreaField TextAreaField = this.CreateTextAreaField(Title, MemberName);
			TextAreaField.Control.Style.Width = Width;
			TextAreaField.Control.Style.Height = Height;
			TextAreaField.Control.Value = Value;
			return this.Add(TextAreaField);
		}
		public ButtonField AddButton(string MemberName)
		{
			return this.AddButton(MemberName, this.GetFieldTitle(MemberName));
		}
		public ButtonField AddButton(string MemberName, string Value)
		{
			return this.AddButton(MemberName, Value, 50);
		}
		public ButtonField AddButton(string MemberName, string Value, int Width)
		{
			return this.AddButton(this.GetFieldTitle(MemberName), MemberName, Value, Button.ButtonType.Submit, Width);
		}
		public ButtonField AddButton(string Title, string MemberName, string Value, Button.ButtonType Type, int Width)
		{
			ButtonField Field = this.CreateButtonField("", MemberName);
			Field.Control.Type = Type;
			Field.Control.Value = Value;
			Field.Control.Style.Width = Width;
			return this.Add(Field);
		}
		public Field Add(Field Field)
		{
			this.List.Add(Field);
			Field.Section = this.CurrentSection;
			return Field;
		}
		private Field CreateField()
		{
			return new Field(this, "", "");
		}
		private TimeField CreateTimeField(string Title, string MemberName)
		{
			return new TimeField(this, Title, MemberName);
		}
		private ConditionalSelectBoxField CreateConditionalSelectBoxField(string Title, string MemberName)
		{
			return new ConditionalSelectBoxField(this, Title, MemberName);
		}
		private ConditionalNumberBoxField CreateConditionalNumberBoxField(string Title, string MemberName)
		{
			return new ConditionalNumberBoxField(this, Title, MemberName);
		}
		private ConditionalTextBoxField CreateConditionalTextBoxField(string Title, string MemberName)
		{
			return new ConditionalTextBoxField(this, Title, MemberName);
		}
		private ConditionalFilterBoxField CreateConditionalFilterBoxField(string Title, string MemberName)
		{
			return new ConditionalFilterBoxField(this, Title, MemberName);
		}
		private TextBoxField CreateTextBoxField(string Title, string MemberName)
		{
			return new TextBoxField(this, Title, MemberName);
		}
		private ColorPickerField CreateColorField(string Title, string MemberName)
		{
			return new ColorPickerField(this, Title, MemberName);
		}
		private NumberBoxField CreateInfinityNumberBoxField(string Title, string MemberName)
		{
			return new InfinityNumberBoxField(this, Title, MemberName);
		}
		private NumberBoxField CreateNumberBoxField(string Title, string MemberName)
		{
			return new NumberBoxField(this, Title, MemberName);
		}
		private PasswordField CreatePasswordBoxField(string Title, string MemberName)
		{
			return new PasswordField(this, Title, MemberName);
		}
		internal ButtonField CreateButtonField(string Title, string MemberName)
		{
			return new ButtonField(this, Title, MemberName);
		}
		private CheckBoxField CreateCheckBoxField(string Title, string MemberName)
		{
			return new CheckBoxField(this, Title, MemberName);
		}
		private CheckBoxListField CreateCheckBoxListField(string Title, string MemberName)
		{
			return new CheckBoxListField(this, Title, MemberName);
		}
		private GridField CreateGridField(string Title, string MemberName)
		{
			return new GridField(this, Title, MemberName);
		}
		private RadioButtonField CreateRadioButtonField(string Title, string MemberName)
		{
			return new RadioButtonField(this, Title, MemberName);
		}
		private FileBoxWithAjaxField CreateFileBoxWithAjaxField(string Title, string MemberName)
		{
			return new FileBoxWithAjaxField(this, Title, MemberName);
		}
		private FileBoxField CreateFileBoxField(string Title, string MemberName)
		{
			return new FileBoxField(this, Title, MemberName);
		}
		private HiddenField CreateHiddenField(string Title, string MemberName)
		{
			return new HiddenField(this, Title, MemberName);
		}
		private LinkField CreateLinkField(string Title, string MemberName)
		{
			return new LinkField(this, Title, MemberName);
		}
		private LabelField CreateLabelField(string Title, string MemberName)
		{
			return new LabelField(this, Title, MemberName);
		}
		private FilterBoxField CreateFilterBoxField(string Title, string MemberName)
		{
			return new FilterBoxField(this, Title, MemberName);
		}
		private SelectBoxField CreateSelectBoxField(string Title, string MemberName)
		{
			return new SelectBoxField(this, Title, MemberName);
		}
		private TextAreaField CreateTextAreaField(string Title, string MemberName)
		{
			return new TextAreaField(this, Title, MemberName);
		}
		public FieldCollection(Form.Form Form)
		{
			this.oForm = Form;
		}
	}
}

