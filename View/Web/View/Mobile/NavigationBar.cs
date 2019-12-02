using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Mobile
{
	public class NavigationBar : Ophelia.Web.View.Controls.ComplexWebControl
	{
		private Panel oTitleArea;
		private Panel oLeftButtonArea;
		private Panel oRightButtonArea;
		private Label oBreadCrumbsArea;
		public Panel TitleArea {
			get { return this.oTitleArea; }
		}
		public Panel RightButtonArea {
			get { return this.oRightButtonArea; }
		}
		public Panel LeftButtonArea {
			get { return this.oLeftButtonArea; }
		}
		public Label BreadCrumbsArea {
			get { return this.oBreadCrumbsArea; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			Panel Panel = new Panel("nav_area");
			Panel.Controls.Add(this.LeftButtonArea);
			Panel.Controls.Add(this.TitleArea);
			Panel.Controls.Add(this.RightButtonArea);
			Content.Add(Panel.Draw);
		}
		public NavigationBar()
		{
			this.oTitleArea = new Panel("nav_title_area");
			this.oLeftButtonArea = new Panel("nav_left_area");
			this.oRightButtonArea = new Panel("nav_rigth_area");
			this.oBreadCrumbsArea = new Label("nav_breadcrumb_area");
		}
	}
}
