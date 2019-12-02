using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View
{
	public class Style
	{
		#region "Variables"
		private Content oContent;
		protected Visibility eVisibility = View.Web.Visibility.None;
		protected string sBackgroundColor = "";
		protected string sBackgroundImage = "";
		protected bool bRepeatBackroundImage = true;
		protected int nPadding = int.MinValue;
		protected int nPaddingTop = int.MinValue;
		protected int nPaddingLeft = int.MinValue;
		protected int nPaddingRight = int.MinValue;
		protected int nPaddingBottom = int.MinValue;
			//Css Left için geçerlidir.
		protected int nPositionLeft = int.MinValue;
			//Css Right için geçerlidir.
		protected int nPositionRight = int.MinValue;
			//Css Top için geçerlidir.
		protected int nPositionTop = int.MinValue;
			//Css Bottom için geçerlidir.
		protected int nPositionBottom = int.MinValue;
		protected int nTop = int.MinValue;
		protected int nMarginBottom = int.MinValue;
		protected int nMargin = int.MinValue;
		protected int nMarginInAuto = int.MinValue;
		protected int nLeft = int.MinValue;
		protected int nWidth = int.MinValue;
		protected int nMinWidth = int.MinValue;
		protected int nHeight = int.MinValue;
		protected int nMinHeight = int.MinValue;
		protected int nMaxHeight = int.MinValue;
		protected int nMaxWidth = int.MinValue;
		protected decimal dMaxHeightInPercent = decimal.MinValue;
		protected decimal dMaxWidthInPercent = decimal.MinValue;
		protected int nZIndex = 1;
		protected decimal nOpacity = -1;
		protected double nLineHeight = 0;
		protected bool bVisible = true;
		protected bool bCollapse = false;
		protected VerticalAlignment eVerticalAlignment = VerticalAlignment.None;
		protected HorizontalAlignment eHorizontalAlignment = HorizontalAlignment.None;
		protected BordersConfiguration oBorders = new BordersConfiguration();
		protected FontConfiguration oFont = new FontConfiguration();
		protected DockStyle eDock = DockStyle.None;
		protected string sClass = "";
		protected RepeatBackroungImage eRepeatBackroungImage = RepeatBackroungImage.None;
		protected DisplayMethod eDisplayMethod = DisplayMethod.None;
		protected Cursor eCursor = DisplayMethod.None;
		protected Position ePosition = Position.None;
		protected ListStyle eListStyle = ListStyle.None;
		protected ClearStyle eClear = ClearStyle.None;
		protected FloatType eFloat = FloatType.None;
		protected OverflowType eOverflow = OverflowType.None;
		protected OverflowType eOverflowX = OverflowType.None;
		protected OverflowType eOverflowY = OverflowType.None;
		protected WordwrapType eWordwrap = WordwrapType.None;
		internal bool bCustomized = false;
		protected int nBackgroundImageLeft = int.MinValue;
		protected int nBackgroundImageTop = int.MinValue;
		private int nBottom = Int32.MinValue;
		private decimal nMinWidthInPercent = -1;
		private decimal nMinHeightInPercent = -1;
		private decimal nWidthInPercent = -1;
		private decimal nHeightInPercent = -1;
		private decimal nRight = int.MinValue;
		private WhiteSpace eWhiteSpace = -1;
			#endregion
		protected string sFilter = "";
		public bool IsCustomized {
			get { return this.bCustomized | this.Font.Customized | this.Borders.Customized; }
		}
		public string Class {
			get { return this.sClass; }
			set {
				this.sClass = value;
				this.bCustomized = true;
			}
		}
		public BordersConfiguration Borders {
			get { return this.oBorders; }
		}
		public FontConfiguration Font {
			get { return this.oFont; }
		}
		#region "BackgroundElements"
		public string BackgroundColor {
			get { return this.sBackgroundColor; }
			set {
				this.sBackgroundColor = value;
				this.bCustomized = true;
			}
		}
		public string BackgroundImage {
			get { return this.sBackgroundImage; }
			set {
				this.sBackgroundImage = value;
				this.bCustomized = true;
			}
		}
		public string BackgroundImageLeft {
			get { return this.nBackgroundImageLeft; }
			set {
				this.nBackgroundImageLeft = value;
				this.bCustomized = true;
			}
		}
		public string BackgroundImageTop {
			get { return this.nBackgroundImageTop; }
			set {
				this.nBackgroundImageTop = value;
				this.bCustomized = true;
			}
		}
		public bool RepeatBackroundImage {
			get { return this.bRepeatBackroundImage; }
			set {
				this.bRepeatBackroundImage = value;
				if (bRepeatBackroundImage == false) {
					this.CurrentRepeatBackroungImage = RepeatBackroungImage.None;
				}
				this.bCustomized = true;
			}
		}
		public RepeatBackroungImage CurrentRepeatBackroungImage {
			get { return this.eRepeatBackroungImage; }
			set {
				this.eRepeatBackroungImage = value;
				this.bCustomized = true;
			}
		}
		#endregion
		#region "PositionElements"
		public int PaddingTop {
			get { return this.nPaddingTop; }
			set {
				this.nPaddingTop = value;
				this.bCustomized = true;
			}
		}
		public int PaddingLeft {
			get { return this.nPaddingLeft; }
			set {
				this.nPaddingLeft = value;
				this.bCustomized = true;
			}
		}
		public int PaddingRight {
			get { return this.nPaddingRight; }
			set {
				this.nPaddingRight = value;
				this.bCustomized = true;
			}
		}
		public int PaddingBottom {
			get { return this.nPaddingBottom; }
			set {
				this.nPaddingBottom = value;
				this.bCustomized = true;
			}
		}
		public int Padding {
			get { return this.nPadding; }
			set {
				this.nPadding = value;
				this.bCustomized = true;
			}
		}
		public int MarginInAuto {
			get { return this.nMarginInAuto; }
			set {
				this.nMarginInAuto = value;
				this.bCustomized = true;
			}
		}
		public int Margin {
			get { return this.nMargin; }
			set {
				this.nMargin = value;
				this.bCustomized = true;
			}
		}
		public int Left {
			get { return this.nLeft; }
			set {
				this.nLeft = value;
				this.bCustomized = true;
			}
		}
		public int Top {
			get { return this.nTop; }
			set {
				this.nTop = value;
				this.bCustomized = true;
			}
		}
		public int MarginBottom {
			get { return this.nMarginBottom; }
			set {
				this.nMarginBottom = value;
				this.bCustomized = true;
			}
		}
		public int Bottom {
			get { return this.nBottom; }
			set {
				this.nBottom = value;
				this.bCustomized = true;
			}
		}
		public int Right {
			get { return this.nRight; }
			set {
				this.nRight = value;
				this.bCustomized = true;
			}
		}
		public int PositionLeft {
			get { return this.nPositionLeft; }
			set {
				this.nPositionLeft = value;
				this.bCustomized = true;
			}
		}
		public int PositionRight {
			get { return this.nPositionRight; }
			set {
				this.nPositionRight = value;
				this.bCustomized = true;
			}
		}
		public int PositionTop {
			get { return this.nPositionTop; }
			set {
				this.nPositionTop = value;
				this.bCustomized = true;
			}
		}
		public int PositionBottom {
			get { return this.nPositionBottom; }
			set {
				this.nPositionBottom = value;
				this.bCustomized = true;
			}
		}
		#endregion
		#region "SizeElements"
		public DockStyle Dock {
			get { return this.eDock; }
			set {
				this.eDock = value;
				this.bCustomized = true;
			}
		}
		public int Width {
			get { return this.nWidth; }
			set {
				this.nWidth = value;
				this.bCustomized = true;
			}
		}
		public int MinWidth {
			get { return this.nMinWidth; }
			set {
				this.nMinWidth = value;
				this.bCustomized = true;
			}
		}
		public decimal MinWidthInPercent {
			get { return this.nMinWidthInPercent; }
			set {
				this.nMinWidthInPercent = value;
				this.bCustomized = true;
			}
		}
		public int MaxHeight {
			get { return this.nMaxHeight; }
			set {
				this.nMaxHeight = value;
				this.bCustomized = true;
			}
		}
		public int MaxWidth {
			get { return this.nMaxWidth; }
			set {
				this.nMaxWidth = value;
				this.bCustomized = true;
			}
		}
		public decimal MaxHeightInPercent {
			get { return this.dMaxHeightInPercent; }
			set {
				this.dMaxHeightInPercent = value;
				this.bCustomized = true;
			}
		}
		public decimal MaxWidthInPercent {
			get { return this.dMaxWidthInPercent; }
			set {
				this.dMaxWidthInPercent = value;
				this.bCustomized = true;
			}
		}
		public decimal MinHeightInPercent {
			get { return this.nMinHeightInPercent; }
			set {
				this.nMinHeightInPercent = value;
				this.bCustomized = true;
			}
		}
		public decimal WidthInPercent {
			get { return this.nWidthInPercent; }
			set {
				this.nWidthInPercent = value;
				this.bCustomized = true;
			}
		}
		public decimal HeightInPercent {
			get { return this.nHeightInPercent; }
			set {
				this.nHeightInPercent = value;
				this.bCustomized = true;
			}
		}
		public int Height {
			get { return this.nHeight; }
			set {
				this.nHeight = value;
				this.bCustomized = true;
			}
		}
		public int MinHeight {
			get { return this.nMinHeight; }
			set {
				this.nMinHeight = value;
				this.bCustomized = true;
			}
		}
		#endregion
		public FloatType Float {
			get { return this.eFloat; }
			set {
				this.eFloat = value;
				this.bCustomized = true;
			}
		}
		public OverflowType OverflowX {
			get { return this.eOverflowX; }
			set {
				this.eOverflowX = value;
				this.bCustomized = true;
			}
		}
		public OverflowType OverflowY {
			get { return this.eOverflowY; }
			set {
				this.eOverflowY = value;
				this.bCustomized = true;
			}
		}
		public OverflowType Overflow {
			get { return this.eOverflow; }
			set {
				this.eOverflow = value;
				this.bCustomized = true;
			}
		}
		public ClearStyle Clear {
			get { return this.eClear; }
			set {
				this.eClear = value;
				this.bCustomized = true;
			}
		}
		public VerticalAlignment VerticalAlignment {
			get { return this.eVerticalAlignment; }
			set {
				this.eVerticalAlignment = value;
				this.bCustomized = true;
			}
		}
		public HorizontalAlignment HorizontalAlignment {
			get { return this.eHorizontalAlignment; }
			set {
				this.eHorizontalAlignment = value;
				this.bCustomized = true;
			}
		}
		public Visibility Visibility {
			get { return this.eVisibility; }
			set {
				this.eVisibility = value;
				this.bCustomized = true;
			}
		}
		public double LineHeight {
			get { return this.nLineHeight; }
			set {
				this.nLineHeight = value;
				this.bCustomized = true;
			}
		}
		public WordwrapType Wordwrap {
			get { return this.eWordwrap; }
			set {
				this.eWordwrap = value;
				this.bCustomized = true;
			}
		}
		public int ZIndex {
			get { return this.nZIndex; }
			set {
				this.nZIndex = value;
				this.bCustomized = true;
			}
		}
		public decimal Opacity {
			get { return this.nOpacity; }
			set {
				this.nOpacity = value;
				this.bCustomized = true;
			}
		}
		public string Filter {
			get { return this.sFilter; }
			set {
				this.sFilter = value;
				this.bCustomized = true;
			}
		}
		public DisplayMethod Display {
			get { return this.eDisplayMethod; }
			set {
				this.eDisplayMethod = value;
				this.bCustomized = true;
			}
		}
		public Cursor CursorStyle {
			get { return this.eCursor; }
			set {
				this.eCursor = value;
				this.bCustomized = true;
			}
		}
		public WhiteSpace WhiteSpace {
			get { return this.eWhiteSpace; }
			set {
				this.eWhiteSpace = value;
				this.bCustomized = true;
			}
		}
		public Position PositionStyle {
			get { return this.ePosition; }
			set {
				this.ePosition = value;
				this.bCustomized = true;
			}
		}
		public ListStyle ListStyle {
			get { return this.eListStyle; }
			set {
				this.eListStyle = value;
				this.bCustomized = true;
			}
		}
		public Content Content {
			get {
				if (this.oContent == null)
					this.oContent = new Content();
				return this.oContent;
			}
		}
		public virtual string Draw(bool DrawClass = false, string RestrictPropertyList = "", string HiddenPropertyList = "")
		{
			string ReturnString = "";
			if (this.IsCustomized) {
				if (!DrawClass) {
					if (!string.IsNullOrEmpty(this.sClass)) {
						ReturnString += " class=\"" + this.sClass + "\"";
					}
				}
				ReturnString += this.GetStyle(DrawClass, RestrictPropertyList, HiddenPropertyList);
				ReturnString += "";
			}
			return ReturnString;
		}
		protected string GetSubStyle(bool DrawClass = false, string RestrictedPropertyList = "", string HiddenPropertyList = "")
		{
			string ReturnString = "";
			bool ShowVisibilty = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("visibility")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("visibility"));
			bool ShowPositionStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("position")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("position"));
			bool ShowAligmentStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("alignment")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("alignment"));
			bool ShowBackgroundColorStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("color")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("color"));
			bool ShowBackgroundImageStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("image")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("image"));
			bool ShowBorderStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("border")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("border"));
			bool ShowFontStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("font")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("font"));
			bool ShowPaddingStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("padding")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("padding"));
			bool ShowDisplayStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("display")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("display"));
			bool ShowCursorStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("cursor")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("cursor"));
			bool ShowListTypeStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("clear")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("clear"));
			bool ShowClearStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("listtype")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("listtype"));
			bool ShowZIndexStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("zindex")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("zindex"));
			bool ShowOpacityStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("opacity")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("opacity"));
			bool ShowFloatStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("float")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("float"));
			bool ShowOverflowStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("overflow")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("overflow"));
			bool ShowWordwrapStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("word-wrap")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("word-wrap"));
			bool ShowLineHeightStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("line-height")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("line-height"));
			bool ShowFilterStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("filter")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("filter"));
			bool ShowWhiteSpaceStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("white-space")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("white-space"));
			bool ShowWidth = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("width")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("width"));
			bool ShowHeight = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("heigth")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("heigth"));

			if (ShowVisibilty)
				ReturnString += this.GetVisibilityStyle();
			if (ShowPositionStyle)
				ReturnString += this.GetPositioningStyle();
			if (ShowWidth)
				ReturnString += this.GetWidthStyle();
			if (ShowHeight)
				ReturnString += this.GetHeightStyle();

			if (ShowAligmentStyle)
				ReturnString += this.GetAlignmentStyle();
			if (ShowBackgroundColorStyle)
				ReturnString += this.GetBackgroundColorStyle();
			if (ShowBackgroundImageStyle)
				ReturnString += this.GetBackgroundImageStyle();
			if (ShowBorderStyle)
				ReturnString += this.Borders.GetStyle;
			if (ShowFontStyle)
				ReturnString += this.Font.GetStyle;
			if (ShowPaddingStyle)
				ReturnString += this.GetPaddingStyle();
			if (ShowDisplayStyle)
				ReturnString += this.GetDisplayStyle();
			if (ShowCursorStyle)
				ReturnString += this.GetCursorStyle();
			if (ShowPositionStyle)
				ReturnString += this.GetPositionStyle();
			if (ShowListTypeStyle)
				ReturnString += this.GetListStyle();
			if (ShowClearStyle)
				ReturnString += this.GetClearStyle();
			if (ShowZIndexStyle)
				ReturnString += this.GetZIndexStyle();
			if (ShowLineHeightStyle)
				ReturnString += this.GetLineHeightStyle();
			if (ShowOpacityStyle)
				ReturnString += this.GetOpacityStyle();
			if (ShowFloatStyle)
				ReturnString += this.GetFloatStyle();
			if (ShowOverflowStyle)
				ReturnString += this.GetOverflowStyle();
			if (ShowWordwrapStyle)
				ReturnString += this.GetWordwrapStyle();
			if (ShowFilterStyle)
				ReturnString += this.GetFilterStyle();
			if (ShowWhiteSpaceStyle)
				ReturnString += this.GetWhiteSpaceStyle();


			return ReturnString;
		}
		protected string GetStyle(bool DrawClass = false, string RestrictedPropertyList = "", string HiddenPropertyList = "")
		{
			string ReturnString = "";
			bool ShowVisibility = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("visibility")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("visibility"));
			bool ShowPositionStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("position")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("position"));
			bool ShowAligmentStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("alignment")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("alignment"));
			bool ShowBackgroundColorStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("color")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("color"));
			bool ShowBackgroundImageStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("image")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("image"));
			bool ShowBorderStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("border")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("border"));
			bool ShowFontStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("font")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("font"));
			bool ShowPaddingStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("padding")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("padding"));
			bool ShowDisplayStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("display")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("display"));
			bool ShowCursorStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("cursor")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("cursor"));
			bool ShowClearStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("clear")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("clear"));
			bool ShowListTypeStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("listtype")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("listtype"));
			bool ShowZIndexStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("zindex")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("zindex"));
			bool ShowOpacityStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("opacity")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("opacity"));
			bool ShowFloatStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("float")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("float"));
			bool ShowOverflowStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("overflow")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("overflow"));
			bool ShowWordwrapStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("word-wrap")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("word-wrap"));
			bool ShowLineHeightStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("line-height")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("line-height"));
			bool ShowFilterStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("filter")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("filter"));
			bool ShowWhiteSpaceStyle = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("white-space")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("white-space"));
			bool ShowWidth = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("width")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("width"));
			bool ShowHeight = (string.IsNullOrEmpty(RestrictedPropertyList) || RestrictedPropertyList.ToLower().Contains("heigth")) && (string.IsNullOrEmpty(HiddenPropertyList) || !HiddenPropertyList.ToLower().Contains("heigth"));

			if (!DrawClass) {
				ReturnString = " style=\"";
			}
			if (ShowVisibility)
				ReturnString += this.GetVisibilityStyle();
			if (ShowPositionStyle)
				ReturnString += this.GetPositioningStyle();
			if (ShowWidth)
				ReturnString += this.GetWidthStyle();
			if (ShowHeight)
				ReturnString += this.GetHeightStyle();
			if (ShowAligmentStyle)
				ReturnString += this.GetAlignmentStyle();
			if (ShowBackgroundColorStyle)
				ReturnString += this.GetBackgroundColorStyle();
			if (ShowBackgroundImageStyle)
				ReturnString += this.GetBackgroundImageStyle();
			if (ShowBorderStyle)
				ReturnString += this.Borders.GetStyle;
			if (ShowFontStyle)
				ReturnString += this.Font.GetStyle;
			if (ShowPaddingStyle)
				ReturnString += this.GetPaddingStyle();
			if (ShowDisplayStyle)
				ReturnString += this.GetDisplayStyle();
			if (ShowCursorStyle)
				ReturnString += this.GetCursorStyle();
			if (ShowPositionStyle)
				ReturnString += this.GetPositionStyle();
			if (ShowClearStyle)
				ReturnString += this.GetClearStyle();
			if (ShowListTypeStyle)
				ReturnString += this.GetListStyle();
			if (ShowZIndexStyle)
				ReturnString += this.GetZIndexStyle();
			if (ShowOpacityStyle)
				ReturnString += this.GetOpacityStyle();
			if (ShowFloatStyle)
				ReturnString += this.GetFloatStyle();
			if (ShowOverflowStyle)
				ReturnString += this.GetOverflowStyle();
			if (ShowWordwrapStyle)
				ReturnString += this.GetWordwrapStyle();
			if (ShowLineHeightStyle)
				ReturnString += this.GetLineHeightStyle();
			if (ShowFilterStyle)
				ReturnString += this.GetFilterStyle();
			if (ShowWhiteSpaceStyle)
				ReturnString += this.GetWhiteSpaceStyle();


			if (ReturnString == " style=\"") {
				ReturnString = "";
			} else {
				if (!DrawClass) {
					ReturnString += "\"";
				}
			}
			return ReturnString;
		}
		protected string GetFilterStyle()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(Filter)) {
				ReturnString += "filter:" + this.Filter + ";";
			}
			return ReturnString;
		}
		protected string GetWhiteSpaceStyle()
		{
			string ReturnString = "";
			if (this.WhiteSpace > -1) {
				ReturnString += "white-space:" + this.WhiteSpace.ToString() + ";";
			}
			return ReturnString;
		}
		protected string GetZIndexStyle()
		{
			string ReturnString = "";
			if (ZIndex > 1) {
				ReturnString += "z-index:" + this.ZIndex + ";";
			}
			return ReturnString;
		}
		protected string GetLineHeightStyle()
		{
			string ReturnString = "";
			if (this.LineHeight > 0) {
				ReturnString += "line-height:" + this.LineHeight.ToString().Replace(",", ".") + ";";
			}
			return ReturnString;
		}
		protected string GetOpacityStyle()
		{
			string ReturnString = "";
			if (this.Opacity > -1) {
				ReturnString += "opacity:" + this.Opacity + ";";
				ReturnString = Strings.Replace(ReturnString, ",", ".");
			}
			return ReturnString;
		}
		protected string GetVisibilityStyle()
		{
			string ReturnString = "";
			if (this.Visibility == View.Web.Visibility.Hidden) {
				ReturnString += "visibility:hidden;";
			} else if (this.Visibility == View.Web.Visibility.Collapse) {
				ReturnString += "visibility:collapse;";
			} else if (this.Visibility == View.Web.Visibility.Visible) {
				ReturnString += "visibility:visible;";
			}
			return ReturnString;
		}
		protected string GetPaddingStyle()
		{
			string ReturnString = "";
			if (this.Padding >= 0) {
				ReturnString += "padding:" + this.Padding + "px;";
			}
			if (this.PaddingTop >= 0) {
				ReturnString += "padding-top:" + this.PaddingTop + "px;";
			}
			if (this.PaddingLeft >= 0) {
				ReturnString += "padding-left:" + this.PaddingLeft + "px;";
			}
			if (this.PaddingRight >= 0) {
				ReturnString += "padding-right:" + this.PaddingRight + "px;";
			}
			if (this.PaddingBottom >= 0) {
				ReturnString += "padding-bottom:" + this.PaddingBottom + "px;";
			}
			return ReturnString;
		}
		protected string GetBackgroundImageStyle()
		{
			string ReturnString = "";
			if (!string.IsNullOrEmpty(this.BackgroundImage)) {
				ReturnString += "background-image:url('" + this.BackgroundImage + "');";
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
			if (BackgroundImageLeft > int.MinValue && BackgroundImageTop > int.MinValue) {
				ReturnString += "background-position:" + BackgroundImageLeft + "px " + BackgroundImageTop + "px;";
			}
			return ReturnString;
		}
		protected string GetPositionStyle()
		{
			string ReturnString = "";
			if (this.PositionStyle != Position.None) {
				ReturnString = "position:";
				switch (PositionStyle) {
					case Position.Relative:
						ReturnString += "relative";
						break;
					case Position.Absolute:
						ReturnString += "absolute";
						break;
					case Position.Static:
						ReturnString += "static";
						break;
					case Position.Fixed:
						ReturnString += "fixed";
						break;
					case Position.Inherited:
						ReturnString += "inherit";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetDisplayStyle()
		{
			string ReturnString = "";
			if (this.Display != DisplayMethod.None) {
				ReturnString = "display:";
				switch (Display) {
					case DisplayMethod.Block:
						ReturnString += "block";
						break;
					case DisplayMethod.Hidden:
						ReturnString += "none";
						break;
					case DisplayMethod.Inline:
						ReturnString += "inline";
						break;
					case DisplayMethod.InlineBlock:
						ReturnString += "inline-block";
						break;
					case DisplayMethod.Table:
						ReturnString += "table";
						break;
					case DisplayMethod.TableRowGroup:
						ReturnString += "table-row-group";
						break;
					case DisplayMethod.TableCell:
						ReturnString += "table-cell";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetBackgroundColorStyle()
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
		protected string GetCursorStyle()
		{
			string ReturnString = "";
			if (this.CursorStyle != Cursor.None) {
				ReturnString = "cursor:";
				switch (this.CursorStyle) {
					case Cursor.Pointer:
						ReturnString += "pointer";
						break;
					case Cursor.Move:
						ReturnString += "move";
						break;
					case Cursor.Progress:
						ReturnString += "progress";
						break;
					case Cursor.Default:
						ReturnString += "default";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetWidthStyle()
		{
			string ReturnString = "";
			if (this.Width >= 0) {
				ReturnString += "width:" + this.Width + "px;";
			} else if (this.WidthInPercent > 0) {
				ReturnString += "width:" + this.WidthInPercent + "%;";
			}
			return ReturnString;
		}
		protected string GetHeightStyle()
		{
			string ReturnString = "";
			if (this.Height >= 0) {
				ReturnString += "height:" + this.Height + "px;";
			} else if (this.HeightInPercent > 0) {
				ReturnString += "height:" + this.HeightInPercent + "%;";
			}
			return ReturnString;
		}
		protected string GetPositioningStyle()
		{
			string ReturnString = "";
			if (this.Margin > int.MinValue) {
				ReturnString += "margin:" + this.Margin + "px;";
			} else if (this.MarginInAuto >= 0) {
				ReturnString += "margin:" + this.MarginInAuto + "px auto;";
			}
			if (this.Top > int.MinValue) {
				ReturnString += "margin-top:" + this.Top + "px;";
			}
			if (this.MarginBottom > int.MinValue) {
				ReturnString += "margin-bottom:" + this.MarginBottom + "px;";
			}
			if (this.Left > int.MinValue) {
				ReturnString += "margin-left:" + this.Left + "px;";
			}
			if (this.Right > int.MinValue) {
				ReturnString += "margin-right:" + this.Right + "px;";
			}
			if (this.PositionTop > int.MinValue) {
				ReturnString += "top:" + this.PositionTop + "px;";
			}
			if (this.PositionBottom > int.MinValue) {
				ReturnString += "bottom:" + this.PositionBottom + "px;";
			}
			if (this.PositionLeft > int.MinValue) {
				ReturnString += "left:" + this.PositionLeft + "px;";
			}
			if (this.PositionRight > int.MinValue) {
				ReturnString += "right:" + this.PositionRight + "px;";
			}
			if (this.Dock == DockStyle.Fill) {
				ReturnString += "width:100%;";
				ReturnString += "height:100%;";
			} else if (this.Dock == DockStyle.Width) {
				ReturnString += "width:100%;";
			} else if (this.Dock == DockStyle.Height) {
				ReturnString += "height:100%;";
			}
			if (this.MinWidth > 0) {
				ReturnString += "min-width:" + this.MinWidth + "px;";
			} else if (this.MinWidthInPercent > 0) {
				ReturnString += "min-width:" + this.MinWidthInPercent + "%;";
			}
			if (this.MinHeight > 0) {
				ReturnString += "min-height:" + this.MinHeight + "px;";
			} else if (this.MinHeightInPercent > 0) {
				ReturnString += "min-height:" + this.MinHeightInPercent + "%;";
			}
			if (this.MaxHeight > 0) {
				ReturnString += "max-height:" + this.MaxHeight + "px;";
			} else if (this.MaxHeightInPercent > 0) {
				ReturnString += "max-height:" + this.MaxHeightInPercent + "%;";
			}
			if (this.MaxWidth > 0) {
				ReturnString += "max-width:" + this.MaxWidth + "px;";
			} else if (this.MaxWidthInPercent > 0) {
				ReturnString += "max-width:" + this.MaxWidthInPercent + "%;";
			}
			if (this.Bottom > Int32.MinValue) {
				ReturnString += "bottom:" + this.Bottom + "px;";
			}
			return ReturnString;
		}
		protected string GetListStyle()
		{
			string ReturnString = "";
			if (this.ListStyle != View.Web.ListStyle.None) {
				ReturnString = "list-style:";
				switch (this.ListStyle) {
					case View.Web.ListStyle.Hidden:
						ReturnString += "none";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetClearStyle()
		{
			string ReturnString = "";
			if (this.Clear != View.Web.ClearStyle.None) {
				ReturnString = "clear:";
				switch (this.Clear) {
					case View.Web.ClearStyle.Left:
						ReturnString += "left";
						break;
					case View.Web.ClearStyle.Right:
						ReturnString += "right";
						break;
					case View.Web.ClearStyle.Both:
						ReturnString += "both";
						break;
					case View.Web.ClearStyle.Inherit:
						ReturnString += "inherit";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetFloatStyle()
		{
			string ReturnString = "";
			if (this.Float != View.Web.FloatType.None) {
				ReturnString = "float:";
				switch (this.Float) {
					case FloatType.Left:
						ReturnString += "left";
						break;
					case FloatType.Right:
						ReturnString += "right";
						break;
					case FloatType.Nofloat:
						ReturnString += "none";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetWordwrapStyle()
		{
			string ReturnString = "";
			if (this.Wordwrap != WordwrapType.None) {
				ReturnString = "word-wrap:";
				switch (this.Wordwrap) {
					case WordwrapType.BreakWord:
						ReturnString += "break-word";
						break;
				}
				ReturnString += ";";
			}
			return ReturnString;
		}
		protected string GetOverflowStyle()
		{
			string ReturnString = "";
			if (this.Overflow != OverflowType.None) {
				ReturnString = "overflow:";
				switch (this.Overflow) {
					case OverflowType.Hidden:
						ReturnString += "hidden";
						break;
					case OverflowType.Auto:
						ReturnString += "auto";
						break;
					case OverflowType.Scroll:
						ReturnString += "scroll";
						break;
					case OverflowType.Visible:
						ReturnString += "visible";
						break;
				}
				ReturnString += ";";
			} else {
				if (this.OverflowX != View.Web.OverflowType.None) {
					ReturnString = "overflow-x:";
					switch (this.OverflowX) {
						case OverflowType.Hidden:
							ReturnString += "hidden";
							break;
						case OverflowType.Auto:
							ReturnString += "auto";
							break;
						case OverflowType.Scroll:
							ReturnString += "scroll";
							break;
						case OverflowType.Visible:
							ReturnString += "visible";
							break;
					}
					ReturnString += ";";
				}
				if (this.OverflowY != View.Web.OverflowType.None) {
					ReturnString += "overflow-y:";
					switch (this.OverflowY) {
						case OverflowType.Hidden:
							ReturnString += "hidden";
							break;
						case OverflowType.Auto:
							ReturnString += "auto";
							break;
						case OverflowType.Scroll:
							ReturnString += "scroll";
							break;
						case OverflowType.Visible:
							ReturnString += "visible";
							break;
					}
					ReturnString += ";";
				}
			}
			return ReturnString;
		}
		public Style Clone()
		{
			Style Style = new Style();

			Style.Class = this.Class;

			Style.Borders.Style = this.Borders.Style;
			Style.Borders.Width = this.Borders.Width;
			Style.Borders.Color = this.Borders.Color;

			Style.Borders.Bottom.Width = this.Borders.Bottom.Width;
			Style.Borders.Bottom.Color = this.Borders.Bottom.Color;
			Style.Borders.Bottom.Style = this.Borders.Bottom.Style;

			Style.Borders.Top.Width = this.Borders.Top.Width;
			Style.Borders.Top.Color = this.Borders.Top.Color;
			Style.Borders.Top.Style = this.Borders.Top.Style;

			Style.Borders.Right.Width = this.Borders.Right.Width;
			Style.Borders.Right.Color = this.Borders.Right.Color;
			Style.Borders.Right.Style = this.Borders.Right.Style;

			Style.Borders.Left.Width = this.Borders.Left.Width;
			Style.Borders.Left.Color = this.Borders.Left.Color;
			Style.Borders.Left.Style = this.Borders.Left.Style;

			Style.Font.Color = this.Font.Color;
			Style.Font.Weight = this.Font.Weight;
			Style.Font.Style = this.Font.Style;
			Style.Font.Size = this.Font.Size;
			Style.Font.Unit = this.Font.Unit;
			Style.Font.IsLink = this.Font.IsLink;
			Style.Font.Family = this.Font.Family;
			Style.Font.Decoration = this.Font.Decoration;
			Style.Font.Color = this.Font.Color;

			Style.BackgroundColor = this.BackgroundColor;
			Style.BackgroundImage = this.BackgroundImage;
			Style.BackgroundImageLeft = this.BackgroundImageLeft;
			Style.BackgroundImageTop = this.BackgroundImageTop;
			Style.CurrentRepeatBackroungImage = this.CurrentRepeatBackroungImage;
			Style.RepeatBackroundImage = this.RepeatBackroundImage;

			Style.Float = this.Float;
			Style.OverflowX = this.OverflowX;
			Style.OverflowY = this.OverflowY;
			Style.Dock = this.Dock;
			Style.Clear = this.Clear;
			Style.VerticalAlignment = this.VerticalAlignment;
			Style.HorizontalAlignment = this.HorizontalAlignment;
			Style.Visibility = this.Visibility;


			Style.Padding = this.Padding;
			Style.SetPadding(this.PaddingLeft, this.PaddingTop, this.PaddingRight, this.PaddingBottom);
			Style.LineHeight = this.LineHeight;
			Style.MarginInAuto = this.MarginInAuto;
			Style.Margin = this.Margin;
			Style.MarginBottom = this.MarginBottom;

			Style.PositionLeft = this.PositionLeft;
			Style.PositionBottom = this.PositionBottom;
			Style.PositionRight = this.PositionRight;
			Style.PositionTop = this.PositionTop;
			Style.PositionStyle = this.PositionStyle;

			Style.Left = this.Left;
			Style.Top = this.Top;
			Style.Bottom = this.Bottom;
			Style.Right = this.Right;

			Style.Width = this.Width;
			Style.WidthInPercent = this.WidthInPercent;
			Style.MinWidth = this.MinWidth;
			Style.MinWidthInPercent = this.MinWidthInPercent;
			Style.MaxWidth = this.MaxWidth;
			Style.MaxWidthInPercent = this.MaxWidthInPercent;

			Style.Height = this.Height;
			Style.HeightInPercent = this.HeightInPercent;
			Style.MaxHeight = this.MaxHeight;
			Style.MaxHeightInPercent = this.MaxHeightInPercent;
			Style.MinHeight = this.MinHeight;
			Style.MinHeightInPercent = this.MinHeightInPercent;

			Style.Wordwrap = this.Wordwrap;
			Style.ZIndex = this.ZIndex;
			Style.Opacity = this.Opacity;
			Style.Filter = this.Filter;
			Style.Display = this.Display;
			Style.CursorStyle = this.CursorStyle;
			Style.WhiteSpace = this.WhiteSpace;
			Style.ListStyle = this.ListStyle;

			return Style;
		}
		public virtual void AddClass(string Class)
		{
			if (string.IsNullOrEmpty(this.Class)) {
				this.Class = Class;
			} else {
				this.Class += " " + Class;
			}
		}
		public void SetFont(double Size, string Color = "", FontWeight Weight = -1, string Family = "", TextDecoration Decoration = -1, FontStyle Style = -1, FontUnits FontUnit = FontUnits.Point)
		{
			if (Size > 0)
				this.Font.Size = Size;
			if (!string.IsNullOrEmpty(Color))
				this.Font.Color = Color;
			if (Style > -1)
				this.Font.Style = Style;
			if (Weight > -1)
				this.Font.Weight = Weight;
			if (!string.IsNullOrEmpty(Family))
				this.Font.Family = Family;
			if (Decoration > -1)
				this.Font.Decoration = Decoration;
			this.Font.Unit = FontUnit;
		}
		public void SetPadding(int Left, int Top = -1, int Right = -1, int Bottom = -1)
		{
			if (Left > -1)
				this.PaddingLeft = Left;
			if (Top > -1)
				this.PaddingTop = Top;
			if (Right > -1)
				this.PaddingRight = Right;
			if (Bottom > -1)
				this.PaddingBottom = Bottom;
		}
		public void SetSize(decimal Width, decimal Height)
		{
			this.Width = Width;
			this.Height = Height;
		}
		public void SetBackgroundImageResource(string ResourceName, string Namespace)
		{
			this.BackgroundImage = string.Format("{0}EFiles/{1}/{2}", View.Web.UI.FileHandler.ApplicationBase, Namespace, ResourceName);
		}
		public void SetBackgroundImageResource(int ImageID)
		{
			this.BackgroundImage = string.Format("{0}ExternalFiles/{1}.png", View.Web.UI.FileHandler.ApplicationBase, ImageID);
		}
		public void SetBackground(string Url, bool RepeatBackroundImage)
		{
			this.BackgroundImage = Url;
			this.RepeatBackroundImage = RepeatBackroundImage;
		}
		public void SetBackground(string Url, int PositionLeft, int PostionTop)
		{
			this.SetBackground(Url, false);
			this.BackgroundImageLeft = PositionLeft;
			this.BackgroundImageTop = PostionTop;
		}
		public void SetBackground(int PositionLeft, int PostionTop)
		{
			this.BackgroundImageLeft = PositionLeft;
			this.BackgroundImageTop = PostionTop;
		}
	}
	public enum LayoutTechnique
	{
		Tables = 1,
		Css = 2,
		Custom = 3
	}
	public enum DisplayMethod
	{
		None = 0,
		Block = 1,
		Hidden = 2,
		Inline = 3,
		Table = 4,
		TableRowGroup = 5,
		InlineBlock = 6,
		TableCell = 7
	}
	public enum LayoutDirection
	{
		Vertical = 1,
		Horizantal = 2
	}
	public enum DockStyle
	{
		None = 0,
		Fill = 1,
		Width = 2,
		Height = 3
	}
	public enum SectionAlignment
	{
		None = 0,
		Left = 1,
		Right = 2
	}
	public enum ClearStyle
	{
		None = 0,
		Left = 1,
		Right = 2,
		Both = 3,
		Inherit = 4
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
	public enum Cursor
	{
		None = 0,
		Pointer = 1,
		Progress = 2,
		Move = 3,
		Default = 4
	}
	public enum WhiteSpace
	{
		Normal = 0,
		Nowrap = 1,
		Pre = 2,
		Preline = 3,
		Prewrap = 4
	}
	public enum Visibility
	{
		None = 0,
		Visible = 1,
		Hidden = 2,
		Collapse = 3
	}
	public enum Position
	{
		None = 0,
		Relative = 1,
		Absolute = 2,
		Static = 3,
		Fixed = 4,
		Inherited = 5
	}
	public enum ListStyle
	{
		None = 0,
		Hidden = 1
	}
	public enum FloatType
	{
		None = 0,
		Right = 1,
		Left = 2,
		Nofloat = 3
	}
	public enum OverflowType
	{
		None = 0,
		Hidden = 1,
		Visible = 2,
		Scroll = 3,
		Auto = 4
	}
	public enum WordwrapType
	{
		None = 0,
		BreakWord = 1
	}
}
