using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Controls.ServerSide;
using Ophelia.Web.View.Controls.ServerSide.ScriptManager;
namespace Ophelia.Web.View.UI
{
	public interface IPage
	{
		Client Client { get; }
		Header Header { get; }
		StyleSheet StyleSheet { get; }
		ScriptCollection ScriptManager { get; }
		Body Body { get; }
		Controls.WebControlCollection Controls { get; }
		ContentManager ContentManager { get; }
		CacheManager CacheManager { get; }
		HttpRequest Request { get; }
		SessionState.HttpSessionState Session { get; }
		HttpResponse Response { get; }
		HttpContext HttpContext { get; }
		Controls.QueryString QueryString { get; }
		int InstanceID { get; }
		bool IsAjaxRequest { get; set; }
		bool UseHtml5 { get; }
		EmbeddedFileProcessingMethod FileUsageType { get; set; }
		AjaxConfiguration AjaxConfiguration { get; }
	}
}
