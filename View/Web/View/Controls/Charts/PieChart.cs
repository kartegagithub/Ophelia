using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
namespace Ophelia.Web.View.Controls.Charts
{
	public class PieChart
	{
		private bool _bShowPercent = true;
		private bool _bShowValue = true;
		private int _nBorder = 0;
		private int _nWidth = 100;
		private int _nHeight = 100;
		private int _nDepth = 0;
		private string _sBgColor = "white";
		private ArrayList _oData = new ArrayList();
		public bool ShowZeroValues = false;
		public bool ShowPercent {
			set { _bShowPercent = value; }
		}
		public bool ShowValue {
			set { _bShowValue = value; }
		}
		public int Border {
			set { _nBorder = value; }
		}
		public int Width {
			set { _nWidth = value; }
		}
		public int Height {
			set { _nHeight = value; }
		}
		public int Depth {
			set { _nDepth = value; }
		}
		public string BackgroundColor {
			set { _sBgColor = value; }
		}
		public string Title = "";
		public string Draw()
		{
			if (this._oData.Count > 0) {
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(Constants.vbLf + "<table cellpadding=\"2\" border=" + _nBorder.ToString() + " cellspacing=\"2\" style=\"border:1px #000000 solid;\"><tr><td bgcolor=\"#C0C0C0\" style=\"border:1px #000000 solid;\">");
				sb.Append(Constants.vbLf + Title);
				sb.Append(Constants.vbLf + "</td></tr><tr>");
				sb.Append(Constants.vbLf + "<td align=\"center\">");
				Image Image = new Image("", "?DisplayChartImage=width,," + _nWidth.ToString() + "$$$height,," + _nHeight.ToString() + "$$$depth,," + _nDepth.ToString() + "$$$bgcolor,," + Strings.Replace(_sBgColor, "#", "") + "$$$colors,," + GetColors() + "$$$data,," + GetData());
				sb.Append(Image.Draw);
				sb.Append(Constants.vbLf + "<td><div style='OVERFLOW: auto; height=" + (_nHeight + 10).ToString() + "px'>");
				sb.Append(GetLegend());
				sb.Append(Constants.vbLf + "</div></td></tr>");
				sb.Append(Constants.vbLf + "</table>");
				return sb.ToString();
			}
			return "";
		}
		public void MapColor(int ID, string Color)
		{
			this.ColorMap.Add(ID, Color);
		}
		private ArrayList DefaultColors = new ArrayList();
		private Hashtable ColorMap = new Hashtable();
		public void SetData(EntityCollectionCollection ECC)
		{
			int i = 0;
			for (i = 0; i <= ECC.Count - 1; i++) {
				if (i < this.DefaultColors.Count) {
					string Color = null;
					if (this.ColorMap.Count > 0) {
						Color = this.ColorMap[ECC(i).Definition.Groupers(0).Entity.ID];
					} else {
						Color = this.DefaultColors[i];
					}
					this.AddSlice(ECC(i).Definition.Groupers(0).Entity.Name, Color, ECC(i).Count);
					if ((this._oData[i] != null)) {
						this._oData[i]("ID") = ECC(i).Definition.Groupers(0).Entity.ID;
					}
				} else {
					return;
				}
			}
		}
		public void AddSlice(string sName, string sColor, decimal iValue)
		{
			if (Information.IsNumeric(iValue)) {
				if (this.ShowZeroValues || iValue > 0) {
					Hashtable oHashtable = new Hashtable();
					if (string.IsNullOrEmpty(sName))
						sName = "Bilinmiyor";
					oHashtable["Name"] = sName;
					oHashtable["Color"] = sColor;
					oHashtable["Value"] = iValue;
					_oData.Add(oHashtable);
				}
			}
		}
		public void AddSlice(string sName, Color oColor, decimal iValue)
		{
			if (Information.IsNumeric(iValue)) {
				if (this.ShowZeroValues || iValue > 0) {
					Hashtable oHashtable = new Hashtable();
					oHashtable["Name"] = sName;
					oHashtable["Color"] = oColor.Name;
					oHashtable["Value"] = iValue;
					_oData.Add(oHashtable);
				}
			}
		}
		private string GetData()
		{
			string sTemp = "";
			IEnumerator oEnumerator = _oData.GetEnumerator();
			while (oEnumerator.MoveNext()) {
				if (oEnumerator.Current != null) {
					Hashtable oHashtable = (Hashtable)oEnumerator.Current;
					string sValue = Convert.ToString(oHashtable["Value"]);

					if (string.IsNullOrEmpty(sTemp)) {
						sTemp += sValue;
					} else {
						sTemp += "," + sValue;
					}
				}
			}
			return sTemp;
		}
		private decimal GetTotal()
		{
			decimal iRet = 0;
			IEnumerator oEnumerator = _oData.GetEnumerator();
			while (oEnumerator.MoveNext()) {
				if (oEnumerator.Current != null) {
					Hashtable oHashtable = (Hashtable)oEnumerator.Current;
					decimal iValue = Convert.ToDecimal(oHashtable["Value"]);
					iRet += iValue;
				}
			}
			return iRet;
		}
		private string GetColors()
		{
			string sTemp = "";
			IEnumerator oEnumerator = _oData.GetEnumerator();
			while (oEnumerator.MoveNext()) {
				if (oEnumerator.Current != null) {
					Hashtable oHashtable = (Hashtable)oEnumerator.Current;
					string sColor = Strings.Replace(Convert.ToString(oHashtable["Color"]), "#", "");

					if (string.IsNullOrEmpty(sTemp)) {
						sTemp += sColor;
					} else {
						sTemp += "," + sColor;
					}
				}
			}
			return sTemp;
		}
		private string GetLegend()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append(Constants.vbLf + "<table border=" + _nBorder.ToString() + " cellspacing=\"2\" cellpadding=\"1\" class=\"Legend\">");
			decimal iTotal = GetTotal();
			IEnumerator oEnumerator = _oData.GetEnumerator();
			while (oEnumerator.MoveNext()) {
				if (oEnumerator.Current != null) {
					Hashtable oHashtable = (Hashtable)oEnumerator.Current;
					string sName = Convert.ToString(oHashtable["Name"]);
					string sColor = Convert.ToString(oHashtable["Color"]);
					decimal iValue = Convert.ToDecimal(oHashtable["Value"]);

					sb.Append(Constants.vbLf + "<tr><td width=10 bgcolor=" + sColor + "><br>");
					sb.Append(Constants.vbLf + "</td><td ALIGN=\"right\">");

					if (_bShowPercent == true) {
						decimal iPercent = (iValue / iTotal) * 100;
						sb.Append(iPercent.ToString("0.##") + "% ");
						sb.Append(Constants.vbLf + "</td><td align=\"right\">");
					}

					if (_bShowValue == true) {
						sb.Append(iValue.ToString("0.##"));
						sb.Append(Constants.vbLf + "</td><td>");
					}
					if (!string.IsNullOrEmpty(this.LinkBase)) {
						sb.Append("<a href=\"" + this.LinkBase + Convert.ToDecimal(oHashtable["ID"]) + "\">" + sName + "</a>");
					} else {
						sb.Append(sName);
					}
					sb.Append(Constants.vbLf + "</td></tr>");
				}
			}

			sb.Append(Constants.vbLf + "</table>");
			return sb.ToString();
		}
		public string LinkBase = "";
		public PieChart()
		{
			this.DefaultColors.Add("Green");
			this.DefaultColors.Add("Blue");
			this.DefaultColors.Add("Yellow");
			this.DefaultColors.Add("Pink");
			this.DefaultColors.Add("Violet");
			this.DefaultColors.Add("Orange");
			this.DefaultColors.Add("Red");
			this.DefaultColors.Add("Black");
			this.DefaultColors.Add("White");
		}
	}
}
