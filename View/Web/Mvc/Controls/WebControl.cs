//using Microsoft.VisualBasic;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Web.UI;
//using Ophelia.Text;

//namespace Ophelia.Web.View.Mvc.Controls
//{
//	public class WebControl : System.Web.UI.Control
//	{
//		protected ContentBuilder oContent;
//		private HtmlTextWriterTag eTag = HtmlTextWriterTag.Div;
//		private QueryString oQueryString;
//		public string OnClick { get; set; }
//		public bool Binded { get; set; }
//		public virtual int Width { get; set; }
//        public virtual IDictionary<string, object> HtmlAttributes { get; set; }
//		public virtual string RequestData {
//			get {
//				if (this.Request != null && !string.IsNullOrEmpty(this.ID)) {
//					return this.Request[this.ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		public QueryString QueryString {
//			get {
//				if (this.oQueryString == null) {
//					this.oQueryString = new QueryString(this.Request);
//				}
//				return this.oQueryString;
//			}
//		}
//		public System.Web.HttpRequest Request {
//			get { return System.Web.HttpContext.Current.Request; }
//		}
//		public System.Web.HttpResponse Response {
//			get { return System.Web.HttpContext.Current.Response; }
//		}
//		protected virtual HtmlTextWriterTag Tag {
//			get { return this.eTag; }
//			set { this.eTag = value; }
//		}
//		public ContentBuilder Content {
//			get {
//				if (this.oContent == null)
//					this.oContent = new ContentBuilder();
//				return this.oContent;
//			}
//		}
//		public virtual string StyleClass { get; set; }
//		protected override void Render(HtmlTextWriter writer)
//		{
//			if (this.Visible) {
//				this.OnBeforeRender(writer);
//				writer.WriteBeginTag(this.Tag.ToString());
//				if (!string.IsNullOrEmpty(this.StyleClass))
//					writer.WriteAttribute(HtmlTextWriterAttribute.Class.ToString(), this.StyleClass);
//				if (!string.IsNullOrEmpty(this.ID)) {
//					writer.WriteAttribute(HtmlTextWriterAttribute.Id.ToString(), this.ID);
//					writer.WriteAttribute(HtmlTextWriterAttribute.Name.ToString(), this.ID);
//				}
//				if (this.Width > -1)
//					writer.WriteAttribute(HtmlTextWriterAttribute.Style.ToString(), "width:" + this.Width + "px;");
//				if (!string.IsNullOrEmpty(this.OnClick))
//					writer.WriteAttribute(HtmlTextWriterAttribute.Onclick.ToString(), this.OnClick);
//                if (this.HtmlAttributes.Count > 0) {
//                    foreach (var item in this.HtmlAttributes)
//                    {
//                        writer.WriteAttribute(item.Key, Convert.ToString(item.Value));
//                    }
//                }
//				this.OnAttributesRender(writer);
//				writer.Write(HtmlTextWriter.TagRightChar);
//				this.OnBeforeWriteContent(writer);
//				if (this.oContent != null)
//					writer.Write(this.oContent.Build());
//				this.OnAfterWriteContent(writer);
//				this.OnBeforeRenderChildren(writer);
//				this.RenderChildren(writer);
//				this.OnAfterRenderChildren(writer);
//				writer.WriteEndTag(this.Tag.ToString());
//				this.OnAfterRender(writer);
//			}
//		}
//		public System.Web.Mvc.MvcHtmlString ToMvcHtmlString()
//		{
//            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
//			System.Web.UI.HtmlTextWriter Writer = new System.Web.UI.HtmlTextWriter(new System.IO.StringWriter(Builder, System.Globalization.CultureInfo.InvariantCulture));
//			this.RenderControl(Writer);
//			return new System.Web.Mvc.MvcHtmlString(Builder.ToString());
//		}

//		protected virtual void OnAttributesRender(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnBeforeRender(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnAfterRender(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnBeforeRenderChildren(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnAfterRenderChildren(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnBeforeWriteContent(HtmlTextWriter writer)
//		{
//		}

//		protected virtual void OnAfterWriteContent(HtmlTextWriter writer)
//		{
//		}
//        public WebControl AddAttribute(string Key, object Value) {
//            this.HtmlAttributes.Add(Key, Value);
//            return this;
//        }

//		public WebControl()
//		{
//			this.Width = -1;
//            this.HtmlAttributes = new Dictionary<string, object>();
//		}
//	}
//}