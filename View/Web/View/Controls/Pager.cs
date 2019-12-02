using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class Pager : WebControl
	{
		private Ophelia.Application.Base.EntityCollection oCollection;
		private int nCurrentPage = -1;
		private int nLinkedPageCount = 10;
		private int nLowerIndex = -1;
		private int nUpperIndex = -1;
		private string sFontColor = "#000000";
		private string sSelectionBackColor = "#FFFFFF";
		private string sSelectionForeColor = "#000000";
		private string sBackgroundImage = "";
		private int nFontSize = 10;
		private int nCellSize = 15;
		private string sFontWeight = "bold";
		private NavigateLinkString eNavigateLink = NavigateLinkString.Arrows;
		private PagePointer eCurrentPagePointer = PagePointer.DrawBorder;
		private string sLeftNavigateLink = "";
		private string sRightNavigateLink = "";
		private bool bSplitNumbers = false;
		private string sPageString = "Page";
		private FontSizeType eFontSizeType = FontSizeType.Pixcel;
		private bool bShowCollectionCount = false;
		private string sCollectionDisplayName = "";
		private bool bShowPageSelectorInString = false;
		private int nCellHeight = -1;
		private ArrayList sIgnoreParameterList = new ArrayList();
		private int nPageSize = -1;
		private string sNextNavigateLink = "sonraki";
		private string sPreviousNavigateLink = "önceki";
		private string sItemClass = "";
		private bool bDisplayPagerGroupLinks = true;
		public ArrayList IgnoreParameterList {
			get { return this.sIgnoreParameterList; }
		}
		public bool CanBeShowCollectionCount {
			get { return bShowCollectionCount; }
		}
		public string CollectionDisplayName {
			get { return this.sCollectionDisplayName; }
		}
		public void ShowCollectionCount(string DisplayName = "")
		{
			this.bShowCollectionCount = true;
			this.sCollectionDisplayName = DisplayName;
		}
		public void HideCollectionCount()
		{
			this.bShowCollectionCount = false;
			this.sCollectionDisplayName = "";
		}
		public bool DisplayPagerGroupLinks {
			get { return this.bDisplayPagerGroupLinks; }
			set { this.bDisplayPagerGroupLinks = value; }
		}
		public int CellHeight {
			get { return nCellHeight; }
			set { nCellHeight = value; }
		}
		public string ItemClass {
			get { return this.sItemClass; }
			set { sItemClass = value; }
		}
		public bool ShowPageSelectorInString {
			get { return bShowPageSelectorInString; }
			set { bShowPageSelectorInString = value; }
		}
		public FontSizeType CurrentFontSizeType {
			get { return this.eFontSizeType; }
			set { this.eFontSizeType = value; }
		}
		public string NextNavigateLink {
			get { return this.sNextNavigateLink; }
			set { this.sNextNavigateLink = value; }
		}
		public string PreviousNavigateLink {
			get { return this.sPreviousNavigateLink; }
			set { this.sPreviousNavigateLink = value; }
		}
		public string LeftNavigateLink {
			get { return this.sLeftNavigateLink; }
			set { this.sLeftNavigateLink = value; }
		}
		public string RightNavigateLink {
			get { return this.sRightNavigateLink; }
			set { this.sRightNavigateLink = value; }
		}
		public string SelectionBackColor {
			get { return this.sSelectionBackColor; }
			set { this.sSelectionBackColor = value; }
		}
		public string SelectionForeColor {
			get { return this.sSelectionForeColor; }
			set { this.sSelectionForeColor = value; }
		}
		public string PageString {
			get { return this.sPageString; }
			set { this.sPageString = value; }
		}
		public bool SplitNumbers {
			get { return this.bSplitNumbers; }
			set { this.bSplitNumbers = value; }
		}
		public NavigateLinkString NavigateLink {
			get { return this.eNavigateLink; }
			set { this.eNavigateLink = value; }
		}
		public PagePointer CurrentPagePointer {
			get { return this.eCurrentPagePointer; }
			set { this.eCurrentPagePointer = value; }
		}
		public int FontSize {
			get { return this.nFontSize; }
			set { this.nFontSize = value; }
		}
		public int CellSize {
			get { return this.nCellSize; }
			set { this.nCellSize = value; }
		}
		public string FontWeight {
			get { return this.sFontWeight; }
			set { this.sFontWeight = value; }
		}
		public string FontColor {
			get { return this.sFontColor; }
			set { this.sFontColor = value; }
		}
		private Ophelia.Application.Base.EntityCollection Collection {
			get { return this.oCollection; }
		}
		public virtual int CurrentPage {
			get {
				if (this.nCurrentPage == -1) {
					if ((this.Request(this.PageString) != null)) {
						this.nCurrentPage = this.Request(this.PageString);
					} else if (!string.IsNullOrEmpty(this.Page.QueryString(this.PageString))) {
						this.nCurrentPage = this.Page.QueryString(this.PageString);
					}
					if (this.nCurrentPage <= 0)
						this.nCurrentPage = 1;
					if (this.PageSize > 0 && this.nCurrentPage > Convert.ToInt32(this.Collection.Count / this.PageSize) + 1)
						this.nCurrentPage = Convert.ToInt32(this.Collection.Count / this.PageSize) + 1;
				}
				return this.nCurrentPage;
			}
		}
		public int LowerIndex {
			get {
				this.CalculateBounds();
				return this.nLowerIndex;
			}
		}
		public int UpperIndex {
			get {
				this.CalculateBounds();
				return this.nUpperIndex;
			}
		}
		private void CalculateBounds()
		{
			if (this.nLowerIndex == -1) {
				this.nLowerIndex = (this.CurrentPage - 1) * this.PageSize;
				this.nUpperIndex = this.nLowerIndex + this.PageSize - 1;
				if (this.nUpperIndex > this.Collection.Count - 1)
					this.nUpperIndex = this.Collection.Count - 1;
			}
		}
		public int LinkedPageCount {
			get { return this.nLinkedPageCount; }
			set { this.nLinkedPageCount = value; }
		}
		public int PageSize {
			get {
				if (this.nPageSize == -1) {
					return this.Collection.Definition.PageSize;
				} else {
					return this.nPageSize;
				}
			}
			set {
				this.nPageSize = value;
				this.Collection.Definition.PageSize = this.PageSize;
			}
		}

		protected virtual void UpdateQueryString(ref QueryString QueryString)
		{
		}
		public override void OnBeforeDraw(Ophelia.Web.View.Content Content)
		{
			string ReturnString = "";
			string StyleString = "";
			QueryString QueryString = new QueryString(Request);
			dynamic PagerItemClass = "";
			if (this.ItemClass != string.Empty)
				PagerItemClass = "class='" + this.ItemClass + "'";

			this.UpdateQueryString(ref QueryString);
			int TotalPages = 0;
			if (this.PageSize > 0) {
				TotalPages = System.Decimal.Floor(this.Collection.Count / this.PageSize);
				if (this.Collection.Count % this.PageSize > 0)
					TotalPages += 1;
			}
			for (int n = 0; n <= this.IgnoreParameterList.Count - 1; n++) {
				QueryString.Remove(this.IgnoreParameterList[n]);
			}
			if (string.IsNullOrEmpty(this.ID))
				this.ID = "pager";
			if (TotalPages > 1) {
				if (this.CurrentFontSizeType == FontSizeType.Pixcel) {
					ReturnString += "<div id=\"" + this.ID + "\" style=\"align:center;font-size:" + this.FontSize + "px;font-weight:" + this.FontWeight + ";\"><table cellspacing=\"0\" cellpadding=\"0\"><TR>";
				} else if (this.CurrentFontSizeType == FontSizeType.Punto) {
					ReturnString += "<div id=\"" + this.ID + "\" style=\"align:center;font-size:" + this.FontSize + "pt;font-weight:" + this.FontWeight + ";\"><table cellspacing=\"0\" cellpadding=\"0\"><TR>";
				}
				this.StyleSheet.AddCustomRule("div#" + this.ID + " td", "text-align:center;");
				int FirstLinkNumber = 0;
				int LinkCountToDraw = 0;

				if (LinkedPageCount % 2 == 0) {
					FirstLinkNumber = CurrentPage - LinkedPageCount / 2;
				} else {
					FirstLinkNumber = CurrentPage - (LinkedPageCount + 1) / 2;
				}
				if (FirstLinkNumber < 0)
					FirstLinkNumber = 0;

				if (TotalPages - FirstLinkNumber < LinkedPageCount) {
					LinkCountToDraw = TotalPages - FirstLinkNumber;
				} else {
					LinkCountToDraw = LinkedPageCount;
				}
				if (FirstLinkNumber > 0) {
					if (CurrentPage - LinkedPageCount > 0) {
						QueryString.Update(this.PageString, CurrentPage - LinkedPageCount);
					} else {
						QueryString.Update(this.PageString, 1);
					}
					if (this.DisplayPagerGroupLinks) {
						ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? "height=\"" + this.CellHeight : "") + " align=\"center\"><a href=\"" + QueryString.Value + "\" " + PagerItemClass + "  style=\"color:" + this.FontColor + ";\">";
						if (this.NavigateLink == NavigateLinkString.Arrows) {
							ReturnString += "<<</a></td>";
						} else if (this.NavigateLink == NavigateLinkString.Points) {
							ReturnString += "...</a></td>";
						} else if (this.NavigateLink == NavigateLinkString.Image) {
							if (!string.IsNullOrEmpty(this.LeftNavigateLink)) {
								Image LeftNavigateImage = new Image("", this.LeftNavigateLink);
								ReturnString += LeftNavigateImage.Draw + "</a></td>";
							} else {
								ReturnString += "...</a></td>";
							}
						} else if (this.NavigateLink == NavigateLinkString.Custom) {
							ReturnString += this.LeftNavigateLink + "</a></td>";
						}
					}
				}
				if (ShowPageSelectorInString) {
					if (CurrentPage > 1) {
						QueryString.Update(this.PageString, CurrentPage - 1);
						ReturnString += "<td width=\"" + this.CellSize * 3 + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight : "") + "\" align=\"center\"><a href=\"" + QueryString.Value + "\" " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">";
						ReturnString += this.PreviousNavigateLink + "</a></td>";
					}
				}
				int n = 0;
				for (n = 1; n <= LinkCountToDraw; n++) {
					if (FirstLinkNumber + n != CurrentPage) {
						ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight + "\"" : "") + " align=\"center\">";
						QueryString.Update(this.PageString, FirstLinkNumber + n);
						ReturnString += "<a href=\"" + QueryString.Value + "\" " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">";
					} else {
						if (this.CurrentPagePointer == PagePointer.DrawBorder) {
							ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight + "\"" : "") + " style=\"border:1px solid " + this.FontColor + "\" align=\"center\">";
						} else if (this.CurrentPagePointer == PagePointer.DrawBold) {
							ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight + "\"" : "") + " style=\"" + this.FontColor + "\" align=\"center\">";
						} else if (this.CurrentPagePointer == PagePointer.ChangeColor) {
							ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight + "\"" : "") + " style=\"background-color:" + this.SelectionBackColor + ";background-repeat:repeat-x;\" align=\"center\">";
						} else if (this.CurrentPagePointer == PagePointer.Custom) {
							ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? " height=\"" + this.CellHeight + "\"" : "") + " align=\"center\">";
						}
					}
					if (this.CurrentPagePointer == PagePointer.DrawBorder) {
						if (FirstLinkNumber + n != CurrentPage) {
							ReturnString += "<font style=\"color:" + this.FontColor + ";\">" + FirstLinkNumber + n + (this.SplitNumbers ? "." : "") + "</font>";
						} else {
							ReturnString += "<font " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">" + FirstLinkNumber + n + (this.SplitNumbers ? "." : "") + "</font>";
						}
					} else if (this.CurrentPagePointer == PagePointer.DrawBold) {
						if (FirstLinkNumber + n != CurrentPage) {
							ReturnString += "<font style=\"color:" + this.FontColor + ";\">" + FirstLinkNumber + n + (this.SplitNumbers ? "." : "") + "</font>";
						} else {
							ReturnString += "<font " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">" + FirstLinkNumber + n + (this.SplitNumbers ? "." : "") + "</font>";
						}
					} else if (this.CurrentPagePointer == PagePointer.ChangeColor) {
						if (FirstLinkNumber + n != CurrentPage) {
							ReturnString += FirstLinkNumber + n;
						} else {
							ReturnString += "<font " + PagerItemClass + " style=\"color:" + this.SelectionForeColor + ";\">" + FirstLinkNumber + n + "</font>";
						}
					} else if (this.CurrentPagePointer == PagePointer.Custom) {
						if (FirstLinkNumber + n != CurrentPage) {
							ReturnString += FirstLinkNumber + n;
						} else {
							if (!string.IsNullOrEmpty(SelectionForeColor))
								StyleString = "color:" + this.SelectionForeColor + ";";
							if (!string.IsNullOrEmpty(SelectionBackColor))
								StyleString += "background:" + this.SelectionBackColor + ";";
							if (!string.IsNullOrEmpty(StyleString))
								StyleString += " style='" + StyleString + "'";

							ReturnString += "<font " + PagerItemClass + "" + StyleString + ">" + FirstLinkNumber + n + "</font>";
						}
					}
					if (FirstLinkNumber + n != CurrentPage) {
						ReturnString += "</a>";
					}
					ReturnString += "</td>";
				}
				if (CurrentPage < TotalPages) {
					if (ShowPageSelectorInString) {
						if (this.PageSize > 0 && CurrentPage < Convert.ToInt32(this.Collection.Count / this.PageSize) + 1) {
							QueryString.Update(this.PageString, this.CurrentPage + 1);
							ReturnString += "<td width=\"" + this.CellSize * 3 + "\"" + (nCellHeight > -1 ? "height=\"" + this.CellHeight + "\"" : "") + " align=\"center\"><a href=\"" + QueryString.Value + "\" " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">";
							ReturnString += this.NextNavigateLink + "</a></td>";
						}
					}
				}
				if (FirstLinkNumber + n <= TotalPages) {
					if (this.PageSize > 0 && CurrentPage + LinkedPageCount > this.Collection.Count / this.PageSize) {
						QueryString.Update(this.PageString, Math.Truncate((this.Collection.Count - 1) / this.PageSize) + 1);
					} else {
						QueryString.Update(this.PageString, CurrentPage + LinkedPageCount);
					}
					if (this.DisplayPagerGroupLinks) {
						ReturnString += "<td width=\"" + this.CellSize + "\"" + (nCellHeight > -1 ? "height=\"" + this.CellHeight + "\"" : "") + " align=\"center\"><a href=\"" + QueryString.Value + "\" " + PagerItemClass + " style=\"color:" + this.FontColor + ";\">";
						if (this.NavigateLink == NavigateLinkString.Arrows) {
							ReturnString += ">></a><td>";
						} else if (this.NavigateLink == NavigateLinkString.Points) {
							ReturnString += "...</a><td>";
						} else if (this.NavigateLink == NavigateLinkString.Image) {
							if (!string.IsNullOrEmpty(this.RightNavigateLink)) {
								Image RightNavigateImage = new Image("", this.RightNavigateLink);
								ReturnString += RightNavigateImage.Draw + "</a></td>";
							} else {
								ReturnString += "...</a></td>";
							}
						} else if (this.NavigateLink == NavigateLinkString.Custom) {
							ReturnString += this.RightNavigateLink + "</a></td>";
						}
					}
				}
				if (this.CanBeShowCollectionCount) {
					ReturnString += "<td width=\"" + this.CellSize * 7 + "\"" + (nCellHeight > -1 ? "height=\"" + this.CellHeight + "\"" : "") + " align=\"right\"><font color=\"" + this.FontColor + "\">";
					ReturnString += "Toplam " + Strings.FormatNumber(Collection.Count, 0) + (!string.IsNullOrEmpty(this.CollectionDisplayName) ? " " + CollectionDisplayName : "") + "</font></td>";
				}
				ReturnString += "</tr></table>";
				ReturnString += "</div>";

			}
			Content.Clear();
			Content.Add(ReturnString);
		}
		public static string HighligtedPages = "";
		public static string Draw(string Url, int ItemCount, int PageSize, int CurrentPage, int LinkedPageCount, HttpRequest Request = null)
		{
			string ReturnString = "";
			QueryString QueryString = new QueryString(Url, Request);
			int TotalPages = System.Decimal.Floor(ItemCount / PageSize);
			if (ItemCount % PageSize > 0)
				TotalPages += 1;
			if (TotalPages > 1) {
				if (CurrentPage <= 0)
					CurrentPage = 1;
				if (CurrentPage > TotalPages)
					CurrentPage = TotalPages;
				ReturnString += "<DIV STYLE=\"align:center;font-size:10pt;font-weight:bold;\"><TABLE><TR>";
				int FirstLinkNumber = 0;
				int LinkCountToDraw = 0;
				if (CurrentPage % LinkedPageCount == 0) {
					FirstLinkNumber = CurrentPage - LinkedPageCount + 1;
				} else {
					FirstLinkNumber = CurrentPage - (CurrentPage % LinkedPageCount);
				}
				if (TotalPages - FirstLinkNumber < LinkedPageCount) {
					LinkCountToDraw = TotalPages - FirstLinkNumber;
				} else {
					LinkCountToDraw = LinkedPageCount;
				}
				if (FirstLinkNumber > 0) {
					if (CurrentPage - LinkedPageCount > 0) {
						QueryString.Update("Page", CurrentPage - LinkedPageCount);
					} else {
						QueryString.Update("Page", 1);
					}
					ReturnString += "<TD WIDTH=\"15\" ALIGN=\"center\"><A HREF=\"" + QueryString.Value + "\">";
					ReturnString += "<<</A></TD>";
				}
				int n = 0;
				for (n = 1; n <= LinkCountToDraw; n++) {
					if (FirstLinkNumber + n != CurrentPage) {
						ReturnString += "<TD WIDTH=\"15\" ALIGN=\"center\">";
						QueryString.Update("Page", FirstLinkNumber + n);
						ReturnString += "<A HREF=\"" + QueryString.Value + "\">";
					} else {
						ReturnString += "<TD WIDTH=\"15\" STYLE=\"border:1px solid #000000\" ALIGN=\"center\">";
					}
					if (HighligtedPages.IndexOf("-" + FirstLinkNumber + n + "-") > -1) {
						ReturnString += "<FONT COLOR=\"#EF5D00\">" + FirstLinkNumber + n + "</FONT>";
					} else {
						ReturnString += FirstLinkNumber + n;
					}
					if (FirstLinkNumber + n != CurrentPage) {
						ReturnString += "</A>";
					}
					ReturnString += "</TD>";
				}
				if (FirstLinkNumber + n < TotalPages) {
					if (CurrentPage + LinkedPageCount > ItemCount / PageSize) {
						QueryString.Update("Page", Convert.ToInt32(ItemCount / PageSize) + 1);
					} else {
						QueryString.Update("Page", CurrentPage + LinkedPageCount);
					}
					ReturnString += "<TD WIDTH=\"15\" ALIGN=\"center\"><A HREF=\"" + QueryString.Value + "\">";
					ReturnString += ">></A><TD>";
				}
				ReturnString += "</TR></TABLE></DIV>";
			}
			HighligtedPages = "";
			return ReturnString;
		}
		public static string Draw(SimpleCollection Collection, int PageSize, string Url, int CurrentPage = 1, int LinkedPageCount = 10)
		{
			return Draw(Url, Collection.Count, PageSize, CurrentPage, LinkedPageCount);
		}
		public static string Draw(EntityCollection Collection, string Url, int CurrentPage, int LinkedPageCount, HttpRequest Request)
		{
			return Draw(Url, Collection.Count, Collection.Definition.PageSize, CurrentPage, LinkedPageCount, Request);
		}
		public static string Draw(EntityCollection Collection, string Url, int CurrentPage = 1, int LinkedPageCount = 10)
		{
			return Draw(Url, Collection.Count, Collection.Definition.PageSize, CurrentPage, LinkedPageCount);
		}

		public Pager()
		{
		}

		public virtual void OnConfigure()
		{
		}
		public Pager(Ophelia.Application.Base.EntityCollection Collection) : this()
		{
			this.oCollection = Collection;
			this.OnConfigure();
		}
		public enum NavigateLinkString
		{
			Arrows = 1,
			Points = 2,
			Image = 3,
			Custom = 4
		}
		public enum PagePointer
		{
			DrawBorder = 1,
			DrawBold = 2,
			ChangeColor = 3,
			Custom = 4
		}
		public enum FontSizeType
		{
			Punto = 1,
			Pixcel = 2
		}
	}
}
