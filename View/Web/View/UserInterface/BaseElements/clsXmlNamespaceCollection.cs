using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class XmlNamespaceCollection : Ophelia.Application.Base.CollectionBase
	{
		public new XmlNamespace this[int Index] {
			get { return base.Item(Index); }
		}
		public XmlNamespace Add(string Namespace)
		{
			return this.Add("", Namespace);
		}
		public XmlNamespace Add(string Title, string Namespace)
		{
			XmlNamespace XmlNamespace = new XmlNamespace();
			XmlNamespace.Title = Title;
			XmlNamespace.Namespace = Namespace;
			return this.Add(XmlNamespace);
		}
		public XmlNamespace Add(XmlNamespace XmlNamespace)
		{
			bool Found = false;
			for (int i = 0; i <= this.Count - 1; i++) {
				if (this[i].IsEqualTo(XmlNamespace)) {
					Found = true;
					break; // TODO: might not be correct. Was : Exit For
				}
			}
			if (!Found) {
				this.InnerList.Add(XmlNamespace);
				return XmlNamespace;
			}
			return null;
		}
		internal string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				Content.Add(this[i].Draw);
			}
			return Content.Value;
		}
	}
}
