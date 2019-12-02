using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderColumnCustomization : Entity
	{
		[Attributes.Data(500, true)]
		public string MemberName {
			get { return this.Data.GetPropertyValue("MemberName", ""); }
			set { this.Data.SetPropertyValue("MemberName", value); }
		}
		public CollectionBinderCustomization CollectionBinderCustomization {
			get { return this.Data.GetPropertyValue("CollectionBinderCustomization"); }
			set { this.Data.SetPropertyValue("CollectionBinderCustomization", value); }
		}
		public int Indis {
			get { return this.Data.GetPropertyValue("Indis", -1); }
			set { this.Data.SetPropertyValue("Indis", value); }
		}
		public int Visible {
			get { return this.Data.GetPropertyValue("Visible", 0); }
			set { this.Data.SetPropertyValue("Visible", value); }
		}
	}
}
