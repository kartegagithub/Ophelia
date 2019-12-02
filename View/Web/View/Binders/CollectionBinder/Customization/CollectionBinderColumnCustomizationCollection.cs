using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderColumnCustomizationCollection : EntityCollection
	{
		public new CollectionBinderColumnCustomization this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		public new CollectionBinderColumnCustomization this[string MemberName] {
			get {
				int i = 0;
				for (i = 0; i <= this.Count - 1; i++) {
					if (this[i].MemberName == MemberName)
						return this[i];
				}
				return null;
			}
		}
		public Application.Base.Entity Add(string MemberName, int Indis, int Visible, CollectionBinderCustomization Customization)
		{
			CollectionBinderColumnCustomization ColumnCustomization = base.Add();
			ColumnCustomization.MemberName = MemberName;
			ColumnCustomization.Visible = Visible;
			ColumnCustomization.Indis = Indis;
			ColumnCustomization.CollectionBinderCustomization = Customization;
			ColumnCustomization.Save();
			return ColumnCustomization;
		}
		protected override void UpdateEntityDefinition(ref Application.Base.EntityDefinition Definition)
		{
			Definition.IsRelationClass();
		}
	}
}
