using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public interface IFilterBoxDataSource
	{
		ICollection GetCollection(string SearchMember, string SearchableValue);
	}
}
