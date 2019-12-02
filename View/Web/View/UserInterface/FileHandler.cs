using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Reflection;
namespace Ophelia.Web.View.UI
{
	public class FileHandler : IHttpHandler
	{
		private string sComparableCahcingDateString = string.Empty;
		private static string ValidationCachinLastDateModifiedDateInString = "Fri, 31 Dec 2010 22:00:00 GMT";
		private static System.DateTime CachingLastDateModifiedDate = new System.DateTime(2011, 1, 1);
		private static System.DateTime ResponseAbsoluteCache = new System.DateTime(2099, 12, 1);
		private static Hashtable oAssemblyTable;
		private static bool AssembliesIsLoaded = false;
		private static string sApplicationBase = string.Empty;
		public static string ApplicationBase()
		{
			if (string.IsNullOrEmpty(sApplicationBase)) {
				sApplicationBase = System.Configuration.ConfigurationManager.AppSettings("ApplicationBase");
				if (string.IsNullOrEmpty(sApplicationBase))
					sApplicationBase = "/";
				if (!sApplicationBase.EndsWith("/"))
					sApplicationBase += "/";
			}
			return sApplicationBase;
		}
		public static string GetEmbeddedFileUrl(string AssemblyName, string FileName, EmbeddedFileProcessingMethod Type)
		{
			if (Type == EmbeddedFileProcessingMethod.PageProcessing) {
				return string.Format("{0}?DisplayFile=filename,,{1}$$$Namespace,,{2}", ApplicationBase(), FileName, AssemblyName);
			} else {
				return GetEmbeddedFileUrl(AssemblyName, FileName);
			}
		}
		public static string GetEmbeddedFileUrl(string AssemblyName, string FileName)
		{
			return string.Format("{0}EFiles/{1}/{2}", ApplicationBase(), AssemblyName, FileName);
		}
		public static string GetExternalFileUrl(string FileDirectory)
		{
			return string.Format("{0}ExternalFiles/{1}", ApplicationBase(), FileDirectory);
		}
		public static string GetStaticFileUrl(string FileDirectory)
		{
			return string.Format("{0}Files/{1}", ApplicationBase(), FileDirectory);
		}
		public static Hashtable AssemblyTable {
			get {
				if (!AssembliesIsLoaded)
					LoadAssemly();
				return oAssemblyTable;
			}
		}
		public bool IsReusable {
			get { return true; }
		}
		private static void LoadAssemly()
		{
			AssembliesIsLoaded = true;
			oAssemblyTable = new Hashtable();
			System.Collections.ObjectModel.ReadOnlyCollection<Assembly> Assemblies = My.Application.Info.LoadedAssemblies;
			string AssemblyName = string.Empty;
			for (int i = 0; i <= Assemblies.Count - 1; i++) {
				try {
					System.Type[] Types = Assemblies[i].GetTypes();
					AssemblyName = Types[0].Namespace.Replace(".My", "");
					if (!AssemblyName.StartsWith("System.") && !AssemblyName.StartsWith("Microsoft.")) {
						oAssemblyTable.Add(AssemblyName, Assemblies[i]);
					}

				} catch (Exception ex) {
				}
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			HttpRequest Request = context.Request;
			HttpResponse Response = context.Response;
			string IfModifiedSince = string.Empty;
			string ExecutionFilePath = string.Empty;
			string[] RawUrlData = null;
			string FileName = string.Empty;
			int BufferLength = 0;
			byte[] buffer = null;
			try {
				IfModifiedSince = Request.Headers["If-Modified-Since"];
				if ((!string.IsNullOrEmpty(IfModifiedSince) && string.Equals(IfModifiedSince, ValidationCachinLastDateModifiedDateInString, StringComparison.CurrentCulture))) {
					Response.StatusCode = 304;
					Response.Status = "304 Not Modified";
					Response.StatusDescription = "304 Not Modified";
					Response.AddHeader("Content-Length", "0");
					CacheRequestOnServerAndClient(Response, false);
					return;
				}
				IfModifiedSince = string.Empty;
				ExecutionFilePath = Request.CurrentExecutionFilePath;
				if (ExecutionFilePath.IndexOf(ApplicationBase()) > -1) {
					ExecutionFilePath = ExecutionFilePath.Substring(ApplicationBase().Length);
				} else {
					ExecutionFilePath = ExecutionFilePath.Substring(1);
				}
				RawUrlData = ExecutionFilePath.Split('/');
				SetMimeType(Response, System.IO.Path.GetExtension(ExecutionFilePath));
				// 
				switch (RawUrlData[0]) {
					case "EFiles":
					case "efiles":
						FileName = RawUrlData.Last;
						string AssemblyNameInHash = RawUrlData[1];
						string AssemblyName = AssemblyNameInHash;
						using (System.IO.Stream Stream = AssemblyTable[AssemblyNameInHash].GetManifestResourceStream(AssemblyName + "." + FileName)) {
							BufferLength = Stream.Length;
							if (BufferLength == 0)
								Throw404Exception(Response, "Buffer length 0, AssemblyNameInHash: " + AssemblyNameInHash);
							buffer = new byte[BufferLength];
							Stream.Read(buffer, 0, BufferLength);
							CacheRequestOnServerAndClient(Response, true);
							Response.BinaryWrite(buffer);
						}

						AssemblyName = string.Empty;
						break;
					case "ExternalFiles":
					case "externalfiles":
						FileName = ExecutionFilePath.Substring(13);
						buffer = this.GetExternalStream(FileName);
						BufferLength = buffer.Length;
						if (BufferLength == 0)
							Throw404Exception(Response);
						CacheRequestOnServerAndClient(Response, false);
						Response.BinaryWrite(buffer);
						break;
					case "Files":
					case "files":
						FileName = ExecutionFilePath.Substring(6);
						using (System.IO.FileStream FileStream = new System.IO.FileStream(AppDomain.CurrentDomain.BaseDirectory + FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
							BufferLength = FileStream.Length;
							if (BufferLength == 0)
								Throw404Exception(Response);
							buffer = new byte[BufferLength];
							FileStream.Read(buffer, 0, BufferLength);
							CacheRequestOnServerAndClient(Response, false);
							Response.BinaryWrite(buffer);
						}

						break;
				}
			} catch (Threading.ThreadAbortException ex) {
				throw ex;
			} catch (Exception ex) {
				Throw404Exception(Response, ex.Message + " " + ex.StackTrace);
			} finally {
				//Reset arguments
				IfModifiedSince = string.Empty;
				ExecutionFilePath = string.Empty;
				FileName = string.Empty;
				if (BufferLength > 0)
					Array.Clear(buffer, 0, BufferLength);
				if (RawUrlData != null)
					Array.Clear(RawUrlData, 0, RawUrlData.Length);
			}
		}
		protected void ReturnValue(HttpResponse Response, byte[] buffer)
		{
			if (Response.ContentType == "text/javascript" || Response.ContentType == "text/css") {
				System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(buffer);
				System.IO.StreamReader oStreamReader = new System.IO.StreamReader(oMemoryStream);
				Response.Write(oStreamReader.ReadToEnd());
			} else {
				Response.BinaryWrite(buffer);
			}
		}
		protected virtual Array GetExternalStream(string FileName)
		{
			return null;
		}
		protected static void SetMimeType(HttpResponse Response, string Extension)
		{
			Response.ContentType = Functions.GetMimeType(Extension);
			if (Extension.Equals(".xml", StringComparison.InvariantCultureIgnoreCase)) {
				Response.AddHeader("content-disposition", "attachment; filename=" + "file");
			}
		}
		protected static void CacheRequestOnServerAndClient(HttpResponse Response, bool IsPersistent)
		{
			Response.ExpiresAbsolute = ResponseAbsoluteCache;
			HttpCachePolicy cache = Response.Cache;
			cache.SetValidUntilExpires(true);
			cache.VaryByParams.IgnoreParams = false;
			cache.SetOmitVaryStar(true);
			cache.SetLastModified(CachingLastDateModifiedDate);
			cache.SetCacheability(HttpCacheability.Public);
			cache.SetAllowResponseInBrowserHistory(true);
			if (IsPersistent) {
				cache.SetMaxAge(new TimeSpan(24, 0, 0));
			} else {
				cache.SetMaxAge(new TimeSpan(0, 10, 0));
			}
		}
		protected static void Throw404Exception(HttpResponse Response, string AdditionalMessage = "")
		{
			Response.StatusCode = 404;
			Response.StatusDescription = "Image not found " + AdditionalMessage;
			Response.AddHeader("Content-Length", "0");
			Response.End();
			return;
		}
	}
}
