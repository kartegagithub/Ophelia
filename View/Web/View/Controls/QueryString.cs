using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
namespace Ophelia.Web.View.Controls
{
	public class QueryString
	{
		private HttpRequest oRequest;
		private SortedList oInnerList;
		public int ItemCount = 0;
		private bool ValueCreated = false;
		private string sValue = "";
		private IList oKeyList;
		private IList oValueList;
		private string sScriptName = "";
		private string sRawUrl = "";
		public int DefaultValueOfIntegerValues;
		public HttpRequest Request {
			get { return this.oRequest; }
			set { this.oRequest = value; }
		}
		public SortedList InnerList {
			get { return this.oInnerList; }
		}
		public string ScriptName {
			get { return this.sScriptName; }
		}
		public string RawUrl {
			get { return this.sRawUrl; }
		}
		public string this[string Key, string DefaultValue = "", bool AllowInjection = false] {
			get {
				if ((this.InnerList[Key] != null)) {
					if (AllowInjection) {
						return this.InnerList[Key].ToString();
					}
					return Utilities.CheckStringForInjection(this.InnerList[Key].ToString());
				}
				return DefaultValue;
			}
		}
		public IList KeyList {
			get {
				if (this.oKeyList == null)
					this.oKeyList = this.InnerList.GetKeyList();
				return this.oKeyList;
			}
		}
		public IList ValueList {
			get {
				if (this.oValueList == null)
					this.oValueList = this.InnerList.GetValueList();
				return this.oValueList;
			}
		}
		public string Value {
			get {
				this.Create();
				return this.sValue;
			}
		}
		public void Update(string Identifier, string Value)
		{
			if (this.InnerList[Identifier] == null) {
				this.Add(Identifier, Value);
			} else {
				this.ValueCreated = false;
				this.InnerList[Identifier] = Value;
			}
		}
		public void Remove(string Identifier)
		{
			if (this.InnerList[Identifier] != null) {
				this.InnerList.Remove(Identifier);
				ItemCount -= 1;
			}
		}
		public void Add(string Identifier, string Value)
		{
			if (this.InnerList[Identifier] == null) {
				this.ValueCreated = false;
				this.InnerList.Add(Identifier, Value);
				ItemCount += 1;
			} else {
				this.Update(Identifier, Value);
			}
		}
		public void Create()
		{
			if (!this.ValueCreated) {
				int n = 0;
				this.sValue = this.ScriptName;
				if (this.sValue.IndexOf("?") < 0 && this.ItemCount > 0)
					this.sValue += "?";
				for (n = 0; n <= this.ItemCount - 1; n++) {
					this.sValue += this.KeyList[n] + "=" + this.ValueList[n] + "&";
				}
				if (this.sValue.Length - 1 == this.sValue.LastIndexOf("&"))
					this.sValue = Strings.Left(this.sValue, this.sValue.Length - 1);
				this.ValueCreated = true;
			}
		}
		public string QueryParametersInString {
			get {
				string sQueryParametersInString = "";
				for (int n = 0; n <= this.ItemCount - 1; n++) {
					sQueryParametersInString += this.KeyList[n] + "=" + this.ValueList[n] + "&";
				}
				if (sQueryParametersInString.Length - 1 == sQueryParametersInString.LastIndexOf("&"))
					sQueryParametersInString = Strings.Left(sQueryParametersInString, sQueryParametersInString.Length - 1);
				return sQueryParametersInString;
			}
		}
		private void AddExistingIdentifiers()
		{
			if ((this.Request != null)) {
				int n = 0;
				for (n = 0; n <= this.Request.QueryString.Keys.Count - 1; n++) {
					if ((this.Request.QueryString.Keys(n) != null)) {
						if (this.InnerList[this.Request.QueryString.Keys(n)] == null) {
							this.InnerList.Add(this.Request.QueryString.Keys(n), this.Request.QueryString.Item(this.Request.QueryString.Keys(n)));
							ItemCount += 1;
						} else {
							this.InnerList[this.Request.QueryString.Keys(n)] = this.Request.QueryString.Item(this.Request.QueryString.Keys(n));
						}
					}
				}
				for (n = 0; n <= this.Request.Form.Keys.Count - 1; n++) {
					if ((this.Request.Form.Keys(n) != null)) {
						if (this.InnerList[this.Request.Form.Keys(n)] == null) {
							this.InnerList.Add(this.Request.Form.Keys(n), this.Request.Form.Item(this.Request.Form.Keys(n)));
							ItemCount += 1;
						} else {
							this.InnerList[this.Request.Form.Keys(n)] = this.Request.Form.Item(this.Request.Form.Keys(n));
						}
					}
				}
				this.sRawUrl = this.Request.RawUrl;
			} else if (!string.IsNullOrEmpty(this.RawUrl)) {
				if (this.RawUrl.IndexOf("?") > -1) {
					string[] sIdentifiers = Strings.Split(Strings.Right(this.RawUrl, this.RawUrl.Length - this.RawUrl.IndexOf("?") - 1), "&");
					int n = 0;
					string Key = "";
					string Value = "";
					while (!(n > sIdentifiers.Length - 1)) {
						Value = "";
						Key = "";
						if (sIdentifiers[n].IndexOf("=") > -1) {
							Key = Strings.Left(sIdentifiers[n], sIdentifiers[n].IndexOf("="));
						}
						if (sIdentifiers[n].Length - sIdentifiers[n].IndexOf("=") - 1 > -1) {
							Value = Strings.Right(sIdentifiers[n], sIdentifiers[n].Length - sIdentifiers[n].IndexOf("=") - 1);
						}
						this.Add(Key, Value);
						n += 1;
					}
				}
			}
			if (this.RawUrl.IndexOf("?") > -1) {
				this.sScriptName = Strings.Left(this.RawUrl, this.RawUrl.IndexOf("?"));
			} else {
				this.sScriptName = this.RawUrl;
			}
			View.Web.Controls.ServerSide.ScriptManager.ScriptDrawer.ArrangeQueryStringForAjaxRequest("AjaxRequest", this);
			View.Web.Controls.ServerSide.ScriptManager.ScriptDrawer.ArrangeQueryStringForAjaxRequest("AjaxRequestWithApplication", this);
			View.Web.Controls.ServerSide.ScriptManager.ScriptDrawer.ArrangeQueryStringForAjaxRequest("DisplayFile", this);
			View.Web.Controls.ServerSide.ScriptManager.ScriptDrawer.ArrangeQueryStringForAjaxRequest("DisplayImage", this);
		}
		public void ChangeDefaultValueOfIntegerValues(int NewDefaultValue)
		{
			this.DefaultValueOfIntegerValues = NewDefaultValue;
		}
		public DateTime DateTimeValue(string ParameterString)
		{
			return this.DateTimeValue(ParameterString, DateTime.MaxValue, DateTime.MinValue, DateTime.MinValue);
		}
		public DateTime DateTimeValue(string ParameterString, DateTime DefaultValue)
		{
			return this.DateTimeValue(ParameterString, DateTime.MaxValue, DateTime.MinValue, DefaultValue);
		}
		public DateTime DateTimeValue(string ParameterString, DateTime MaximumValue, DateTime MinimumValue)
		{
			return this.DateTimeValue(ParameterString, DateTime.MaxValue, DateTime.MinValue, DateTime.MinValue);
		}
		public DateTime DateTimeValue(string ParameterString, DateTime MaximumValue, DateTime MinimumValue, DateTime DefaultValue)
		{
			string Value = this[ParameterString, "", true];
			if (Value != string.Empty) {
				DateTime ReturnValue = DefaultValue;
				if (DateTime.TryParse(Value, out ReturnValue)) {
					return ReturnValue;
				}
			}
			return DefaultValue;
		}
		public long LongValue(string ParameterString, long DefaultValue = long.MinValue)
		{
			return this.LongValue(ParameterString, long.MaxValue, long.MinValue, DefaultValue);
		}
		public long LongValue(string ParameterString, long MaximumValue, long MinimumValue, long DefaultValue = long.MinValue)
		{
			if (this.DefaultValueOfIntegerValues > decimal.MinValue && DefaultValue == long.MinValue) {
				DefaultValue = this.DefaultValueOfIntegerValues;
			}
			if (this[ParameterString] != null) {
				long ReturnValue = DefaultValue;
				if (long.TryParse(this[ParameterString], out ReturnValue)) {
					return ReturnValue;
				}
			}
			return DefaultValue;
		}
		public byte ByteValue(string ParameterString, byte DefaultValue = byte.MinValue)
		{
			return this.ByteValue(ParameterString, byte.MaxValue, byte.MinValue, DefaultValue);
		}
		public byte ByteValue(string ParameterString, byte MaximumValue, byte MinimumValue, byte DefaultValue = byte.MinValue)
		{
			if (this.Request(ParameterString) != null) {
				byte ReturnValue = DefaultValue;
				if (this[ParameterString].ToString() == "on") {
					return 1;
				} else if (this[ParameterString].ToString() == "off") {
					return 0;
				}
				if (byte.TryParse(this.Request(ParameterString), out ReturnValue)) {
					return ReturnValue;
				}
			}
			return DefaultValue;
		}
		public decimal DecimalValue(string ParameterString, decimal DefaultValue = decimal.MinValue)
		{
			return this.DecimalValue(ParameterString, decimal.MaxValue, decimal.MinValue, DefaultValue);
		}
		public decimal DecimalValue(string ParameterString, decimal MaximumValue, decimal MinimumValue, decimal DefaultValue = decimal.MinValue)
		{
			if (this.DefaultValueOfIntegerValues > decimal.MinValue && DefaultValue == decimal.MinValue) {
				DefaultValue = this.DefaultValueOfIntegerValues;
			}
			if (this[ParameterString] != null) {
				decimal ReturnValue = DefaultValue;
				string Value = this[ParameterString].ToString().Replace(".", ",");
				if (this[ParameterString].ToString() == "on") {
					Value = 1;
				} else if (this[ParameterString].ToString() == "off") {
					Value = 0;
				} else if (this[ParameterString].ToString() == "*") {
					Value = -1;
				}
				if (decimal.TryParse(Value, out ReturnValue)) {
					return ReturnValue;
				}
			}
			return DefaultValue;
		}
		public int IntegerValue(string ParameterString, int DefaultValue = int.MinValue)
		{
			return this.IntegerValue(ParameterString, int.MaxValue, int.MinValue, DefaultValue);
		}
		public int IntegerValue(string ParameterString, int MaximumValue, int MinimumValue, int DefaultValue = int.MinValue)
		{
			if (this.DefaultValueOfIntegerValues > decimal.MinValue && DefaultValue == int.MinValue) {
				DefaultValue = this.DefaultValueOfIntegerValues;
			}
			if (this[ParameterString] != null) {
				string Value = this[ParameterString];
				if (this[ParameterString].ToString() == "on") {
					Value = 1;
				} else if (this[ParameterString].ToString() == "off") {
					Value = 0;
				} else if (this[ParameterString].ToString() == "*") {
					Value = -1;
				}
				int ReturnValue = DefaultValue;
				if (int.TryParse(Value, out ReturnValue)) {
					return ReturnValue;
				}
			}
			return DefaultValue;
		}
		public QueryString()
		{
			this.oInnerList = new SortedList();
		}
		public QueryString(string RawUrl) : this()
		{
			this.sRawUrl = RawUrl;
			this.AddExistingIdentifiers();
		}
		public QueryString(HttpRequest Request) : this()
		{
			this.oRequest = Request;
			this.AddExistingIdentifiers();
		}
		public QueryString(string RawUrl, HttpRequest Request) : this()
		{
			this.sRawUrl = RawUrl;
			this.oRequest = Request;
			this.AddExistingIdentifiers();
		}
	}
}
