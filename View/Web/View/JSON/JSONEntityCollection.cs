using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Ophelia.Web.View.JSON
{
	public class JSONEntityCollection : Ophelia.Application.Base.CollectionBase
	{
		private string sTitle;
		internal string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public new JSONEntity this[int Index] {
			get { return base.Item(Index); }
		}
		public JSONEntity FirstEntity {
			get {
				if (this.Count == 0)
					return null;
				return this[0];
			}
		}
		public JSON.JSONEntity Add()
		{
			return this.Add(new JSONEntity(this));
		}
		public JSONEntity Add(JSONEntity JSONEntity)
		{
			this.List.Add(JSONEntity);
			return JSONEntity;
		}
		internal void Draw(Content Content)
		{
			for (int i = 0; i <= this.Count - 1; i++) {
				if (i > 0) {
					Content.Add(",");
				}
				this[i].Draw(Content);
			}
		}
		public string Draw()
		{
			Content Content = new Content();
			Content.Add("{\"" + this.Title + "\":[");
			for (int i = 0; i <= this.Count - 1; i++) {
				this[i].Draw(Content);
				if (i != this.Count - 1) {
					Content.Add(",");
				}
			}
			Content.Add("]}");
			return Content.Value;
		}
		//Page de geçilebilsin
		public JSONEntityCollection(string Title, string PropertiesName, object EntityCollection) : this(Title)
		{
			JSONEntity JSONEntity = default(JSONEntity);
			for (int i = 0; i <= EntityCollection.Count - 1; i++) {
				JSONEntity = this.Add();
				JSONEntity.SetData(PropertiesName, EntityCollection(i));
			}
		}
		public JSONEntityCollection(string Title) : this()
		{
			this.Title = Title;
		}

		public JSONEntityCollection()
		{
		}
		public static JSONEntityCollection StringToJSON(string InStr)
		{
			JavaScriptSerializer Serializer = new JavaScriptSerializer();
			System.Collections.Generic.Dictionary<string, object> Result = null;
			try {
				Result = Serializer.DeserializeObject(InStr);
			} catch (Exception ex) {
				JSONEntityCollection ResultObjectsOnError = new JSONEntityCollection();
				ResultObjectsOnError.Add().SetData("Message", InStr);
				return ResultObjectsOnError;
			}
			JSONEntityCollection ResultObjects = GetJSONCollection(Result);
			return ResultObjects;
		}
		public static JSONEntityCollection UrlToJSON(string Url)
		{
			string InStr = string.Empty;
			System.Net.WebClient Client = new System.Net.WebClient();
			Client.Encoding = System.Text.UTF8Encoding.UTF8;
			InStr = Client.DownloadString(Url);
			Client.Dispose();
			return StringToJSON(InStr);
		}
		private static JSONEntityCollection GetJSONCollection(System.Collections.Generic.Dictionary<string, object> Result)
		{
			JSONEntityCollection ResultObjects = new JSONEntityCollection();
			if ((Result != null)) {
				if (Result[Result.Keys(0)].GetType().FullName == "System.Object[]") {
					for (int i = 0; i <= Result[Result.Keys(0)].Length - 1; i++) {
						JSONEntity JSObject = ResultObjects.Add();
						System.Collections.Generic.Dictionary<string, object> Elements = Result[Result.Keys(0)](i);
						for (int k = 0; k <= Elements.Count - 1; k++) {
							JSObject.Data(Elements.Keys(k)) = Elements[Elements.Keys(k)];
						}
					}
				} else {
					System.Collections.Generic.Dictionary<string, object> Elements = Result[Result.Keys(0)];
					JSONEntity JSObject = ResultObjects.Add();
					for (int j = 0; j <= Elements.Count - 1; j++) {
						JSObject.Data(Elements.Keys(j)) = Elements[Elements.Keys(j)];
					}
				}
			}
			return ResultObjects;
		}
		public JSONEntity Replace(JSONEntity JSONEntity, int Index)
		{
			this.List.Item(Index) = JSONEntity;
			return JSONEntity;
		}
	}
}
