using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Controls
{
	public class InfinityNumberBox : NumberBox
	{
		public override void OnBeforeDraw(Content Content)
		{
			if (!this.Style.Class.Contains("InfinityNumberBoxClass")) {
				this.Style.Class += "InfinityNumberBoxClass";
			}
			base.OnBeforeDraw(Content);
		}
		public override string Value {
			get {
				if (base.Value.ToString == -1) {
					return "*";
				}
				return base.Value;
			}
			set { base.Value = value; }
		}
		public InfinityNumberBox(string MemberName, string Message = "Sayısal değer giriniz.") : base(MemberName)
		{
			this.Validators.Clear();
			this.Validators.AddNumericValidator(Message);
		}
		public InfinityNumberBox(string MemberName, int Value) : this(MemberName)
		{
			this.Value = Value;
		}
	}
}
