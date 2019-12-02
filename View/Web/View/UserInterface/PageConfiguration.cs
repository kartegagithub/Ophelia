using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class PageConfiguration
	{
		private string sApplicationBase = "/";
		private bool bAutoRefresh = false;
		private int nRefreshPeriod = -1;
		private HipertextTransferProtocolSecureManagementType eHTTPSManagementType = HipertextTransferProtocolSecureManagementType.Development;
		private bool bIsSecurePage = false;
		private HtmlDocumentType eDocumentType = HtmlDocumentType.HTML4;
		private string sErrorPageUrl = "Error.htm";
		private ArrayList ContentLibraries = new ArrayList();
		private Hashtable oCahcingQueryStringParameter;
		private Page oPage;
		private string sCacheGroup;
		private PageCachingType ePageCachingType;
		private int dCacheResetTimeInMinute = 1440;
		private CahceParameterCollection oCahcingPageParameters;
		private int nCurrentLibraryCounter = 0;
		private bool bAjaxRedirectionIsAvailable = true;
		private string sContentLanguage = "TR";
		private bool bResponseCaching = true;
		internal string CurrentLibraryUrl {
			get {
				nCurrentLibraryCounter += 1;
				int n = this.nCurrentLibraryCounter % this.ContentLibraries.Count;
				return this.ContentLibraries[n];
			}
		}
		public PageCachingType CachingType {
			get { return this.ePageCachingType; }
			set {
				if (value == PageCachingType.ParameterCache && this.CahcingQueryStringParameter.Count == 0)
					return;
				this.ePageCachingType = value;
			}
		}
		public HipertextTransferProtocolSecureManagementType HTTPSManagementType {
			get { return this.eHTTPSManagementType; }
			set { this.eHTTPSManagementType = value; }
		}
		public bool IsSecurePage {
			get { return this.bIsSecurePage; }
			set { this.bIsSecurePage = value; }
		}
		public string ErrorPageUrl {
			get { return this.sErrorPageUrl; }
			set { this.sErrorPageUrl = value; }
		}
		public bool AjaxRedirectionIsAvailable {
			get { return this.bAjaxRedirectionIsAvailable; }
			set { this.bAjaxRedirectionIsAvailable = value; }
		}
		public HtmlDocumentType DocumentType {
			get { return this.eDocumentType; }
			set { this.eDocumentType = value; }
		}
		internal string GetHtmlDocumentType()
		{
			switch (this.eDocumentType) {
				case HtmlDocumentType.HTML4:
					return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 //EN\">";
				case HtmlDocumentType.HTML5:
					return "<!DOCTYPE HTML> ";
			}
			return "";
		}
		public string ApplicationBase {
			get { return this.sApplicationBase; }
			set { this.sApplicationBase = value; }
		}
		public bool AutoRefresh {
			get { return this.bAutoRefresh; }
			set { this.bAutoRefresh = value; }
		}
		public int RefreshPeriod {
			get { return this.nRefreshPeriod; }
			set { this.nRefreshPeriod = value; }
		}
		public string CacheGroup {
			get { return this.sCacheGroup; }
			set { this.sCacheGroup = value; }
		}
		public string CacheResetTimeInMinute {
			get { return this.dCacheResetTimeInMinute; }
			set { this.dCacheResetTimeInMinute = value; }
		}
		public string ContentLanguage {
			get {
				if (string.IsNullOrEmpty(this.sContentLanguage))
					this.sContentLanguage = "TR";
				return this.sContentLanguage;
			}
			set {
				this.sContentLanguage = value;
				if ((value != this.oPage.Header.ContentLanguage)) {
					this.oPage.Header.ContentLanguage = value;
				}
			}
		}
		public bool ResponseCaching {
			get { return this.bResponseCaching; }
			set { this.bResponseCaching = value; }
		}
		public bool CanBeCached {
			get {
				if (this.CachingType == PageCachingType.ParameterCache && this.CahcingQueryStringParameter.Count > 0) {
					return true;
				}
				if (this.CachingType == PageCachingType.StaticCahce)
					return true;
				if (this.CachingType == PageCachingType.SubUrlCache && this.CahcingPageParameters.Count > 0) {
					for (int i = 0; i <= this.CahcingPageParameters.Count - 1; i++) {
						if (this.CahcingPageParameters[i].CahcingSubPage && this.oPage.Request.RawUrl.ToUpper.Contains(this.CahcingPageParameters[i].Url.ToUpper())) {
							return true;
						} else if (!this.CahcingPageParameters[i].CahcingSubPage && this.oPage.Request.RawUrl.ToUpper == this.ApplicationBase.ToUpper() + this.CahcingPageParameters[i].Url.ToUpper()) {
							return true;
						}
					}
				}
				return false;
			}
		}
		public CahceParameterCollection CahcingPageParameters {
			get {
				if (this.oCahcingPageParameters == null)
					this.oCahcingPageParameters = new CahceParameterCollection();
				return this.oCahcingPageParameters;
			}
		}
		internal Hashtable CahcingQueryStringParameter {
			get {
				if (this.oCahcingQueryStringParameter == null)
					this.oCahcingQueryStringParameter = new Hashtable();
				return this.oCahcingQueryStringParameter;
			}
		}
		public void AddCahcingQueryStringParameter(string Key, string DefaultValue)
		{
			this.CahcingQueryStringParameter[Key] = DefaultValue;
			this.CachingType = PageCachingType.ParameterCache;
		}
		public void RemoveCahcingQueryStringParameter(string Key)
		{
			this.CahcingQueryStringParameter.Remove(Key);
			if (this.CahcingQueryStringParameter.Count == 0)
				this.CachingType = PageCachingType.None;
		}
		public bool AddContentLibrary(string ContentLibraryUrl)
		{
			this.ContentLibraries.Add(ContentLibraryUrl);
		}
		public bool RemoveContentLibrary(string ContentLibraryUrl)
		{
			for (int i = 0; i <= this.ContentLibraries.Count - 1; i++) {
				if (this.ContentLibraries[i].ToString() == ContentLibraryUrl) {
					this.ContentLibraries.Remove(ContentLibraryUrl);
					return true;
				}
			}
			return false;
		}
		public PageConfiguration(Page Page)
		{
			this.oPage = Page;
			this.CacheGroup = Page.ToString();
			this.ContentLibraries.Add("/");
		}
		public enum HipertextTransferProtocolSecureManagementType
		{
			Development = 1,
			IIS = 2,
			LoadBalancer = 3
		}
		public enum HtmlDocumentType
		{
			HTML4 = 1,
			HTML5 = 2,
			None = 99
		}
		public enum PageCachingType
		{
			None = 0,
			StaticCahce = 1,
			SubUrlCache = 2,
			ParameterCache = 3
		}
	}
	public class CahceParameter
	{
		private string sUrl = "";
		private bool bCahcingSubPage = false;
		private int nDuration = -1;
		public string Url {
			get { return this.sUrl; }
			set { this.sUrl = value; }
		}
		public int Duration {
			get { return this.nDuration; }
			set { this.nDuration = value; }
		}
		public bool CahcingSubPage {
			get { return this.bCahcingSubPage; }
			set { this.bCahcingSubPage = value; }
		}
	}
	public class CahceParameterCollection : Ophelia.Application.Base.CollectionBase
	{
		public new CahceParameter this[int Index] {
			get { return base.Item(Index); }
		}
		public CahceParameter Add(string Url, bool CahcingSubPage)
		{
			return this.Add(Url, CahcingSubPage, -1);
		}
		public CahceParameter Add(string Url, bool CahcingSubPage, int Duration)
		{
			CahceParameter CacheParameter = new CahceParameter();
			CacheParameter.Url = Url;
			CacheParameter.CahcingSubPage = CahcingSubPage;
			CacheParameter.Duration = Duration;
			this.List.Add(CacheParameter);
			return CacheParameter;
		}
	}
}
