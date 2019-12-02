using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View
{
	internal class RuleCollection : Ophelia.Application.Base.CollectionBase
	{
		public new Rule this[int Index] {
			get { return base.Item(Index); }
		}
		public void Add(string TextSelector, Style Style)
		{
			this.Add(new Rule(TextSelector, Style));
		}
		public void Add(string TextSelector, string CustomStyle)
		{
			this.Add(new Rule(TextSelector, CustomStyle));
		}
		private void Add(Rule Rule)
		{
			for (int i = 0; i <= this.List.Count - 1; i++) {
				if (this[i].SelectorText == Rule.SelectorText) {
					return;
				}
			}
			this.List.Add(Rule);
		}
		public string Draw()
		{
			Content Content = new Content();
			for (int i = 0; i <= this.Count - 1; i++) {
				Content.Add(this[i].Draw);
			}
			return Content.Value;
		}
	}
}


