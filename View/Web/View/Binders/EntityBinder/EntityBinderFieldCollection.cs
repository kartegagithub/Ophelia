using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Binders.Fields;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class EntityBinderFieldCollection : Ophelia.Application.Base.CollectionBase
	{
		public EntityBinder EntityBinder;
		public new EntityBinderField this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List(Index);
				}
				return null;
			}
			set {
				if (Index > -1 && Index < this.List.Count) {
					this.List(Index) = value;
				}
			}
		}
		public new Field this[string Name] {
			get {
				int n = 0;
				for (n = 0; n <= this.Count - 1; n++) {
					if (this[n].MemberName == Name) {
						return this[n].ControlField;
					}
				}
				return null;
			}
			set {
				int n = 0;
				for (n = 0; n <= this.Count - 1; n++) {
					if (this[n].MemberName == Name) {
						this[n].ControlField = value;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public Field FirstField {
			//BinderField'ın ControlField'ını döner. Çünkü asıl işimiz onunla.
			get {
				if (this[0] != null) {
					return this[0].ControlField;
				} else {
					return null;
				}
			}
		}
		public Field LastField {
			get {
				if (this.Count > 0) {
					return this[this.Count - 1].ControlField;
				} else {
					return null;
				}
			}
		}
		private Field AddField(EntityBinderField EntityBinderField)
		{
			//This is a  generic class and must be called in customized Add functions
			if (!string.IsNullOrEmpty(EntityBinderField.MenuName) && EntityBinderField.GroupName != "BaseGroup") {
				MenuItem MenuItemInstance = this.EntityBinder.Menu.MenuItems(EntityBinderField.MenuName);
				//Check whether menu exists or not
				if (MenuItemInstance == null) {
					MenuItemInstance = this.EntityBinder.Menu.MenuItems.AddMenuItem(EntityBinderField.MenuName);
				}
				//Check whether group exists or not
				Group GroupInstance = MenuItemInstance.Groups(EntityBinderField.GroupName);
				if (GroupInstance == null) {
					GroupInstance = MenuItemInstance.Groups.AddGroup(EntityBinderField.GroupName);
				}
				GroupInstance.Fields.Add(EntityBinderField.ControlField);
			} else {
				this.EntityBinder.BaseGroup.Fields.Add(EntityBinderField.ControlField);
			}
			this.List.Add(EntityBinderField);
			return EntityBinderField.ControlField;
		}
		public LabelField AddLabel(string MemberName)
		{
			EntityBinderField NewEntityBinderField = new EntityBinderField(this.EntityBinder, MemberName, "", "BaseGroup");
			return this.AddField(NewEntityBinderField);
		}
		public LabelField AddLabel(string MemberName, string MenuName, string GroupName = "")
		{
			EntityBinderField NewEntityBinderField = new EntityBinderField(this.EntityBinder, MemberName, MenuName, GroupName);
			return this.AddField(NewEntityBinderField);
		}
		public CheckBoxField AddCheckBox(string MemberName, string MenuName = "", string GroupName = "BaseGroup")
		{
			EntityBinderField Field = new EntityBinderField(this.EntityBinder, MemberName, MenuName, GroupName);
			this.List.Add(Field);
			return Field.ControlField;
		}
		public DateTimePickerField AddDateTimePicker(string MemberName, string MenuName = "", string GroupName = "")
		{
			EntityBinderField NewEntityBinderField = new EntityBinderField(this.EntityBinder, MemberName, MenuName, GroupName);
			return this.AddField(NewEntityBinderField);
		}
		public DataGrid.DataGrid AddGrid(string MemberName, string MenuName = "", string GroupName = "")
		{
			EntityBinderField NewEntityBinderField = new EntityBinderField(this.EntityBinder, MemberName, MenuName, GroupName);
			GridField Field = this.AddField(NewEntityBinderField);
			Field.Control.Binder.UpdateWithAjax = true;
			return Field.Control.Binder;
		}
		//Public Function AddDateTimePicker(ByVal MemberName As String, ByVal Value As DateTime) As DateTimePickerField
		//    Return Me.AddDateTimePicker(MemberName, Me.GetFieldTitle(MemberName), Value)
		//End Function
		//Public Function AddDateTimePicker(ByVal MemberName As String, ByVal Title As String, ByVal Value As DateTime) As DateTimePickerField
		//    Dim Field As New DateTimePickerField(Me, Title, MemberName)
		//    Field.Control.Value = Value
		//    Me.Add(Field)
		//    Return Field
		//End Function
		//Public Function AddTextBox(ByVal MemberName As String) As TextBoxField
		//    Return Me.AddTextBox(MemberName, "")
		//End Function
		//Public Function AddTextBox(ByVal MemberName As String, ByVal Value As String) As TextBoxField
		//    Return Me.AddTextBox(MemberName, Value, -1)
		//End Function
		//Public Function AddTextBox(ByVal Title As String, ByVal MemberName As String, ByVal Value As String) As TextBoxField
		//    Return Me.AddTextBox(Title, MemberName, Value, -1)
		//End Function
		//Public Function AddTextBox(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As TextBoxField
		//    Return Me.AddTextBox(Me.GetFieldTitle(MemberName), MemberName, Value, Width)
		//End Function
		//Public Function AddTextBox(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As TextBoxField
		//    Dim Field As TextBoxField = Me.CreateTextBoxField(Title, MemberName)
		//    Field.Control.Value = Value
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddInfinityNumberBox(ByVal MemberName As String) As NumberBoxField
		//    Return Me.AddNumberBox(MemberName, "0")
		//End Function
		//Public Function AddInfinityNumberBox(ByVal MemberName As String, ByVal Value As String) As NumberBoxField
		//    Return Me.AddNumberBox(MemberName, Value, -1)
		//End Function
		//Public Function AddInfinityNumberBox(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As NumberBoxField
		//    Return Me.AddNumberBox(Me.GetFieldTitle(MemberName), MemberName, Value, Width)
		//End Function
		//Public Function AddInfinityNumberBox(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As NumberBoxField
		//    Dim Field As NumberBoxField = Me.CreateInfinityNumberBoxField(Title, MemberName)
		//    Field.Control.Value = Value
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddNumberBox(ByVal MemberName As String) As NumberBoxField
		//    Return Me.AddNumberBox(MemberName, "0")
		//End Function
		//Public Function AddNumberBox(ByVal MemberName As String, ByVal Value As String) As NumberBoxField
		//    Return Me.AddNumberBox(MemberName, Value, -1)
		//End Function
		//Public Function AddNumberBox(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As NumberBoxField
		//    Return Me.AddNumberBox(Me.GetFieldTitle(MemberName), MemberName, Value, Width)
		//End Function
		//Public Function AddNumberBox(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As NumberBoxField
		//    Dim Field As NumberBoxField = Me.CreateNumberBoxField(Title, MemberName)
		//    Field.Control.Value = Value
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddFilterBoxField(ByVal Title As String, ByVal ID As String, ByVal MemberNameToFilter As String, ByVal DataSource As EntityCollection, ByVal DisplayMember As String) As FilterBoxField
		//    Dim FilterBoxField As FilterBoxField = Me.AddFilterBoxField(Title, ID, MemberNameToFilter, DataSource)
		//    FilterBoxField.Control.DisplayMember = DisplayMember
		//    Return FilterBoxField
		//End Function
		//Public Function AddFilterBoxField(ByVal Title As String, ByVal ID As String, ByVal MemberNameToFilter As String, ByVal DataSource As EntityCollection) As FilterBoxField
		//    Dim FilterBoxField As FilterBoxField = Me.AddFilterBoxField(Title, ID, MemberNameToFilter)
		//    FilterBoxField.Control.DataSource = DataSource
		//    Return FilterBoxField
		//End Function
		//Public Function AddFilterBoxField(ByVal Title As String, ByVal ID As String, ByVal MemberNameToFilter As String) As FilterBoxField
		//    Dim FilterBoxField As FilterBoxField = Me.AddFilterBoxField(Title, ID)
		//    FilterBoxField.Control.MemberNameToFilter = MemberNameToFilter
		//    FilterBoxField.Control.DisplayMember = MemberNameToFilter
		//    Return FilterBoxField
		//End Function
		//Public Function AddFilterBoxField(ByVal Title As String, ByVal ID As String) As FilterBoxField
		//    Return Me.Add(Me.CreateFilterBoxField(Title, ID))
		//End Function
		//Public Function AddColorPicker(ByVal MemberName As String, ByVal Value As String) As ColorPickerField
		//    Return Me.AddColorPicker(MemberName, Value, -1)
		//End Function
		//Public Function AddColorPicker(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As ColorPickerField
		//    Return Me.AddColorPicker(Me.GetFieldTitle(MemberName), MemberName, Value, Width)
		//End Function
		//Public Function AddColorPicker(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As ColorPickerField
		//    Dim Field As ColorPickerField = Me.CreateColorField(Title, MemberName)
		//    Field.Control.Value = Value
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddLink(ByVal MemberName As String) As LinkField
		//    Return Me.AddLink(MemberName, "")
		//End Function
		//Public Function AddLink(ByVal MemberName As String, ByVal Value As String) As LinkField
		//    Return Me.AddLink(MemberName, Value, 80)
		//End Function
		//Public Function AddLink(ByVal MemberName As String, ByVal Value As String, ByVal Url As String) As LinkField
		//    Return Me.AddLink(MemberName, Value, Url, -1)
		//End Function
		//Public Function AddLink(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As LinkField
		//    Return Me.AddLink(MemberName, Value, "", Width)
		//End Function
		//Public Function AddLink(ByVal MemberName As String, ByVal Value As String, ByVal Url As String, ByVal Width As Integer) As LinkField
		//    Return Me.AddLink(Me.GetFieldTitle(MemberName), MemberName, Value, Url, Width)
		//End Function
		//Public Function AddLink(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Url As String, ByVal Width As Integer) As LinkField
		//    Dim Field As LinkField = Me.CreateLinkField(Title, MemberName)
		//    Field.Control.Value = Value
		//    Field.Control.Url = Url
		//    Field.Control.Style.Width = Width
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddSelectBox(ByVal MemberName As String) As SelectBoxField
		//    Return Me.AddSelectBox(MemberName, Nothing)
		//End Function
		//Public Function AddSelectBox(ByVal MemberName As String, ByVal Collection As ICollection) As SelectBoxField
		//    Return Me.AddSelectBox(MemberName, Collection, Nothing, -1)
		//End Function
		//Public Function AddSelectBox(ByVal MemberName As String, ByVal Collection As ICollection, ByVal Value As Object, Optional ByVal ValueMember As String = "") As SelectBoxField
		//    Return Me.AddSelectBox(MemberName, Collection, Value, -1, ValueMember)
		//End Function
		//Public Function AddSelectBox(ByVal MemberName As String, ByVal Collection As ICollection, ByVal Value As Object, ByVal Width As Integer, Optional ByVal ValueMember As String = "") As SelectBoxField
		//    Return Me.AddSelectBox(Me.GetFieldTitle(MemberName), MemberName, Collection, Value, False, Width, ValueMember)
		//End Function
		//Public Function AddSelectBox(ByVal Title As String, ByVal MemberName As String, ByVal Collection As ICollection, ByVal Value As Object, ByVal CreateBlankOption As Boolean, ByVal Width As Integer, Optional ByVal ValueMember As String = "") As SelectBoxField
		//    Dim Field As SelectBoxField = Me.CreateSelectBoxField(Title, MemberName)
		//    Field.Control.CreateBlankOption = CreateBlankOption
		//    Field.Control.ValueMember = ValueMember
		//    Field.Control.DataSource = Collection
		//    Field.Control.SelectedItem = Value
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddRadioButton(ByVal MemberName As String) As RadioButtonField
		//    Return Me.AddRadioButton(MemberName, Nothing)
		//End Function
		//Public Function AddRadioButton(ByVal MemberName As String, ByVal Collection As Ophelia.Application.Base.CollectionBase) As RadioButtonField
		//    Return Me.AddRadioButton(MemberName, Collection, Nothing, -1)
		//End Function
		//Public Function AddRadioButton(ByVal MemberName As String, ByVal Collection As Ophelia.Application.Base.CollectionBase, ByVal Value As Object) As RadioButtonField
		//    Return Me.AddRadioButton(MemberName, Collection, Value, -1)
		//End Function
		//Public Function AddRadioButton(ByVal MemberName As String, ByVal Collection As Ophelia.Application.Base.CollectionBase, ByVal Value As Object, ByVal Width As Integer) As RadioButtonField
		//    Return Me.AddRadioButton(Me.GetFieldTitle(MemberName), MemberName, Collection, Value, "", Width)
		//End Function
		//Public Function AddRadioButton(ByVal Title As String, ByVal MemberName As String, ByVal Collection As Ophelia.Application.Base.CollectionBase, ByVal Value As Object, ByVal BlankOptionMessage As String, ByVal Width As Integer) As RadioButtonField
		//    Dim Field As RadioButtonField = Me.CreateRadioButtonField(Title, MemberName)
		//    Field.Control.Collection = Collection
		//    Field.Control.SelectedItem = Value
		//    Field.Control.BlankOptionMessage = BlankOptionMessage
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddPasswordBox(ByVal MemberName As String) As PasswordField
		//    Return Me.AddPasswordBox(Me.GetFieldTitle(MemberName), MemberName)
		//End Function
		//Public Function AddPasswordBox(ByVal Title As String, ByVal MemberName As String) As PasswordField
		//    Return Me.AddPasswordBox(Title, MemberName, 120)
		//End Function
		//Public Function AddPasswordBox(ByVal Title As String, ByVal MemberName As String, ByVal Width As Integer) As PasswordField
		//    Dim Field As PasswordField = Me.CreatePasswordBoxField(Title, MemberName)
		//    Field.Control.Style.Width = Width
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddFileBox(ByVal MemberName As String) As FileBoxField
		//    Return Me.AddFileBox(MemberName, 200)
		//End Function
		//Public Function AddFileBox(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As FileBoxField
		//    Return Me.AddFileBox(Me.GetFieldTitle(MemberName), MemberName, Width, Value)
		//End Function
		//Public Function AddFileBox(ByVal MemberName As String, ByVal Value As String) As FileBoxField
		//    Return Me.AddFileBox(Me.GetFieldTitle(MemberName), MemberName, -1, Value)
		//End Function
		//Public Function AddFileBox(ByVal MemberName As String, ByVal Width As Integer) As FileBoxField
		//    Return Me.AddFileBox(Me.GetFieldTitle(MemberName), MemberName, Width, "")
		//End Function
		//Public Function AddFileBox(ByVal Title As String, ByVal MemberName As String, ByVal Width As Integer, ByVal Value As String) As FileBoxField
		//    Dim Field As FileBoxField = Me.CreateFileBoxField(Title, MemberName)
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    If Value <> "" Then
		//        Field.Control.Value = Value
		//    End If
		//    Field.Control.ResetToolTip = Me.Form.Dictionary.GetWord("Command.Reset")
		//    Field.Control.ShowFileToolTip = Me.Form.Dictionary.GetWord("Command.ShowFile")
		//    Return Me.Add(Field)
		//End Function
		//'TODO : Ahmet burayı kendine göre tekrar düzenleyecek. AddCheckBox(ByVal MemberName As String, ByVal Value As Boolean) ile çakışıyor.
		//'Public Function AddCheckBox(ByVal MemberName As String, ByVal GroupIndex As Integer) As CheckBoxField
		//'    Dim Field As CheckBoxField = Me.AddCheckBox(MemberName, False)
		//'    Field.GroupIndex = GroupIndex
		//'    Return Field
		//'End Function
		//Public Function AddCheckBox(ByVal MemberName As String) As CheckBoxField
		//    Return Me.AddCheckBox(MemberName, False)
		//End Function
		//Public Function AddCheckBox(ByVal MemberName As String, ByVal Value As Boolean) As CheckBoxField
		//    Return Me.AddCheckBox(Me.GetFieldTitle(MemberName), MemberName, Value, -1)
		//End Function
		//Public Function AddCheckBox(ByVal Title As String, ByVal MemberName As String, ByVal Value As Boolean, ByVal Width As Integer) As CheckBoxField
		//    Dim Field As CheckBoxField = Me.CreateCheckBoxField(Title, MemberName)
		//    Field.Control.Value = Value
		//    Field.Control.Style.Width = Width
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddComboBox(ByVal MemberName As String) As ComboBoxField
		//    Return Me.AddComboBox(MemberName, -1)
		//End Function
		//Public Function AddComboBox(ByVal MemberName As String, ByVal Width As Integer) As ComboBoxField
		//    Return Me.AddComboBox(Me.GetFieldTitle(MemberName), MemberName, Width)
		//End Function
		//Public Function AddComboBox(ByVal Title As String, ByVal MemberName As String, ByVal Width As Integer) As ComboBoxField
		//    Dim Field As ComboBoxField = New ComboBoxField(Me, Title, MemberName)
		//    If Width > 0 Then
		//        Field.Control.Style.Width = Width
		//    End If
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddEmptyField(ByVal MemberName As String) As LabelField
		//    Dim Field As LabelField = Me.CreateLabelField("", MemberName)
		//    'Field.Control.Value = "&nbsp;"
		//    Return Me.Add(Field)
		//End Function
		//Public Function AddHiddenBox(ByVal MemberName As String) As HiddenField
		//    Return Me.AddHiddenBox(MemberName, "1")
		//End Function
		//Public Function AddHiddenBox(ByVal MemberName As String, ByVal Value As String) As HiddenField
		//    Dim HiddenField As HiddenField = Me.CreateHiddenField("", MemberName)
		//    HiddenField.Control.Value = Value
		//    Return Me.Add(HiddenField)
		//End Function
		//Public Function AddLabel(ByVal MemberName As String) As LabelField
		//    Return Me.AddLabel(MemberName, "")
		//End Function
		//Public Function AddLabel(ByVal MemberName As String, ByVal Value As String) As LabelField
		//    Return Me.AddLabel(Me.GetFieldTitle(MemberName), MemberName, Value)
		//End Function
		//Public Function AddLabel(ByVal Title As String, ByVal MemberName As String, ByVal Value As String) As LabelField
		//    Dim LabelField As LabelField = Me.CreateLabelField(Title, MemberName)
		//    LabelField.Control.Value = Value
		//    Return Me.Add(LabelField)
		//End Function
		//Public Function AddTextArea(ByVal MemberName As String) As TextAreaField
		//    Return Me.AddTextArea(Me.GetFieldTitle(MemberName), MemberName, "")
		//End Function
		//Public Function AddTextArea(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer, ByVal Height As Integer) As TextAreaField
		//    Return Me.AddTextArea(Me.GetFieldTitle(MemberName), MemberName, Value, Width, Height)
		//End Function
		//Public Function AddTextArea(ByVal Title As String, ByVal MemberName As String, ByVal Value As String) As TextAreaField
		//    Return Me.AddTextArea(Title, MemberName, Value, 300, 100)
		//End Function
		//Public Function AddTextArea(ByVal MemberName As String, ByVal Value As String) As TextAreaField
		//    Return Me.AddTextArea(Me.GetFieldTitle(MemberName), MemberName, Value, 300, 100)
		//End Function
		//Public Function AddTextArea(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer, ByVal Height As Integer) As TextAreaField
		//    Dim TextAreaField As TextAreaField = Me.CreateTextAreaField(Title, MemberName)
		//    TextAreaField.Control.Style.Width = Width
		//    TextAreaField.Control.Style.Height = Height
		//    TextAreaField.Control.Value = Value
		//    Return Me.Add(TextAreaField)
		//End Function
		//Public Function AddButton(ByVal MemberName As String) As ButtonField
		//    Return Me.AddButton(MemberName, Me.GetFieldTitle(MemberName))
		//End Function
		//Public Function AddButton(ByVal MemberName As String, ByVal Value As String) As ButtonField
		//    Return Me.AddButton(MemberName, Value, 50)
		//End Function
		//Public Function AddButton(ByVal MemberName As String, ByVal Value As String, ByVal Width As Integer) As ButtonField
		//    Return Me.AddButton(Me.GetFieldTitle(MemberName), MemberName, Value, Button.ButtonType.Submit, Width)
		//End Function
		//Public Function AddButton(ByVal Title As String, ByVal MemberName As String, ByVal Value As String, ByVal Type As Button.ButtonType, ByVal Width As Integer) As ButtonField
		//    Dim Field As ButtonField = Me.CreateButtonField("", MemberName)
		//    Field.Control.Type = Type
		//    Field.Control.Value = Value
		//    Field.Control.Style.Width = Width
		//    Return Me.Add(Field)
		//End Function
		//Public Function Add(ByVal Field As Field) As Field
		//    Me.List.Add(Field)
		//    Field.Section = Me.CurrentSection
		//    Return Field
		//End Function
		public EntityBinderFieldCollection(EntityBinder Binder)
		{
			this.EntityBinder = Binder;
		}
	}
}

