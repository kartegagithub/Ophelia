using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.UI;
namespace Ophelia.Web.View.UI
{
	public static class PageContext
	{
		[ThreadStatic()]
		private static IPage oCurrent;
		public static IPage Current {
			get { return oCurrent; }
		}
		static internal void SetCurrentPage(IPage Page)
		{
			oCurrent = Page;
		}
	}
}
