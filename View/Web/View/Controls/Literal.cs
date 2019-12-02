using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Forms;
namespace Ophelia.Web.View.Controls
{
	public class Literal : Container
	{
		public override void OnBeforeDraw(Content Content)
		{
			Content.Add(this.Content.Value);
			base.DrawControls(Content);
		}
	}
}
