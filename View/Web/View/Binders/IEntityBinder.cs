using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders
{
	public interface IEntityBinder
	{
		Client Client { get; }
		Application.Framework.Dictionary Dictionary { get; }
		Entity Entity { get; }
		EntityCollection Collection { get; }
	}
}
