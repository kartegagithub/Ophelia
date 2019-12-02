using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.JSON
{
	public class JSONEntity
	{
		private JSON.JSONEntityCollection oCollection;
		private Hashtable oData = new Hashtable();
		private string sTitle = "";
		public Hashtable Data {
			get { return this.oData; }
		}
		public string Title {
			get { return this.sTitle; }
		}
		public string GetData(string PropertyName, string DefaultValue = "")
		{
			if (this.Data[PropertyName] == null) {
				return DefaultValue;
			}
			return this.Data[PropertyName].ToString();
		}
		public Generic.Dictionary<string, object> GetCollection(string PropertyName)
		{
			return this.Data[PropertyName];
		}
		public void DropData(string PropertyName)
		{
			this.Data.Remove(PropertyName);
		}
		public void SetData(string PropertyName, string Value)
		{
			this.Data[PropertyName] = Value;
		}
		public void SetData(string PropertyName, JSONEntityCollection Value)
		{
			this.Data[PropertyName] = Value;
		}
		public void SetData(string PropertyName, string PropertiesName, EntityCollection Collection)
		{
			JSONEntityCollection JSCollection = new JSONEntityCollection();
			for (int i = 0; i <= Collection.Count - 1; i++) {
				JSONEntity JSEntity = new JSONEntity(JSCollection);
				JSEntity.SetData(PropertiesName, Collection(i));
				JSCollection.Add(JSEntity);
			}
			this.Data[PropertyName] = JSCollection;
		}
		public void SetData(string PropertiesName, Entity Entity)
		{
			this.SetData("ID", Entity.ID);
			string[] Properties = PropertiesName.Split(",");
			string TempPropertyName = "";
			string[] TempProperty = null;
			bool IsEntity = false;
			for (int i = 0; i <= Properties.Count - 1; i++) {
				IsEntity = false;
				TempPropertyName = Properties[i];
				if (TempPropertyName.IndexOf(".") > 0) {
					IsEntity = true;
				}
				System.Reflection.PropertyInfo PropertyInfo = null;
				object Value = null;
				TempProperty = TempPropertyName.Split(".");
				if (TempProperty.Count > 1) {
					for (int j = 0; j <= TempProperty.Count - 1; j++) {
						if (Value == null) {
							Value = Entity.GetType.GetProperty(TempProperty[j]).GetValue(Entity, null);
						} else {
							Value = Value.GetType().GetProperty(TempProperty[j]).GetValue(Value, null);
						}
						if (Value == null) {
							break; // TODO: might not be correct. Was : Exit For
						}
					}
				} else {
					Value = Entity.GetType.GetProperty(TempPropertyName).GetValue(Entity, null);
				}
				if (Value == null) {
					if (IsEntity) {
						if (Properties[i].IndexOf("ID") > 0) {
							this.SetData(Properties[i], "0");
						} else {
							this.SetData(Properties[i], "");
						}
					} else {
						this.SetData(Properties[i], "");
					}
					continue;
				}
				if (Value.GetType().FullName.IndexOf("Entity") > 0) {
					this.SetData(TempPropertyName + "ID", Value.ID);
				} else {
					this.SetData(TempPropertyName, Value.ToString());
				}
			}
		}
		public JSON.JSONEntityCollection Collection {
			get { return this.oCollection; }
		}
		public string Draw()
		{
			Content Content = new Content();
			this.Draw(Content);
			return Content.Value;
		}
		internal void Draw(Content Content)
		{
			Content.Add("{");
			if (!string.IsNullOrEmpty(this.Title)) {
				Content.Add("\"" + this.Title + "\":{");
			}
			for (int i = 0; i <= this.Data.Count - 1; i++) {
				if (this.Data[this.Data.Keys(i)] != null && this.Data[this.Data.Keys(i)].GetType().Name == "JSONEntityCollection") {
					((JSONEntityCollection)this.Data[this.Data.Keys(i)]).Title = "";
					Content.Add("\"" + this.Data.Keys(i) + "\":[");
					((JSONEntityCollection)this.Data[this.Data.Keys(i)]).Draw(Content);
					Content.Add("]");
				} else {
					string Key = this.Data.Keys(i).ToString.ToLower.Replace(".", "").Replace("ı", "i");
					if (this.Data[this.Data.Keys(i)] == null || object.ReferenceEquals(this.Data[this.Data.Keys(i)], DBNull.Value)) {
						Content.Add("\"" + Key + "\":\"" + "" + "\"");
					} else {
						Content.Add("\"" + Key + "\":\"" + this.Data[this.Data.Keys(i)].ToString().Replace(Constants.vbCrLf, "\\n").Replace(Strings.Chr(13), "\\n").Replace(Strings.Chr(10), "\\r").Replace(Strings.Chr(0), "").Replace("\"", "'") + "\"");
					}
				}
				if (i != this.Data.Count - 1) {
					Content.Add(",");
				}
			}
			if (!string.IsNullOrEmpty(this.Title)) {
				Content.Add("}");
			}
			Content.Add("}");
		}
		public JSONEntity(JSON.JSONEntityCollection Collection)
		{
			this.oCollection = Collection;
		}
		public JSONEntity(string Title)
		{
			this.sTitle = Title;
		}
	}
}
