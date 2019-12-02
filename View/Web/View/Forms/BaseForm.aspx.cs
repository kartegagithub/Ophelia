using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide;
using Ophelia.Application.Base;
using Ophelia.Web.View.Controls;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using Ophelia.Web.View.UI;

namespace Ophelia.Web.View.Forms
{
	public abstract class BaseForm : System.Web.UI.Page, UI.IPage
	{
		private Client oClient;
		private Layout oLayout;
		private string sPageTitle;
		private bool bRequiresAuthorization = false;
		private bool bAutoRefresh = false;
		private int nRefreshPeriod = 60;
		private ResourceRetriver ResourceRetriver = new ResourceRetriver(this);
		private ServerSide.ScriptManager.ScriptCollection oScriptCollection;
		private UI.Header oHeader;
		private Controls.WebControlCollection oControls;
		private ContentManager oContentManager;
		private CacheManager oCacheManager;
		private QueryString oQueryString;
		private Body oBody;
		private int nInstanceID;
		private bool bIsAjaxRequest = false;
		private AjaxConfiguration oAjaxConfiguration;
		public virtual bool UseHtml5 {
			get { return false; }
		}
		public EmbeddedFileProcessingMethod FileUsageType { get; set; }
		public int InstanceID {
			get { return this.nInstanceID; }
		}
		public CacheManager CacheManager {
			get {
				if (this.oCacheManager == null)
					this.oCacheManager = new CacheManager();
				return this.oCacheManager;
			}
		}
		public ContentManager ContentManager {
			get {
				if (this.oContentManager == null) {
					if (this.CacheManager.GetObject("Ophelia@@ContentManager@@") != null) {
						this.oContentManager = this.CacheManager.GetObject("Ophelia@@ContentManager@@");
					}
					if (this.oContentManager == null) {
						this.oContentManager = this.GetContentManager();
						this.CacheManager.Cache("Ophelia@@ContentManager@@", this.oContentManager, 60);
					}
				}
				return this.oContentManager;
			}
		}
		public bool IsAjaxRequest {
			get { return this.bIsAjaxRequest; }
			set { this.bIsAjaxRequest = value; }
		}
		protected virtual ContentManager GetContentManager()
		{
			if (this.Client == null) {
				this.InitializeApplication();
				if (this.Client.Application != null)
					this.Client.Application.Close();
			}
			return this.Client.ContentManager;
		}
		public new UI.Header Header {
			get {
				if (this.oHeader == null)
					this.oHeader = new UI.Header();
				return this.oHeader;
			}
		}
		public Body Body {
			get {
				if (this.oBody == null)
					this.oBody = new Body(this);
				return this.oBody;
			}
		}
		public new Controls.WebControlCollection Controls {
			get { return this.Body.Controls; }
		}
		public StyleSheet StyleSheet {
			get { return this.Header.StyleSheet; }
		}
		public Controls.ServerSide.ScriptManager.ScriptCollection ScriptManager {
			get { return this.Header.ScriptManager; }
		}
		public HttpContext HttpContext {
			get { return System.Web.HttpContext.Current; }
		}
		public new HttpRequest Request {
			get { return base.Request; }
		}
		public new HttpResponse Response {
			get { return base.Response; }
		}
		public new SessionState.HttpSessionState Session {
			get { return base.Session; }
		}
		public Controls.QueryString QueryString {
			get {
				if (this.oQueryString == null)
					this.oQueryString = new QueryString(this.Request);
				return this.oQueryString;
			}
		}
		public Client Client {
			get { return this.oClient; }
		}
		public AjaxConfiguration AjaxConfiguration {
			get {
				if (this.oAjaxConfiguration == null) {
					this.oAjaxConfiguration = new AjaxConfiguration();
					this.CustomizeAjaxConfiguration(this.oAjaxConfiguration);
				}
				return this.oAjaxConfiguration;
			}
		}

		protected virtual void CustomizeAjaxConfiguration(AjaxConfiguration Configuration)
		{
		}
		public Layout Layout {
			get { return this.oLayout; }
			set { this.oLayout = value; }
		}
		public string PageTitle {
			get { return this.sPageTitle; }
			set { this.sPageTitle = value; }
		}
		public bool AutoRefresh {
			get { return this.bAutoRefresh; }
			set { this.bAutoRefresh = value; }
		}
		public int RefreshPeriod {
			get { return this.nRefreshPeriod; }
			set { this.nRefreshPeriod = value; }
		}
		public bool RequiresAuthorization {
			get { return this.bRequiresAuthorization; }
			set { this.bRequiresAuthorization = value; }
		}
		public Application.Framework.Dictionary Dictionary {
			get { return this.Client.Dictionary; }
		}
		public virtual Section ContentRegion {
			get { return this.Layout; }
		}
		public ServerSide.ScriptManager.ScriptCollection Scripts {
			get {
				if (this.oScriptCollection == null) {
					this.oScriptCollection = new ServerSide.ScriptManager.ScriptCollection();
				}
				return this.oScriptCollection;
			}
		}
		private bool bLogQueries = false;
		protected bool LogQueries {
			get { return this.bLogQueries; }
			set { this.bLogQueries = value; }
		}
		protected virtual bool CheckForSpecialRequest()
		{
			if ((this.Request("DisplayChartImage") != null) && !string.IsNullOrEmpty(this.Request("DisplayChartImage"))) {
				Ophelia.Web.View.Controls.ServerSide.ImageDrawer.DrawPieChart(this);
				return true;
			} else if ((this.Request("DisplayCaptchaImage") != null) && !string.IsNullOrEmpty(this.Request("DisplayCaptchaImage"))) {
				Ophelia.Web.View.Controls.ServerSide.ImageDrawer.DrawCaptchaImage(this);
				return true;
			} else if ((this.Request("ResizeImage") != null) && !string.IsNullOrEmpty(this.Request("ResizeImage"))) {
				Ophelia.Web.View.Controls.ServerSide.ImageDrawer.ResizeImage(this);
				return true;
			} else if ((this.Request("WriteScript") != null) && !string.IsNullOrEmpty(this.Request("WriteScript"))) {
				Ophelia.Web.View.Controls.ServerSide.ScriptManager.ScriptDrawer.WriteScript(this);
				return true;
			} else if (this.Request("AjaxRequest") != null || this.Request("AjaxRequestWithApplication") != null) {
				this.AjaxRequestStarted();
				return true;
			}
			return false;
		}
		private void AjaxRequestStarted()
		{
			try {
				bool HasClient = true;
				string EventName = "";
				if (this.Request("AjaxRequest") != null) {
					HasClient = false;
				}
				QueryString QueryString = Ophelia.Web.View.Controls.ServerSide.ScriptManager.ScriptDrawer.ArrangeQueryStringForAjaxRequest((HasClient ? "AjaxRequestWithApplication" : "AjaxRequest"), Request);
				EventName = QueryString.Item("EventName");
				Content Content = new Content();
				if (HasClient) {
					this.oClient = this.CreateClient();
					if (string.IsNullOrEmpty(this.Client.ApplicationPath))
						this.Client.ApplicationPath = this.Server.MapPath(".");
					if ((this.Client.Session != null)) {
						this.Client.Session.Authorized += SessionAuthorized;
					}
					this.InitializeVariables();
					if ((this.Client.Application != null)) {
						this.Client.Application.DataSource.QueryWatcher.LogQueries = this.LogQueries;
					}
				}
				if (this.OnBeforeAjaxRequestedStarted(ref EventName, ref Content, QueryString)) {
					this.OnAjaxRequestStarted(EventName, ref Content, QueryString);
				}
				Response.Write(Content.Value);
			} catch (Exception ex) {
				Response.Write("");
			}
		}
		protected virtual bool OnBeforeAjaxRequestedStarted(ref string EventName, ref Content Content, QueryString QueryString)
		{
			return true;
		}

		protected virtual void OnAjaxRequestStarted(string EventName, ref Content Content, QueryString QueryString)
		{
		}
		private void Page_Load(System.Object sender, System.EventArgs e)
		{
			try {
				if (this.CheckForSpecialRequest()) {
					return;
				}
				this.oClient = this.CreateClient();
				if (string.IsNullOrEmpty(this.Client.ApplicationPath))
					this.Client.ApplicationPath = this.Server.MapPath(".");
				this.Scripts.IgnoreSecurity = this.Client.IgnoreSecurity;
				this.Scripts.ApplicationBase = this.Client.ApplicationBase;
				this.Layout = this.CreateLayout();
				if (Application["Initialized"] == 0) {
					Application["Initialized"] = 1;
					if ((this.Client.ApplicationMonitor != null)) {
						this.Client.ApplicationMonitor.Log.CreateEntry("Application_Start", "Application started with process ID " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString());
					}
					this.InitializeApplication();
				}
				if ((this.Client.Session != null)) {
					this.Client.Session.Authorized += SessionAuthorized;
				}
				this.InitializeVariables();
				if ((this.Client.Application != null)) {
					this.Client.Application.DataSource.QueryWatcher.LogQueries = this.LogQueries;
				}
				this.OnBeforeLoad();
				Response.Write(this.Layout.Draw());
				this.OnAfterLoad();
				if ((this.Client.Application != null)) {
					this.Client.Application.Close();
					this.Client.Application.DataSource.QueryWatcher.LogQueries = false;
				}
			} catch (System.Exception ex) {
				if (ex.GetType().Name == "ThreadAbortException") {
					throw ex;
				} else {
					try {
						this.Client.ExceptionReporter.Data.Add("Url", Request.Url.ToString);
						if ((this.Client.Session != null) && this.Client.Session.IsAuthorized) {
							this.Client.ExceptionReporter.Data.Add("User", this.Client.Session.User.Name + "(" + this.Client.Session.User.ID + ")");
						}
						try {
							int n = 0;
							for (n = 0; n <= Request.Form.AllKeys.GetLength(0) - 1; n++) {
								this.Client.ExceptionReporter.Data.Add(Request.Form.AllKeys(n), Request.Form.GetValues(n)(0));
							}
							this.Client.ExceptionReporter.Data.Add("UserAgent", this.Request.UserAgent);
							this.Client.ExceptionReporter.Data.Add("UserIP", this.Request.UserHostAddress);

						} catch (System.Exception ex3) {
						}
						this.OnBeforeExceptionReported();
						this.Client.ExceptionReporter.Report(ex);
						this.Layout = null;
						this.ShowErrorPage(ex);
					} catch (System.Exception ex2) {
						if ((this.Client != null)) {
							if ((this.Client.Application != null))
								this.Client.Application.Close();
							if ((this.Client.ExceptionReporter != null)) {
								this.OnBeforeExceptionReported();
								this.Client.ExceptionReporter.Report(ex2);
								this.Layout = null;
								this.ShowErrorPage(ex);
							} else {
								throw ex;
							}
						} else {
							this.Redirect("/Error.htm");
						}
					}
				}
			} finally {
				Ophelia.Web.View.UI.PageContext.SetCurrentPage(null);
			}
		}
		public virtual void ShowErrorPage(Exception Ex)
		{
			if (this.Client.ApplicationBase == "/") {
				this.Redirect("/Error.htm");
			} else {
				this.Redirect(this.Client.ApplicationBase + "Error.htm");
			}
		}
		public string ScriptName {
			get { return this.Request.ServerVariables("script_name"); }
		}
		public string Referer {
			get { return this.Request.ServerVariables("http_referer"); }
		}
		public virtual string GetResourceURL(string ImageName, string NameSpace = "Ophelia")
		{
			return ResourceRetriver.GetResourceURL(ImageName, NameSpace);
		}
		public virtual void SetSessionIDCookie(int SessionID)
		{
			if (this.Response.Cookies.Item("SessionID") == null)
				this.Response.Cookies.Add(new System.Web.HttpCookie("SessionID", ""));
			this.Response.Cookies("SessionID").Value = (SessionID * 397);
			if (SessionID == 0 && (this.Client != null)) {
				this.Client.Session.User.ID = 0;
			}
		}
		public virtual int GetSessionIDCookie()
		{
			if (this.Request.Cookies("SessionID") == null) {
				this.SetSessionIDCookie(0);
				return 0;
			} else {
				try {
					return Convert.ToInt64(this.Request.Cookies("SessionID").Value) / 397;
				} catch (System.Exception ex) {
					return 0;
				}
			}
		}

		public virtual void OnBeforeLayoutDraw()
		{
		}

		public virtual void OnAfterLayoutDraw()
		{
		}

		protected virtual void InitializeApplication()
		{
		}
		protected abstract Ophelia.Web.View.Client CreateClient();
		protected virtual Layout CreateLayout()
		{
			return new Layout(this);
		}

		protected virtual void OnBeforeExceptionReported()
		{
		}
		protected virtual void SessionAuthorized(Ophelia.Application.Framework.Session Session)
		{
			this.Client.Session = Session;
			this.SetSessionIDCookie(Session.ID);
		}

		protected virtual void InitializeVariables()
		{
		}

		protected virtual void OnBeforeLoad()
		{
		}

		protected virtual void OnAfterLoad()
		{
		}

		protected virtual void OnBeforeRedirect()
		{
		}
		public void Redirect(string Url)
		{
			this.OnBeforeRedirect();
			if ((this.Client != null)) {
				this.Client.Application.DataSource.QueryWatcher.LogQueries = false;
				this.Client.Application.Close();
			}
			Response.Redirect(Url);
		}
		public BaseForm()
		{
			Load += Page_Load;
			this.nInstanceID = VBMath.Rnd() * 1000000;
			Ophelia.Web.View.UI.PageContext.SetCurrentPage(this);
		}
	}
}
