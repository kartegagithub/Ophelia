using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class HeadLinkCollection : Ophelia.Application.Base.CollectionBase
	{
		private Header oHeader;
		internal Header Header {
			get { return this.oHeader; }
		}
		public new HeadLink this[int Index] {
			get { return base.Item(Index); }
		}
		public HeadLink Add(string Source, HeadLink.ReleationShipType Type)
		{
			HeadLink HeadLink = new HeadLink(this, Source, Type);
			return this.Add(HeadLink);
		}
		private HeadLink Add(HeadLink HeadLink)
		{
			bool Found = false;
			for (int i = 0; i <= this.Count - 1; i++) {
				if (this[i].IsEqualTo(HeadLink)) {
					Found = true;
					if (Found) {
						return this[i];
					}
				}
			}
			if (!Found) {
				this.InnerList.Add(HeadLink);
				return HeadLink;
			}
			return null;
		}
		public string Draw(HeadLink.HeadlineDrawType HeadlineDrawType)
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				if (HeadlineDrawType == this[i].DrawType) {
					Content.Add(this[i].Draw);
				}
			}
			return Content.Value;
		}
		public string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				Content.Add(this[i].Draw);
			}
			return Content.Value;
		}
		public HeadLinkCollection(Header Header)
		{
			this.oHeader = Header;
		}
	}
}

