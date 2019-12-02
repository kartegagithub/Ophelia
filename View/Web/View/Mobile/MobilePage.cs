using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Mobile
{
	public abstract class MobilePage : Ophelia.Web.View.UI.Page
	{
		#region "Properties"
		private NavigationBar oNavigationBar;
		private Panel oFooterArea;
		private Panel oPageContent;
		private Panel oMenuContainer;
		private Panel oMenuOverlay;
		protected Panel MenuContainer {
			get { return this.oMenuContainer; }
		}
		protected NavigationBar NavigationBar {
			get { return this.oNavigationBar; }
		}
		protected Panel FooterArea {
			get { return this.oFooterArea; }
		}
		protected Panel PageContent {
			get {
				if (this.oPageContent == null) {
					this.InitilizePageContent();
				}
				return this.oPageContent;
			}
		}
		protected Panel MenuOverlay {
			get {
				if (oMenuOverlay == null) {
					oMenuOverlay = new Panel("MenuOverlay", PanelDrawingType.Div);
				}
				return oMenuOverlay;
			}
		}
		public override bool UseHtml5 {
			get { return true; }
		}
		#endregion
		protected override sealed void OnLoad()
		{
			base.OnLoad();
			this.oNavigationBar = new NavigationBar();
			this.Controls.Add(this.oNavigationBar);
			this.Controls.Add(this.oNavigationBar.BreadCrumbsArea);
			this.InitilizePageContent();
			this.CustomizeNavigationBar(this.oNavigationBar);
			this.LoadPage();
			this.DrawPageMenu();
			if (this.IsAjaxRequest)
				this.ReturnAjaxResult();
			this.DrawFooter();
			this.CustomizeFooter();
			this.Controls.Add(this.MenuOverlay);
			this.ScriptManager.Add("jquery161min", Ophelia.Web.View.UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "jquery161min.js"));
			this.ScriptManager.Add("OpheliaMobile", Ophelia.Web.View.UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "ophelia-mobile.js"));
			this.Header.Links.Add(Ophelia.Web.View.UI.FileHandler.GetEmbeddedFileUrl("Ophelia", "ophelia-mobile.css"), UI.HeadLink.ReleationShipType.StyleSheet);
			this.Header.Links.Add(this.GetFavIcoFileName(), UI.HeadLink.ReleationShipType.FavIcon);
			this.AddFiles();
		}
		protected override void CustomizeAjaxConfiguration(AjaxConfiguration Configuration)
		{
			base.CustomizeAjaxConfiguration(Configuration);
			Configuration.ShowOverlay = false;
		}
		protected abstract string GetFavIcoFileName();
		protected abstract void LoadPage();
		protected virtual void CustomizeNavigationBar(NavigationBar NavigationBar)
		{
		}
		public virtual bool IsAuthorizedRequest()
		{
			return false;
		}
		#region "Drawing"
		private void InitilizePageContent()
		{
			Panel MainContent = this.Controls.AddPanel("maincontent");
			Panel NavigationContent = MainContent.Controls.AddPanel("navpanel");
			this.oPageContent = NavigationContent.Controls.AddPanel("pagecontent", "content");
		}

		protected virtual void CustomizeFooter()
		{
		}
		private void DrawFooter()
		{
			this.oFooterArea = this.Controls.AddPanel("footer");
		}
		private void DrawPageMenu()
		{
			if (this.IsAuthorizedRequest()) {
				this.oMenuContainer = new Panel("menu");
				this.ConfigureMenu();
				this.Controls.Add(MenuContainer);
			}
		}
		protected void AddMenuItem(string Url, string Text)
		{
			this.MenuContainer.Controls.AddLink("", Url, Text + new Panel("", "icon_arrow").Draw);
		}

		protected virtual void ConfigureMenu()
		{
		}
		#endregion
		#region "Processing"
		protected string GetWord(string Word, string Type = "Concept")
		{
			return this.Client.Dictionary.GetWord(Type + "." + Word);
		}
		private void ReturnAjaxResult()
		{
			Content Content = new Content();
			Content ControlsContent = new Content();
			for (int i = 0; i <= PageContent.Controls.Count - 1; i++) {
				ControlsContent.Add(PageContent.Controls(i).Draw);
			}
			Content.Add(this.PageContent.Content.Value);
			Content.Add(ControlsContent.Value);
			Content.Add(this.Header.ScriptManager.Draw());

			string result = Content.Value + "$$navigatebar$$" + this.NavigationBar.Draw + "$$BreadCrumbsAreaBar$$" + this.NavigationBar.BreadCrumbsArea.Draw + "$$MenuContainer$$";
			if (this.MenuContainer != null)
				result = result + this.MenuContainer.Draw;

			this.ReturnValue(result);
		}
		protected abstract void AddFiles();
		public object GetCookie(string Name)
		{
			return this.Request.Cookies(Name);
		}
		public void SetCookie(string Name, string Value, DateTime ExpireDate)
		{
			dynamic NewCookie = this.GetCookie(Name);
			if (NewCookie == null)
				NewCookie = new HttpCookie(Name);
			NewCookie.Expires = ExpireDate;
			NewCookie.item("Value") = Value;
			this.Response.Cookies.Add(NewCookie);
		}
		public bool RemoveCookie(string Name)
		{
			System.Web.HttpCookie Cookie = this.GetCookie(Name);
			if (Cookie != null) {
				System.Web.HttpContext.Current.Request.Cookies.Remove(Name);
				Cookie.Expires = System.DateTime.Now.AddDays(-1);
				System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
			}
			return false;
		}
		#endregion
		protected override void UpdateConfiguration(ref UI.PageConfiguration Configuration)
		{
			base.UpdateConfiguration(Configuration);
			this.Configuration.AjaxRedirectionIsAvailable = true;
		}
		public MobilePage()
		{
			this.FileUsageType = EmbeddedFileProcessingMethod.FileHandlerProcessing;
		}
	}
}
