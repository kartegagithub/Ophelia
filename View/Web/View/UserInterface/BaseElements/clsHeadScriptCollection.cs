using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.UI
{
	public class HeadScriptCollection : Ophelia.Application.Base.CollectionBase
	{

		public new HeadScript this[int Index] {
			get { return base.Item(Index); }
		}

		public HeadScript Add(string Source, bool AddScriptTag)
		{
			HeadScript HeadScript = new HeadScript(Source, AddScriptTag);
			return this.Add(HeadScript);
		}

		private HeadScript Add(HeadScript HeadScript)
		{
			bool Found = false;
			for (int i = 0; i <= this.Count - 1; i++) {
				if (this[i].IsEqualTo(HeadScript)) {
					Found = true;
					if (Found) {
						return this[i];
					}
				}
			}
			if (!Found) {
				this.InnerList.Add(HeadScript);
				return HeadScript;
			}
			return null;
		}

		public string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				Content.Add(this[i].Draw());
			}
			return Content.Value;
		}

	}
}
