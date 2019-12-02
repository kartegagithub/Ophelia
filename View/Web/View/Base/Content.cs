using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View
{
	public class Content
	{
		private System.Text.StringBuilder ContentBuilder;
		public string Value {
			get { return this.ContentBuilder.ToString(); }
		}
		public void Clear()
		{
			if (this.ContentBuilder.Length > 0) {
				this.ContentBuilder.Remove(0, this.ContentBuilder.Length);
			}
		}
		public void Replace(string ValueToReplace, string NewValue)
		{
			//If Me.ContentBuilder.ToString().Contains(ValueToReplace) Then
			this.ContentBuilder.Replace(ValueToReplace, NewValue);
			//End If
		}
		public void AddIfNotContains(string Value)
		{
			if (!this.ContentBuilder.ToString().Contains(Value)) {
				this.ContentBuilder.Append(Value);
			}
		}
		public Content Add(string Value)
		{
			this.ContentBuilder.Append(Value);
			return this;
		}
		public Content()
		{
			this.ContentBuilder = new System.Text.StringBuilder();
		}
		public Content(int Capacity)
		{
			this.ContentBuilder = new System.Text.StringBuilder(Capacity);
		}
		public Content(int Capacity, int MaxCapacity)
		{
			this.ContentBuilder = new System.Text.StringBuilder(Capacity, MaxCapacity);
		}
	}
}
