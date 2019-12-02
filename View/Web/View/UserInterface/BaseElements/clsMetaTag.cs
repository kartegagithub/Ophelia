using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class MetaTag
	{
		private MetaTagType eType;
		private MetaTagMessageType eMessageType;
		private string sContent;
		private string sPropertyName;
		public MetaTagMessageType MessageType {
			get { return this.eMessageType; }
			set { this.eMessageType = value; }
		}
		public MetaTagType Type {
			get { return this.eType; }
			set { this.eType = value; }
		}
		public string Content {
			get { return this.sContent; }
			set { this.sContent = value; }
		}
		public string PropertyName {
			get { return this.sPropertyName; }
			set { this.sPropertyName = value; }
		}
		public bool IsEqualTo(MetaTag MetaTag)
		{
			if (this.MessageType != MetaTag.MessageType)
				return false;
			if (this.Type != MetaTag.Type)
				return false;
			if (this.PropertyName != MetaTag.PropertyName)
				return false;
			if ((this.Type == MetaTagType.None || this.Type == MetaTagType.ContentType) && this.Content != MetaTag.Content) {
				return false;
			}
			return true;
		}
		public string Draw()
		{
			Ophelia.Web.View.Content Content = new Ophelia.Web.View.Content();
			Content.Add("<meta ");
			switch (this.MessageType) {
				case MetaTagMessageType.Identifier:
					Content.Add("name=\"");
					break;
				case MetaTagMessageType.Information:
					Content.Add("http-equiv=\"");
					break;
				case MetaTagMessageType.Property:
					Content.Add("property=\"").Add(this.PropertyName);
					break;
			}
			switch (this.Type) {
				case MetaTagType.Author:
					Content.Add("author");
					break;
				case MetaTagType.CacheControl:
					Content.Add("cache-control");
					break;
				case MetaTagType.ContentLanguage:
					Content.Add("content-language");
					break;
				case MetaTagType.ContentType:
					Content.Add("content-type");
					break;
				case MetaTagType.Copyright:
					Content.Add("copyright");
					break;
				case MetaTagType.Description:
					Content.Add("description");
					break;
				case MetaTagType.Expires:
					Content.Add("expires");
					break;
				case MetaTagType.GoogleBot:
					Content.Add("googlebot");
					break;
				case MetaTagType.Keywords:
					Content.Add("keywords");
					break;
				case MetaTagType.PragmaNoCache:
					if (string.IsNullOrEmpty(this.Content))
						this.Content = "no-cache";
					Content.Add("pragma");
					break;
				case MetaTagType.Refresh:
					Content.Add("refresh");
					break;
				case MetaTagType.Robots:
					Content.Add("robots");
					break;
				case MetaTagType.ViewPort:
					Content.Add("viewport");
					break;
				case MetaTagType.MobileOptimized:
					Content.Add("mobileoptimized");
					break;
				case MetaTagType.HandheldFriendly:
					Content.Add("handheldfriendly");
					break;
				case MetaTagType.Name:
					Content.Add("name");
					break;
				case MetaTagType.FormatDetection:
					Content.Add("format-detection");
					break;
				case MetaTagType.XUACompatible:
					Content.Add("X-UA-Compatible");
					break;
			}
			Content.Add("\" content=\"").Add(this.Content).Add("\">");
			return Content.Value;
		}

		public MetaTag()
		{
		}
		public MetaTag(MetaTagMessageType MessageType, MetaTagType Type, string Content)
		{
			this.MessageType = MessageType;
			this.Type = Type;
			this.Content = Content;
		}
		public enum MetaTagType : byte
		{
			None = 0,
			Author = 1,
			CacheControl = 2,
			ContentLanguage = 3,
			ContentType = 4,
			Copyright = 5,
			Description = 6,
			Expires = 7,
			Keywords = 8,
			PragmaNoCache = 9,
			Refresh = 10,
			Robots = 11,
			GoogleBot = 12,
			ViewPort = 13,
			MobileOptimized = 14,
			HandheldFriendly = 15,
			Name = 16,
			FormatDetection = 17,
			XUACompatible = 18
		}
		public enum MetaTagMessageType : byte
		{
			Identifier = 1,
			Information = 2,
			Property = 3
		}
	}
}
