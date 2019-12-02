using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls.Charts
{
	public class BarChart
	{
		private ArrayList Keys = new ArrayList();
		private ArrayList KeyValues = new ArrayList();
		private ArrayList Columns = new ArrayList();
		public int BarWidth = 12;
		public int MaxBarHeight = 90;
		public string Title = "";
		public int MaxBarCount = 20;
		public decimal MaxValue = 0;
		public decimal MinValue = 0;
		public DateTime BaseDate;
		public string Interval = "";
		public string Draw()
		{
			string Output = "";
			this.GenerateKeys();
			if (this.Keys.Count > 0) {
				int n = 0;
				Output += "<table cellpadding=\"2\" border=\"0\" style=\"border:1px #000000 solid;\"><tr><td bgcolor=\"#C0C0C0\" style=\"border:1px #000000 solid;\">";
				Output += Title;
				Output += "</td></tr><tr><td>";
				Output += "<table border=\"0\">";
				if (this.Columns.Count > 1) {
					for (n = 0; n <= this.Columns.Count - 1; n++) {
						if (!string.IsNullOrEmpty(((Column)this.Columns[n]).Name.Trim())) {
							Output += "<tr><td width=\"10\" bgcolor=\"" + ((Column)this.Columns[n]).BarColor + "\">&nbsp;</td><td>" + ((Column)this.Columns[n]).Name + "</td></tr>";
						}
					}
				}
				Output += "</table>";
				Output += "</td></tr><tr><td>";
				Output += "<table border=\"0\">";
				if (this.MaxValue > 0) {
					Output += "<tr>";
					for (n = 0; n <= this.Keys.Count - 1; n++) {
						Output += "<td valign=\"bottom\" align=\"center\">";
						if (this.Columns.Count > 0)
							Output += "<table cellpadding=\"0\" cellspacing=\"1\"><tr>";
						int i = 0;
						for (i = 0; i <= this.Columns.Count - 1; i++) {
							Output += ((Column)this.Columns[i]).DrawPositiveData(n);
						}
						if (this.Columns.Count > 0)
							Output += "</tr></table>";
						Output += "</td>";
					}
					Output += "</tr>";
				}
				Output += "<tr>";
				for (n = 0; n <= this.Keys.Count - 1; n++) {
					Output += "<td bgcolor=\"#C0C0C0\" align=\"center\">";
					if (!string.IsNullOrEmpty(((Column)this.Columns[0]).LinkBase)) {
						Output += "<a href=\"" + ((Column)this.Columns[0]).LinkBase + this.KeyValues[n] + "\">" + this.Keys[n] + "</a>";
					} else {
						Output += this.Keys[n];
					}
					Output += "</td>";
				}
				Output += "</tr>";
				if (this.MinValue < 0) {
					Output += "<tr>";
					for (n = 0; n <= this.Keys.Count - 1; n++) {
						Output += "<td valign=\"top\" align=\"center\">";
						if (this.Columns.Count > 0)
							Output += "<table cellpadding=\"0\" celspacing=\"0\"><tr>";
						int i = 0;
						for (i = 0; i <= this.Columns.Count - 1; i++) {
							Output += ((Column)this.Columns[i]).DrawNegativeData(n);
						}
						if (this.Columns.Count > 0)
							Output += "</tr></table>";
						Output += "</td>";
					}
					Output += "</tr>";
				}
				Output += "</table>";
				Output += "</td></tr></table>";
			}
			return Output;
		}
		public void AddValue(string Key, decimal Value)
		{
			if (this.Columns.Count == 0)
				this.AddColumn();
			if (this.Keys.Count < this.MaxBarCount) {
				this.Keys.Add(Key);
				this.AddColumnValue(0, Value);
			}
		}
		public void AddColumnValue(int ColumnIndex, decimal Value)
		{
			((Column)this.Columns[ColumnIndex]).AddValue(Value);
		}
		public void Reset()
		{
			this.Columns.Clear();
			this.Keys.Clear();
			this.KeyValues.Clear();
		}
		public void AddColumn(string BarColor = "", string ColumnName = "", string LinkBase = "")
		{
			this.Columns.Add(new Column(this, BarColor));
			this.Columns[this.Columns.Count - 1].LinkBase = LinkBase;
			this.Columns[this.Columns.Count - 1].Name = ColumnName;
		}
		public void AddColumn(EntityCollection Collection, string DateField, string BarColor = "", string ColumnName = "", string LinkBase = "", string SumField = "")
		{
			this.AddColumn(BarColor, ColumnName, LinkBase);
			this.GenerateKeys();
			int n = 0;
			for (n = 0; n <= this.Keys.Count - 1; n++) {
				EntityCollection DataCollection = Collection.Clone();
				DataCollection.Definition.Filters.Add(DateField, KeyValues[n], FilterComparison.GreaterOrEqual);
				DataCollection.Definition.Filters.Add(DateField, KeyValues[n + 1], FilterComparison.Smaller);
				if (!string.IsNullOrEmpty(SumField)) {
					this.AddColumnValue(this.Columns.Count - 1, DataCollection.Average(SumField));
				} else {
					this.AddColumnValue(this.Columns.Count - 1, DataCollection.Count);
				}
			}
		}
		private void GenerateKeys()
		{
			if (this.Keys.Count == 0) {
				DateTime TempDate = this.BaseDate;
				string KeyString = "";
				while (!(Keys.Count >= this.MaxBarCount)) {
					this.KeyValues.Add(TempDate.ToShortDateString());
					switch (Interval) {
						case "Day":
							KeyString = TempDate.Day + "/" + TempDate.Month;
							TempDate = TempDate.AddDays(1);
							break;
						case "Week":
							KeyString = TempDate.Day + "/" + TempDate.Month;
							TempDate = TempDate.AddDays(7);
							break;
						case "Month":
							KeyString = TempDate.Month + "/" + TempDate.Year.ToString().Substring(2, 2);
							TempDate = TempDate.AddMonths(1);
							break;
						case "Year":
							KeyString = TempDate.Year.ToString().Substring(2, 2);
							TempDate = TempDate.AddYears(1);
							break;
					}
					this.Keys.Add(KeyString);
				}
				this.KeyValues.Add(TempDate.ToShortDateString());
			}
		}
		public BarChart(DateTime BaseDate, string Interval)
		{
			this.BaseDate = BaseDate;
			this.Interval = Interval;
		}

		public BarChart()
		{
		}
		private class Column
		{
			private BarChart Chart;
			public ArrayList Data = new ArrayList();
			public string BarColor = "#73548D";
			public string LinkBase = "";
			public string Name = "";
			public int GetBarHeight(decimal Value)
			{
				if (this.GetAbsoluteMax() > 0) {
					return System.Math.Abs(Value) * this.Chart.MaxBarHeight / this.GetAbsoluteMax();
				} else {
					return 0;
				}
			}
			private decimal GetAbsoluteMax()
			{
				return System.Math.Max(System.Math.Abs(this.Chart.MinValue), this.Chart.MaxValue);
			}
			public void AddValue(decimal Value)
			{
				this.Data.Add(Value);
				if (Value > this.Chart.MaxValue)
					this.Chart.MaxValue = Value;
				if (Value < this.Chart.MinValue)
					this.Chart.MinValue = Value;
			}
			public string DrawPositiveData(int Index)
			{
				string Output = "";
				if (this.Chart.Columns.Count > 0)
					Output += "<td valign=\"bottom\">";
				if (this.Data[Index] > 0) {
					string DataValue = null;
					if (!string.IsNullOrEmpty(this.LinkBase)) {
						DataValue = "<a href=\"" + this.LinkBase + this.Chart.KeyValues[Index] + "\">" + this.Data[Index] + "</a>";
					} else {
						DataValue = this.Data[Index];
					}
					Output += DataValue + "<br><div style=\"background-color:" + BarColor + ";height:" + this.GetBarHeight(this.Data[Index]) + "px;width:" + this.Chart.BarWidth + "px;\">&nbsp;</div>";
				} else {
					Output += "&nbsp;";
				}
				if (this.Chart.Columns.Count > 0)
					Output += "</td>";
				return Output;
			}
			public string DrawNegativeData(int Index)
			{
				string Output = "";
				if (this.Chart.Columns.Count > 0)
					Output += "<td valign=\"top\">";
				if (this.Data[Index] < 0) {
					Output += "<div style=\"background-color:" + BarColor + ";height:" + this.GetBarHeight(this.Data[Index]) + "px;width:" + this.Chart.BarWidth + "px;\">&nbsp;</DIV>" + this.Data[Index];
				} else {
					Output += "&nbsp;";
				}
				if (this.Chart.Columns.Count > 0)
					Output += "</td>";
				return Output;
			}
			public Column(BarChart Chart, string BarColor)
			{
				this.Chart = Chart;
				if (!string.IsNullOrEmpty(BarColor)) {
					this.BarColor = BarColor;
				}
			}
		}
	}
}
