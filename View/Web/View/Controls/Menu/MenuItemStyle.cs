using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Menu
{
	public class MenuItemStyle : Ophelia.Web.View.Style
	{
		public MenuItemStyle()
		{
			this.Font.Color = "black;";
			this.Font.IsLink = true;
			this.Display = DisplayMethod.Block;
			this.eCursor = Cursor.Pointer;
			this.HorizontalAlignment = View.Web.HorizontalAlignment.Left;
			this.ListStyle = View.Web.ListStyle.Hidden;
			this.Padding = 0;
		}
	}
}
