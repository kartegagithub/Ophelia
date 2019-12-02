using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class MetaTagCollection : Ophelia.Application.Base.CollectionBase
	{
		public new MetaTag this[int Index] {
			get { return base.Item(Index); }
		}
		public MetaTag Add(MetaTag.MetaTagMessageType MessageType, MetaTag.MetaTagType Type, string Content, IfContainsAction IfContainsAction = IfContainsAction.OverWrite)
		{
			return this.Add(new MetaTag(MessageType, Type, Content), IfContainsAction);
		}
		public MetaTag AddAsProperty(string PropertyName, string PropertyValue)
		{
			MetaTag MetaTag = this.Add(MetaTag.MetaTagMessageType.Property, MetaTag.MetaTagType.None, PropertyValue, IfContainsAction.OverWrite);
			MetaTag.PropertyName = PropertyName;
			return MetaTag;
		}
		public MetaTag Add(MetaTag MetaTag, IfContainsAction IfContainsAction = IfContainsAction.OverWrite)
		{
			bool Found = false;
			if (IfContainsAction != MetaTagCollection.IfContainsAction.#ctor()) {
				for (int i = 0; i <= this.Count - 1; i++) {
					if (this[i].IsEqualTo(MetaTag)) {
						Found = true;
						switch (IfContainsAction) {
							case MetaTagCollection.IfContainsAction.Append:
								this[i].Content += MetaTag.Content;
								break;
							case MetaTagCollection.IfContainsAction.OverWrite:
								this[i].Content = MetaTag.Content;
								break;
						}
						if (Found) {
							return this[i];
						}
					}
				}
			}
			if (!Found) {
				this.InnerList.Add(MetaTag);
				return MetaTag;
			} 
			return null;
		}
		public string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				Content.Add(this[i].Draw);
			}
			return Content.Value;
		}
		public enum IfContainsAction
		{
			OverWrite = 1,
			Append = 2,
			Ignore = 3,
			New = 4
		}
	}
}
