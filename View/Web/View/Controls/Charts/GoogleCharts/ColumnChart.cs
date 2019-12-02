using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Charts.GoogleTool
{
	public class ColumnChart : WebControl
	{
		private SimpleCollection oCollection = new SimpleCollection();
		private int nGraphWidth = 400;
		private int nGraphHeight = 400;
		private bool bIs3D = true;
		private string sTitle = "";
		private string sValueDisplayName = "";
		private string sSelectBehaviour = "";
		protected override void CustomizeScript(ServerSide.ScriptManager.Script Script)
		{
			base.CustomizeScript(Script);
			this.Script.Manager.Add("jsapi", "https://www.google.com/jsapi");
			if (this.Script.Function("googleloadcorechart") == null) {
				this.Script.AddFunction("googleloadcorechart", "");
				this.Script.AppendLine("google.load('visualization', '1.0', {'packages':['corechart']});");
			}
		}
		public void SetProperties(string Title, string ValueDisplayName, int GraphWidth, int GraphHeight, bool Is3D, string SelectBehaivor = "")
		{
			this.Title = Title;
			this.ValueDisplayName = ValueDisplayName;
			this.GraphHeight = GraphHeight;
			this.GraphWidth = GraphWidth;
			this.Is3D = Is3D;
			this.SelectBehaviour = this.SelectBehaviour;
		}
		public string ValueDisplayName {
			get {
				if (string.IsNullOrEmpty(this.sValueDisplayName))
					return "Value";
				return this.sValueDisplayName;
			}
			set { this.sValueDisplayName = value; }
		}
		public string Title {
			get { return this.sTitle; }
			set { this.sTitle = value; }
		}
		public int GraphWidth {
			get { return this.nGraphWidth; }
			set { this.nGraphWidth = value; }
		}
		public int GraphHeight {
			get { return this.nGraphHeight; }
			set { this.nGraphHeight = value; }
		}
		public string SelectBehaviour {
			get { return this.sSelectBehaviour; }
			set { this.sSelectBehaviour = value; }
		}
		public bool Is3D {
			get { return this.bIs3D; }
			set { this.bIs3D = value; }
		}
		public SimpleCollection Collection {
			get { return this.oCollection; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Script.AppendLine("try{" + this.ID + "drawChart();} catch(err){google.setOnLoadCallback(" + this.ID + "drawChart);}");
			if (this.Script.Function(this.ID + "drawChart") == null) {
				ServerSide.ScriptManager.Function DrawChart = this.Script.AddFunction(this.ID + "drawChart");
				this.Script.AppendLine("var " + this.ID + "data;");
				DrawChart.AppendLine(this.ID + "data = new google.visualization.DataTable();");
				DrawChart.AppendLine(this.ID + "data.addColumn('string', 'Name');");
				DrawChart.AppendLine(this.ID + "data.addColumn('number', '" + this.ValueDisplayName.Replace("'", "\\'") + "');");
				DrawChart.AppendLine(this.ID + "data.addColumn({'type': 'string', 'role': 'tooltip', 'p': {'html': true}});");
				DrawChart.AppendLine(this.ID + "data.addRows([");
				for (int i = 0; i <= this.Collection.Count - 1; i++) {
					if (i > 0)
						DrawChart.AppendLine(",");
					string str = "<div style=\"padding:10px;line-height:15px;border-radius:5px;\"><b>" + this.Collection(i).Name.ToString() + "</b><br>" + this.ValueDisplayName + " : " + Convert.ToDecimal(this.Collection(i).Value).ToString("N") + "</div>";
					DrawChart.AppendLine("['" + this.Collection(i).Name.ToString().Replace("'", "\\'") + "', " + this.Collection(i).Value.Replace(",", ".") + ",'" + str + "']");
					//1M$ sales in 2004'

				}
				//Selectedevent
				//chart.setSelection([{row:0,column:1}]);
				DrawChart.AppendLine("]);");
				this.Script.AppendLine("var " + this.ID + "iddata;");
				DrawChart.AppendLine(this.ID + "iddata = new google.visualization.DataTable();");
				DrawChart.AppendLine(this.ID + "iddata.addColumn('number', 'ID');");
				DrawChart.AppendLine(this.ID + "iddata.addRows([");
				for (int i = 0; i <= this.Collection.Count - 1; i++) {
					if (i > 0)
						DrawChart.AppendLine(",");
					DrawChart.AppendLine("[" + this.Collection(i).ID + "]");
				}
				DrawChart.AppendLine("]);");



				if (this.Page.QueryString.IntegerValue("graphmethod", 0) <= 1) {
					this.GraphHeight = 450;
				}

				//ToDo : Sonra Kaldır
				if (this.Page.QueryString.IntegerValue("GraphHeight", 0) > 0) {
					this.GraphHeight = this.Page.QueryString.IntegerValue("GraphHeight", 0);
				}

				DrawChart.AppendLine("var options = {'title':'" + this.Title.Replace("'", "\\'") + "','width':" + this.GraphWidth + ",'height':" + this.GraphHeight + ",'is3D':" + this.Is3D.ToString().ToLower() + ",'tooltip': { 'isHtml': true },'vAxis':{'minValue':0},'hAxis':{'textPosition':'out'}};");
				this.Script.AppendLine("var " + this.ID + "chart;");
				DrawChart.AppendLine(this.ID + "chart= new google.visualization.ColumnChart(document.getElementById('" + this.ID + "'));");
				DrawChart.AppendLine(this.ID + "chart.draw(" + this.ID + "data, options);");
				if (!string.IsNullOrEmpty(this.SelectBehaviour)) {
					ServerSide.ScriptManager.Function CallbackFunction = this.Script.AddFunction(this.ID + "SelectBehaviour", "", "");
					CallbackFunction.AppendLine("var selectedItem = " + this.ID + "chart.getSelection()[0];");
					CallbackFunction.AppendLine(" if (selectedItem) {");
					CallbackFunction.AppendLine("var name = " + this.ID + "data.getValue(selectedItem.row, 0)");
					CallbackFunction.AppendLine("var value = " + this.ID + "data.getValue(selectedItem.row, 1)");
					CallbackFunction.AppendLine("var id = " + this.ID + "iddata.getValue(selectedItem.row, 0)");
					CallbackFunction.AppendLine(this.SelectBehaviour + "(id,name,value);");
					CallbackFunction.AppendLine("}");
					DrawChart.AppendLine("google.visualization.events.addListener(" + this.ID + "chart, 'select'," + this.ID + "SelectBehaviour);");
				}
			}
			Label Label = new Label(this.ID);
			Label.SetStyle(this.Style);
			Content.Add(Label.Draw);
		}
		public ColumnChart(string ID)
		{
			this.ID = ID;
		}
	}
}
