using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class WebControlCollection : Application.Base.CollectionBase
	{
		private Container oContainer;
		private Ophelia.Web.View.UI.IPage oPage;
		public Ophelia.Web.View.UI.IPage Page {
			get { return this.oPage; }
		}
		public Container Parent {
			get { return this.oContainer; }
		}
		public new WebControl this[int Index] {
			get { return base.Item(Index); }
		}
		public new WebControl this[string ControlID] {
			get {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this[i].ID == ControlID) {
						return this[i];
					}
				}
				return null;
			}
		}
		public WebControl LastControl {
			get {
				if (this.Count > 0)
					return this[this.Count - 1];
				return null;
			}
		}
		public WebControl FindControl(int ID)
		{
			for (int i = 0; i <= this.Count - 1; i++) {
				if (this[i].ID == ID) {
					return this[i];
				}
			}
			return null;
		}
		public Structure.Structure AddStructure(string ID, int RowCount = 1, int ColumnCount = 1)
		{
			Structure.Structure Control = new Structure.Structure(ID, RowCount, ColumnCount);
			return this.Add(Control);
		}
		public Menu.Menu AddMenu(string ID)
		{
			Menu.Menu Menu = new Menu.Menu(ID);
			return this.Add(Menu);
		}
		public Button AddButton(string ID)
		{
			Button Control = new Button(ID);
			return this.Add(Control);
		}
		public Button AddButton(string ID, string Value)
		{
			Button Control = new Button(ID, Value);
			return this.Add(Control);
		}
		public Button AddButton(string ID, string ImageSource, Button.ButtonType Type, string Value = "")
		{
			Button Control = new Button(ID, ImageSource, Type, Value);
			return this.Add(Control);
		}
		public CheckBox AddCheckBox(string ID, bool Value = false, string Header = "", LabelDrawingType TextDrawingType = LabelDrawingType.Span)
		{
			CheckBox Control = new CheckBox(ID);
			Control.Text = Header;
			Control.Value = Value;
			Control.TextControl.DrawingType = TextDrawingType;
			return this.Add(Control);
		}
		public RadioBox AddRadioBox(string ID, string Name, bool Checked = false, string Value = "")
		{
			RadioBox Control = new RadioBox(ID, Name);
			Control.Checked = Checked;
			Control.Value = Value;
			return this.Add(Control);
		}
		public Label AddLabel(string ID)
		{
			Label Control = new Label(ID);
			return this.Add(Control);
		}
		public Label AddLabel(string ID, string Value)
		{
			Label Control = new Label(ID, Value);
			return this.Add(Control);
		}
		public Label AddLabel(string ID, string Value, string Class)
		{
			Label Control = new Label(ID, Value);
			Control.Style.Class = Class;
			return this.Add(Control);
		}
		public HiddenBox AddHiddenBox(string ID, string Value)
		{
			HiddenBox Control = new HiddenBox(ID, Value);
			return this.Add(Control);
		}
		public TextBox AddTextBox(string ID, string Value)
		{
			TextBox Control = new TextBox(ID, Value);
			return this.Add(Control);
		}
		public TextBox AddTextBox(string ID, string Value, bool Disable)
		{
			TextBox Control = new TextBox(ID, Value);
			Control.Disabled = Disable;
			return this.Add(Control);
		}
		public TextBox AddTextBoxText(string ID, string Value, bool IsReadOnly)
		{
			TextBoxText Control = new TextBoxText(ID, Value);
			Control.ReadOnly = IsReadOnly;
			return this.Add(Control);
		}
		public TextArea AddTextArea(string ID, string Value, int RowCount = 0)
		{
			TextArea Control = new TextArea(ID);
			Control.Value = Value;
			Control.RowCount = RowCount;
			return this.Add(Control);
		}
		public ComboBox AddComboBox(string ID)
		{
			ComboBox Control = new ComboBox(ID);
			return this.Add(Control);
		}
		public ComboBox AddComboBox(string ID, ArrayList DataSource)
		{
			ComboBox Control = this.AddComboBox(ID);
			Control.DataSource = DataSource;
			return this.Add(Control);
		}
		public DateTimePicker AddDateTimePicker(string ID)
		{
			DateTimePicker Control = new DateTimePicker(ID);
			return this.Add(Control);
		}
		public DateTimePicker AddDateTimePicker(string ID, System.DateTime MinDate, System.DateTime MaxDate)
		{
			DateTimePicker Control = this.AddDateTimePicker(ID);
			Control.MinDate = MinDate;
			Control.MaxDate = MaxDate;
			return Control;
		}
		public PasswordBox AddPasswordBox(string ID, string Value)
		{
			PasswordBox Control = new PasswordBox(ID, Value);
			return this.Add(Control);
		}
		public NumberBox AddNumberBox(string ID, decimal Value)
		{
			NumberBox Control = new NumberBox(ID, Value);
			return this.Add(Control);
		}
		public NumberBox AddNumberBox(string ID, decimal Value, bool Disable)
		{
			NumberBox Control = new NumberBox(ID, Value);
			Control.Disabled = Disable;
			return this.Add(Control);
		}
		public Link AddLink(string ID, string Url, string Value, bool NewWindow = false)
		{
			Link Control = new Link(ID, Url, Value, NewWindow);
			return this.Add(Control);
		}
		public Link AddLink(string ID)
		{
			Link Control = new Link(ID);
			return this.Add(Control);
		}
		public Image AddImage(string ID, string Source, string AlternateText = "")
		{
			Image Control = new Image(ID, Source, AlternateText);
			return this.Add(Control);
		}
		public PopupControl AddPopup(string ID, bool HideControl, PopupControl.PopupAutoClosingType AutoClosingType = PopupControl.PopupAutoClosingType.Hide)
		{
			PopupControl Control = new PopupControl(ID);
			Control.AutoClosingType = AutoClosingType;
			if (HideControl)
				Control.Hide();
			return this.Add(Control);
		}
		public PopupControl AddPopup(string ID, bool HideControl, string Content, PopupControl.PopupAutoClosingType AutoClosingType = PopupControl.PopupAutoClosingType.Hide)
		{
			PopupControl Control = this.AddPopup(ID, HideControl, AutoClosingType);
			Control.ContentRegion.Content.Add(Content);
			return this.Add(Control);
		}
		public PopupControl AddPopup(string ID, bool HideControl, Container Content, PopupControl.PopupAutoClosingType AutoClosingType = PopupControl.PopupAutoClosingType.Hide)
		{
			PopupControl Control = this.AddPopup(ID, HideControl, AutoClosingType);
			Control.ContentRegion.Controls.Add(Content);
			return this.Add(Control);
		}
		public Heading AddHeading(string MemberName)
		{
			return this.AddHeading(MemberName, string.Empty);
		}
		public Heading AddHeading(string MemberName, string Value)
		{
			return this.AddHeading(MemberName, Value, Heading.HeadingType.H1);
		}
		public Heading AddHeading(string MemberName, string Value, Heading.HeadingType Type)
		{
			Heading Control = new Heading(MemberName, Value, Type);
			return this.Add(Control);
		}
		public object AddFileBox(string MemberName)
		{
			return this.Add(new FileBox(MemberName));
		}
		public Panel AddPanel(string ID)
		{
			return this.AddPanel(ID, PanelDrawingType.Div);
		}
		public Panel AddPanel(string ID, string Class)
		{
			return this.AddPanel(ID, Class, PanelDrawingType.Div);
		}
		public Panel AddPanel(string ID, PanelDrawingType DrawingType)
		{
			return this.Add(new Panel(ID, DrawingType));
		}
		public Panel AddPanel(string ID, string Class, PanelDrawingType DrawingType)
		{
			Panel Panel = new Panel(ID, DrawingType);
			Panel.Style.Class = Class;
			return this.Add(Panel);
		}
		public Literal AddLiteral()
		{
			Literal Control = new Literal();
			return this.Add(Control);
		}
		public SelectBox AddSelectBox(string Name)
		{
			return this.AddSelectBox(Name, string.Empty);
		}
		public SelectBox AddSelectBox(string Name, string Value)
		{
			SelectBox Control = new SelectBox(Name, Value);
			return this.Add(Control);
		}
		private PopupButton CreatePopupButton(string ID, string ImageSource, Button.ButtonType Type, string Content, Container Container, PopupControl.PopupAutoClosingType AutoClosingType)
		{
			PopupButton PopupButton = default(PopupButton);
			if (!string.IsNullOrEmpty(Content)) {
				PopupButton = new PopupButton(ID, ImageSource, Type, Content, AutoClosingType);
			} else if (Container != null) {
				PopupButton = new PopupButton(ID, ImageSource, Type, Container, AutoClosingType);
			} else {
				PopupButton = new PopupButton(ID, ImageSource, Type, "", AutoClosingType);
			}
			return PopupButton;
		}
		public PopupButton AddPopupButton(string ID, string ImageSource, string Content)
		{
			return this.AddPopupButton(ID, ImageSource, Content, PopupControl.PopupAutoClosingType.Hide);
		}
		public PopupButton AddPopupButton(string ID, string ImageSource, string Content, PopupControl.PopupAutoClosingType AutoClosingType)
		{
			return this.Add(this.CreatePopupButton(ID, ImageSource, Button.ButtonType.AjaxButton, Content, null, AutoClosingType));
		}
		public PopupButton AddPopupButton(string ID, string ImageSource, Container Content)
		{
			return this.AddPopupButton(ID, ImageSource, Content, PopupControl.PopupAutoClosingType.Hide);
		}
		public PopupButton AddPopupButton(string ID, string ImageSource, Container Content, PopupControl.PopupAutoClosingType AutoClosingType)
		{
			return this.Add(this.CreatePopupButton(ID, ImageSource, Button.ButtonType.AjaxButton, "", Content, AutoClosingType));
		}
		public WebControl Add(WebControl Control)
		{
			if (Control != null) {
				if (!this.List.Contains(Control)) {
					base.List.Add(Control);
					Control.ParentControl = this.Parent;
					if (this.Parent != null) {
						if (this.Parent.Container != null) {
							Control.Container = this.Parent.Container;
						} else {
							Control.Container = this.Parent;
						}
					}
				}
				return Control;
			}
			return null;
		}
		public bool Remove(WebControl Control)
		{
			if (Control != null && base.List != null && base.List.Contains(Control)) {
				base.List.Remove(Control);
				return true;
			}
			return false;
		}
		public WebControlCollection(Container Container)
		{
			this.oContainer = Container;
		}
		public WebControlCollection(Ophelia.Web.View.UI.IPage Page)
		{
			this.oPage = Page;
		}
	}
}
