using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
namespace Ophelia.Text
{
	[Serializable()]
	public class ContentBuilder : MarshalByRefObject
	{
		private StringBuilder oBaseBuilder;
		private StringBuilder oBuilder;
		private bool CreateBase = true;
		private string sLineBreak = "";
		public string LineBreak {
			get { return this.sLineBreak; }
			set { this.sLineBreak = value; }
		}
		public StringBuilder BaseBuilder {
			get { return this.oBaseBuilder; }
		}
		public StringBuilder Builder {
			get { return this.oBuilder; }
		}
		public void AddBaseContent(string Value)
		{
			if (this.CreateBase)
				this.BaseBuilder.Append(Value);
		}
		public void Add(string Value)
		{
			this.Builder.Append(Value + this.LineBreak);
		}
		public void InsertToStart(string Value)
		{
			this.Builder.Insert(0, Value + this.LineBreak, 1);
		}
		public void InsertToEnd(string Value)
		{
			this.Builder.Insert(this.Builder.ToString().Length, Value + this.LineBreak, 1);
		}
		public void Reset()
		{
			this.Clear();
		}
		public string Build()
		{
			if (this.CreateBase) {
				return this.BaseBuilder.Append(this.Builder.ToString()).ToString();
			} else {
				return this.Builder.ToString();
			}
		}
		public void NewBuilder()
		{
			if ((this.Builder.Length > 0)) {
				try {
					this.oBuilder.Remove(0, this.oBuilder.Length);
					this.oBuilder.Capacity = 0;
				} catch (Exception) {
					this.oBuilder = null;
					this.oBuilder = new StringBuilder();
				}
			}
		}
		public void Clear()
		{
			try {
				if ((this.oBuilder != null) && this.oBuilder.Length > 0) {
					this.oBuilder.Remove(0, this.oBuilder.Length);
					this.oBuilder.Capacity = 0;
				}
			} catch (Exception) {
				this.oBuilder = null;
				this.oBuilder = new StringBuilder();
			}
			try {
				if ((this.oBaseBuilder != null) && this.oBaseBuilder.Length > 0) {
					this.oBaseBuilder.Remove(0, this.oBaseBuilder.Length);
					this.oBaseBuilder.Capacity = 0;
				}
			} catch (Exception) {
				this.oBaseBuilder = null;
				this.oBaseBuilder = new StringBuilder();
			}
		}
		public static ContentBuilder operator +(ContentBuilder ContentBuilder, string Content)
		{
			ContentBuilder.Add(Content);
			return ContentBuilder;
		}
		public ContentBuilder(bool CreateBase = true)
		{
			this.CreateBase = CreateBase;
			if (this.CreateBase)
				this.oBaseBuilder = new StringBuilder();
			this.oBuilder = new StringBuilder();
		}
	}
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
