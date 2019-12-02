using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Forms
{
	public class Section
	{
		private SectionCollection oCollection;
		private string sName = "";
		private Content oContent;
		private string sBackgroundColor = "";
		private string sBackgroundImage = "";
		private bool bRepeatBackroundImage = true;
		private int nPadding = 0;
		private int nPaddingTop = 0;
		private int nPaddingLeft = 0;
		private int nPaddingRight = 0;
		private int nPaddingBottom = 0;
		private int nTop = -1;
		private int nLeft = -1;
		private int nWidth = -1;
		private int nHeight = -1;
		private bool bVisible = true;
		private SectionCollection oSections;
		private LayoutDirection eLayoutDirection = LayoutDirection.Vertical;
		private SectionAlignment eAlignment = SectionAlignment.None;
		private VerticalAlignment eVerticalAlignment = VerticalAlignment.None;
		private HorizontalAlignment eHorizontalAlignment = HorizontalAlignment.None;
		private BordersConfiguration oBorders = new BordersConfiguration();
		private FontConfiguration oFont = new FontConfiguration();
		private Title oTitle = new Title(this);
		private DockStyle eDock = DockStyle.Fill;
		private RepeatBackroungImage eRepeatBackroungImage = RepeatBackroungImage.TwoDimension;
		private bool bIsDependent;
		public bool IsDependent {
			get { return this.bIsDependent; }
			set { this.bIsDependent = value; }
		}
		public BordersConfiguration Borders {
			get { return this.oBorders; }
		}
		public FontConfiguration Font {
			get { return this.oFont; }
		}
		public Title Title {
			get { return this.oTitle; }
		}
		public string BackgroundColor {
			get { return this.sBackgroundColor; }
			set { this.sBackgroundColor = value; }
		}
		public string BackgroundImage {
			get { return this.sBackgroundImage; }
			set { this.sBackgroundImage = value; }
		}
		public LayoutDirection LayoutDirection {
			get { return this.eLayoutDirection; }
			set { this.eLayoutDirection = value; }
		}
		public SectionAlignment Alignment {
			get { return this.eAlignment; }
			set { this.eAlignment = value; }
		}
		public bool RepeatBackroundImage {
			get { return this.bRepeatBackroundImage; }
			set {
				this.bRepeatBackroundImage = value;
				if (bRepeatBackroundImage == false) {
					this.CurrentRepeatBackroungImage = RepeatBackroungImage.None;
				}
			}
		}
		public RepeatBackroungImage CurrentRepeatBackroungImage {
			get { return this.eRepeatBackroungImage; }
			set { this.eRepeatBackroungImage = value; }
		}
		public DockStyle Dock {
			get { return this.eDock; }
			set { this.eDock = value; }
		}
		public VerticalAlignment VerticalAlignment {
			get { return this.eVerticalAlignment; }
			set { this.eVerticalAlignment = value; }
		}
		public HorizontalAlignment HorizontalAlignment {
			get { return this.eHorizontalAlignment; }
			set { this.eHorizontalAlignment = value; }
		}
		public bool Visible {
			get { return this.bVisible; }
			set { this.bVisible = value; }
		}
		public SectionCollection Collection {
			get { return this.oCollection; }
			set { this.oCollection = value; }
		}
		public Section Parent {
			get { return this.Collection.Parent; }
		}
		public System.Web.HttpServerUtility Server {
			get { return this.Form.Server; }
		}
		public System.Web.SessionState.HttpSessionState Session {
			get { return this.Form.Session; }
		}
		public System.Web.HttpRequest Request {
			get { return this.Form.Request; }
		}
		public Ophelia.Web.View.Forms.BaseForm Form {
			get { return this.Layout.Form; }
		}
		public Client Client {
			get { return this.Layout.Form.Client; }
		}
		public Ophelia.Application.Base.ApplicationFacade Application {
			get { return this.Client.Application; }
		}
		public ContentManager ContentManager {
			get { return this.Client.ContentManager; }
		}
		public Layout Layout {
			get {
				if ((this.Collection != null)) {
					return this.Collection.Layout;
				} else {
					return this;
				}
			}
		}
		public SectionCollection Sections {
			get { return this.oSections; }
		}
		public string Name {
			get { return this.sName; }
			set { this.sName = value; }
		}
		public int Padding {
			get { return this.nPadding; }
			set { this.nPadding = value; }
		}
		public int PaddingTop {
			get { return this.nPaddingTop; }
			set { this.nPaddingTop = value; }
		}
		public int PaddingLeft {
			get { return this.nPaddingLeft; }
			set { this.nPaddingLeft = value; }
		}
		public int PaddingRight {
			get { return this.nPaddingRight; }
			set { this.nPaddingRight = value; }
		}
		public int PaddingBottom {
			get { return this.nPaddingBottom; }
			set { this.nPaddingBottom = value; }
		}
		public int Left {
			get { return this.nLeft; }
			set { this.nLeft = value; }
		}
		public int Top {
			get { return this.nTop; }
			set { this.nTop = value; }
		}
		public int Bottom {
			get { return this.Top + this.Height; }
		}
		public int Right {
			get { return this.Left + this.Width; }
		}
		public bool IsFirstSection {
			get {
				if ((this.Collection != null)) {
					if (object.ReferenceEquals(this.Collection.FirstSection, this)) {
						return true;
					}
				}
				return false;
			}
		}
		public bool IsLastSection {
			get {
				if ((this.Collection != null)) {
					if (object.ReferenceEquals(this.Collection.LastSection, this)) {
						return true;
					}
				}
				return false;
			}
		}
		public Section FirstSection {
			get { return this.Sections.FirstSection; }
		}
		public Section LastSection {
			get { return this.Sections.LastSection; }
		}
		public int Width {
			get { return this.nWidth; }
			set { this.nWidth = value; }
		}
		public int Height {
			get { return this.nHeight; }
			set { this.nHeight = value; }
		}
		public Content Content {
			get {
				if (this.oContent == null)
					this.oContent = new Content();
				return this.oContent;
			}
		}
		public string ContentFile {
			set { this.Content.Add(this.ContentManager.GetFile(value)); }
		}
		public virtual string Draw()
		{
			this.OnBeforeDraw();
			string ReturnString = "";
			if ((this.Collection != null) && this.Visible) {
				switch (this.Layout.Technique) {
					case LayoutTechnique.Tables:
						ReturnString += "<TD";
						break;
					case LayoutTechnique.Css:
						ReturnString += "<DIV";
						break;
				}
				if (this.Layout.Technique != LayoutTechnique.Custom) {
					if (!string.IsNullOrEmpty(this.Name)) {
						ReturnString += " ID=\"" + this.Name + "\"";
					}
					ReturnString += this.GetStyle();
					ReturnString += ">";
				}
				ReturnString += this.Title.Draw();
				ReturnString += this.GetContent();
				ReturnString += this.Sections.Draw();
				switch (this.Layout.Technique) {
					case LayoutTechnique.Tables:
						ReturnString += "</TD>";
						break;
					case LayoutTechnique.Css:
						ReturnString += "</DIV>";
						break;
				}
				this.OnAfterDraw();
			}
			return ReturnString;
		}
		protected string GetContent()
		{
			if (!string.IsNullOrEmpty(this.Content.Value)) {
				switch (this.Layout.Technique) {
					case LayoutTechnique.Tables:
						return this.Content.Value;
					case LayoutTechnique.Css:
						return "<DIV>" + this.Content.Value + "</DIV>";
					case LayoutTechnique.Custom:
						return this.Content.Value;
				}
			}
			return "";
		}
		protected string GetStyle()
		{
			string ReturnString = " STYLE=\"";
			ReturnString += this.GetPositioningStyle();
			ReturnString += this.GetAlignmentStyle();
			ReturnString += this.GetColorStyle();
			ReturnString += this.GetImageStyle();
			ReturnString += this.Borders.GetStyle;
			ReturnString += this.Font.GetStyle;
			ReturnString += this.GetPaddingStyle();
			if (ReturnString == " STYLE=\"") {
				ReturnString = "";
			} else {
				ReturnString += "\"";
			}
			return ReturnString;
		}
		protected string GetPaddingStyle()
		{
			string ReturnString = "";
			if (this.Padding == 0) {
				ReturnString += "padding-top:" + this.PaddingTop + "px;";
				ReturnString += "padding-left:" + this.PaddingLeft + "px;";
				ReturnString += "padding-right:" + this.PaddingRight + "px;";
				ReturnString += "padding-bottom:" + this.PaddingBottom + "px;";
			} else {
				ReturnString += "padding:" + this.Padding + "px;";
			}
			return ReturnString;
		}
		protected string GetImageStyle()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(this.BackgroundImage) && this.GetType().Name != "Layout") {
				ReturnString += "background-image:url('" + this.Layout.Client.ImageBase + this.BackgroundImage + "');";
				switch (this.CurrentRepeatBackroungImage) {
					case RepeatBackroungImage.OnlyHorizontal:
						ReturnString += "background-repeat:repeat-x;";
						break;
					case RepeatBackroungImage.OnlyVertical:
						ReturnString += "background-repeat:repeat-y;";
						break;
					case RepeatBackroungImage.None:
						ReturnString += "background-repeat:no-repeat;";
						break;
					case RepeatBackroungImage.TwoDimension:
						ReturnString += "background-repeat:repeat;";
						break;
				}
			}
			return ReturnString;
		}
		protected string GetColorStyle()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(this.BackgroundColor)) {
				ReturnString += "background-color:" + this.BackgroundColor + ";";
			}
			return ReturnString;
		}
		protected string GetAlignmentStyle()
		{
			string ReturnString = "";
			if (this.HorizontalAlignment != HorizontalAlignment.None) {
				ReturnString += "text-align:" + this.HorizontalAlignment.ToString().Replace("HorizontalAlignment.", "").ToLower();
				ReturnString += ";";
			}
			if (this.VerticalAlignment != VerticalAlignment.None) {
				ReturnString += "vertical-align:" + this.VerticalAlignment.ToString().Replace("VerticalAlignment.", "").ToLower();
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetPositioningStyle()
		{
			string ReturnString = "";
			if (this.Top != -1) {
				ReturnString += "margin-top:" + this.Top + "px;";
			}
			if (this.Left != -1) {
				ReturnString += "margin-left:" + this.Left + "px;";
			}
			if (this.Width != -1) {
				ReturnString += "width:" + this.Width + "px;";
			} else if (this.Dock == DockStyle.Fill && this.Width == -1) {
				ReturnString += "width:100%;";
			}
			if (this.Height != -1) {
				ReturnString += "height:" + this.Height + "px;";
			} else if (this.Dock == DockStyle.Fill && this.Parent.LayoutDirection == LayoutDirection.Vertical) {
				ReturnString += "height:100%;";
			}
			return ReturnString;
		}

		protected virtual void OnBeforeDraw()
		{
		}

		protected virtual void OnAfterDraw()
		{
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void CreateSections()
		{
		}
		public Section(SectionCollection Collection, bool CreateSections = true)
		{
			this.oSections = new SectionCollection(this);
			if (Collection == null) {
				this.oCollection = new SectionCollection(this);
			} else {
				this.oCollection = Collection;
			}
			if (CreateSections)
				this.CreateSections();
			this.OnInitialize();
		}
	}
	public enum LayoutTechnique
	{
		Tables = 1,
		Css = 2,
		Custom = 3
	}
	public enum LayoutDirection
	{
		Vertical = 1,
		Horizantal = 2
	}
	public enum DockStyle
	{
		None,
		Fill
	}
	public enum SectionAlignment
	{
		None = 0,
		Left = 1,
		Right = 2
	}
	public enum VerticalAlignment
	{
		None = 0,
		Top = 1,
		Bottom = 2,
		Middle = 3
	}
	public enum HorizontalAlignment
	{
		None = 0,
		Left = 1,
		Right = 2,
		Center = 3,
		Justify = 4
	}
	public enum RepeatBackroungImage
	{
		None = 0,
		OnlyHorizontal = 1,
		OnlyVertical = 2,
		TwoDimension = 3
	}
}
