using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Charts.GoogleTool
{
	public class PieChart : WebControl
	{
		private SimpleCollection oCollection = new SimpleCollection();
		private int nGraphWidth = 400;
		private int nGraphHeight = 400;
		private bool bIs3D = true;
		private string sTitle = "";
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
		public void SetProperties(string Title, int GraphWidth, int GraphHeight, bool Is3D, string SelectBehaivor = "")
		{
			this.Title = Title;
			this.GraphHeight = GraphHeight;
			this.GraphWidth = GraphWidth;
			this.Is3D = Is3D;
			this.SelectBehaviour = this.SelectBehaviour;
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
			bool HasValue = false;
			for (int i = this.Collection.Count - 1; i >= 0; i += -1) {
				if (this.Collection(i).Value > 0) {
					HasValue = true;
					break; // TODO: might not be correct. Was : Exit For
				}
			}
			if (!HasValue) {
				Label EmptyLabel = new Label(this.ID);
				EmptyLabel.SetStyle(this.Style);
				EmptyLabel.Style.Display = DisplayMethod.Hidden;
				Content.Add(EmptyLabel.Draw);
				return;
			}

			this.Script.AppendLine("try{" + this.ID + "drawChart();} catch(err){google.setOnLoadCallback(" + this.ID + "drawChart);}");
			if (this.Script.Function(this.ID + "drawChart") == null) {
				ServerSide.ScriptManager.Function DrawChart = this.Script.AddFunction(this.ID + "drawChart");
				this.Script.AppendLine("var " + this.ID + "data;");
				DrawChart.AppendLine(this.ID + "data = new google.visualization.DataTable();");
				DrawChart.AppendLine(this.ID + "data.addColumn('string', 'Name');");
				DrawChart.AppendLine(this.ID + "data.addColumn('number', 'Value');");
				DrawChart.AppendLine(this.ID + "data.addColumn('number', 'ID');");
				DrawChart.AppendLine(this.ID + "data.addRows([");
				bool AddComma = false;
				for (int i = 0; i <= this.Collection.Count - 1; i++) {
					if (Convert.ToDecimal(this.Collection(i).Value) > 0) {
						if (AddComma)
							DrawChart.AppendLine(",");
						AddComma = true;
						DrawChart.AppendLine("['" + this.Collection(i).Name.ToString().Replace("'", "\\'") + "', " + Convert.ToDecimal(this.Collection(i).Value).ToString().Replace(",", ".") + "," + this.Collection(i).ID + "]");
					}
				}
				DrawChart.AppendLine("]);");
				DrawChart.AppendLine("var options = {'title':'" + this.Title.Replace("'", "\\'") + "','width':" + this.GraphWidth + ",'height':" + this.GraphHeight + ",'is3D':" + this.Is3D.ToString().ToLower() + ", 'chartArea':{left:0,top:40,width:\"100%\"}};");
				DrawChart.AppendLine("var " + this.ID + "chart = new google.visualization.PieChart(document.getElementById('" + this.ID + "'));");
				DrawChart.AppendLine(this.ID + "chart.draw(" + this.ID + "data, options);");
				if (!string.IsNullOrEmpty(this.SelectBehaviour)) {
					ServerSide.ScriptManager.Function CallbackFunction = this.Script.AddFunction(this.ID + "SelectBehaviour", "", "");
					CallbackFunction.AppendLine("var selectedItem = " + this.ID + "chart.getSelection()[0];");
					CallbackFunction.AppendLine(" if (selectedItem) {");
					CallbackFunction.AppendLine("var name = " + this.ID + "data.getValue(selectedItem.row, 0)");
					CallbackFunction.AppendLine("var value = " + this.ID + "data.getValue(selectedItem.row, 1)");
					CallbackFunction.AppendLine("var id = " + this.ID + "data.getValue(selectedItem.row, 2)");
					CallbackFunction.AppendLine(this.SelectBehaviour + "(id,name,value);");
					CallbackFunction.AppendLine("}");

					DrawChart.AppendLine("google.visualization.events.addListener(" + this.ID + "chart, 'select'," + this.ID + "SelectBehaviour);");
				}
			}
			Label Label = new Label(this.ID);
			Label.SetStyle(this.Style);
			Content.Add(Label.Draw);
		}
		public PieChart(string ID)
		{
			this.ID = ID;
		}
	}
}
