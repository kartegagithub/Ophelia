using Microsoft.VisualBasic;
using System.Collections;
using System.Web;
using System;
using Ophelia.Web.Extensions;

namespace Ophelia.Web.Application.Client
{
    public class QueryString
    {
        private HttpRequestBase oRequest;
        private SortedList oInnerList;
        public int ItemCount = 0;
        private bool ValueCreated = false;
        private string sValue = "";
        private IList oKeyList;
        private IList oValueList;
        private string sScriptName = "";
        private string sRawUrl = "";
        public HttpRequestBase Request
        {
            get { return this.oRequest; }
            set { this.oRequest = value; }
        }
        public SortedList InnerList
        {
            get { return this.oInnerList; }
        }
        public string ScriptName
        {
            get { return this.sScriptName; }
        }
        public string RawUrl
        {
            get { return this.sRawUrl; }
        }
        public object Item(string Key)
        {
            return this.InnerList[Key];
        }
        public IList KeyList
        {
            get
            {
                if (this.oKeyList == null)
                    this.oKeyList = this.InnerList.GetKeyList();
                return this.oKeyList;
            }
        }
        public IList ValueList
        {
            get
            {
                if (this.oValueList == null)
                    this.oValueList = this.InnerList.GetValueList();
                return this.oValueList;
            }
        }
        public string Value
        {
            get
            {
                this.Create();
                return this.sValue;
            }
        }
        public void Update(string Identifier, string Value)
        {
            if (string.IsNullOrEmpty(Value))
                Value = "";
            Value = Value.RemoveXSS();
            if (this.InnerList[Identifier] == null)
            {
                this.Add(Identifier, Value);
            }
            else
            {
                this.ValueCreated = false;
                this.InnerList[Identifier] = Value;
            }
        }
        public void Remove(string Identifier)
        {
            if ((this.InnerList[Identifier] != null))
            {
                this.ValueCreated = false;
                this.InnerList.Remove(Identifier);
                ItemCount -= 1;
            }
        }
        public void Add(string Identifier, string Value)
        {
            if (string.IsNullOrEmpty(Value))
                Value = "";
            Value = Value.RemoveXSS();
            if (this.InnerList[Identifier] == null)
            {
                this.ValueCreated = false;
                this.InnerList[Identifier] = Value;
                ItemCount += 1;
            }
            else
            {
                this.Update(Identifier, Value);
            }
        }
        public void Create()
        {
            if (!this.ValueCreated)
            {
                int n = 0;
                this.sValue = this.ScriptName;
                if (this.sValue.IndexOf('?') < 0)
                    this.sValue += "?";
                for (n = 0; n <= this.ItemCount - 1; n++)
                {
                    if (Convert.ToString(this.ValueList[n]) == "true,false" || Convert.ToString(this.ValueList[n]) == "true,true")
                    {
                        this.sValue += this.KeyList[n] + "=true&";
                    }
                    else if (Convert.ToString(this.ValueList[n]) == "false,true" || Convert.ToString(this.ValueList[n]) == "false,false")
                    {
                        this.sValue += this.KeyList[n] + "=false&";
                    }
                    else
                    {
                        var value = Convert.ToString(this.ValueList[n]);
                        if (string.IsNullOrEmpty(value))
                            value = "";
                        value = value.RemoveXSS();
                        this.sValue += this.KeyList[n] + "=" + this.ValueList[n] + "&";
                    }
                }
                if (this.sValue.Length - 1 == this.sValue.LastIndexOf('&'))
                    this.sValue = Strings.Left(this.sValue, this.sValue.Length - 1);
                this.ValueCreated = true;
            }
        }
        private void AddExistingIdentifiers()
        {
            if ((this.Request != null))
            {
                int n = 0;
                for (n = 0; n <= this.Request.Unvalidated.QueryString.Keys.Count - 1; n++)
                {
                    if ((this.Request.Unvalidated.QueryString.Keys[n] != null) && !string.IsNullOrEmpty(this.Request.Unvalidated.QueryString.Keys[n]))
                    {
                        var value = Convert.ToString(this.Request.Unvalidated.QueryString[this.Request.Unvalidated.QueryString.Keys[n]]);
                        if (string.IsNullOrEmpty(value))
                            value = "";
                        value = value.RemoveXSS();

                        if (this.InnerList[this.Request.Unvalidated.QueryString.Keys[n]] == null)
                        {
                            this.InnerList[this.Request.Unvalidated.QueryString.Keys[n]] = value;
                            ItemCount += 1;
                        }
                        else
                        {
                            this.InnerList[this.Request.Unvalidated.QueryString.Keys[n]] = value;
                        }
                    }
                }
                for (n = 0; n < this.Request.Unvalidated.Form.Keys.Count; n++)
                {
                    if ((this.Request.Unvalidated.Form.Keys[n] != null) && !string.IsNullOrEmpty(this.Request.Unvalidated.Form.Keys[n]))
                    {
                        var value = Convert.ToString(this.Request.Unvalidated.Form[this.Request.Unvalidated.Form.Keys[n]]);
                        if (string.IsNullOrEmpty(value))
                            value = "";
                        value = value.RemoveXSS();

                        if (this.InnerList[this.Request.Unvalidated.Form.Keys[n]] == null)
                        {
                            this.InnerList[this.Request.Unvalidated.Form.Keys[n]] = value;
                            ItemCount += 1;
                        }
                        else
                        {
                            this.InnerList[this.Request.Unvalidated.Form.Keys[n]] = value;
                        }
                    }
                }
                //For n = 0 To Me.Request.Form.Keys.Count - 1
                //    If Not Me.Request.Form.Keys(n) Is Nothing AndAlso Me.Request.Form.Keys(n) <> "" Then
                //        If Me.InnerList(Me.Request.Form.Keys(n)) Is Nothing Then
                //            Me.InnerList(Me.Request.Form.Keys(n)) = Me.Request.Form.Item(Me.Request.Form.Keys(n))
                //            ItemCount += 1
                //        Else
                //            Me.InnerList.Item(Me.Request.Form.Keys(n)) = Me.Request.Form.Item(Me.Request.Form.Keys(n))
                //        End If
                //    End If
                //Next
                this.sRawUrl = this.Request.RawUrl;
            }
            else if (!string.IsNullOrEmpty(this.RawUrl))
            {
                if (this.RawUrl.IndexOf('?') > -1)
                {
                    string[] sIdentifiers = Strings.Split(Strings.Right(this.RawUrl, this.RawUrl.Length - this.RawUrl.IndexOf('?') - 1), "&");
                    int n = 0;
                    string Key = "";
                    string Value = "";
                    while (!(n > sIdentifiers.Length - 1))
                    {
                        Value = "";
                        Key = "";
                        if (sIdentifiers[n].IndexOf('=') > -1)
                        {
                            Key = Strings.Left(sIdentifiers[n], sIdentifiers[n].IndexOf('='));
                        }
                        if (sIdentifiers[n].Length - sIdentifiers[n].IndexOf('=') - 1 > -1)
                        {
                            Value = Strings.Right(sIdentifiers[n], sIdentifiers[n].Length - sIdentifiers[n].IndexOf('=') - 1);
                        }
                        if (!string.IsNullOrEmpty(Key))
                            this.Add(Key, Value);
                        n += 1;
                    }
                }
            }
            if (this.RawUrl.IndexOf('?') > -1)
            {
                this.sScriptName = Strings.Left(this.RawUrl, this.RawUrl.IndexOf('?'));
            }
            else
            {
                this.sScriptName = this.RawUrl;
            }
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

        public QueryString(HttpRequestBase Request) : this()
        {
            this.oRequest = Request;
            this.AddExistingIdentifiers();
        }

        public QueryString(HttpRequest Request) : this(Request.ToRequestBase()) { }

        public QueryString(string RawUrl, HttpRequestBase Request)
            : this()
        {
            this.sRawUrl = RawUrl;
            this.oRequest = Request;
            this.AddExistingIdentifiers();
        }
    }
}
