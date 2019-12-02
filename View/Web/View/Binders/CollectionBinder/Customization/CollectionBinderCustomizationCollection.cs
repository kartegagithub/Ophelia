using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderCustomizationCollection : EntityCollection
	{
		public new CollectionBinderCustomization this[int Index] {
			get { return base.Item(Index); }
			set { base.Item(Index) = value; }
		}
		protected override void UpdateEntityDefinition(ref Application.Base.EntityDefinition Definition)
		{
			Definition.IsRelationClass();
		}
	}
}
