using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders
{
	public class CollectionBinderCustomization : Entity
	{
		[Attributes.Data(500, true)]
		public string BinderUniqueName {
			get { return this.Data.GetPropertyValue("BinderUniqueName", ""); }
			set { this.Data.SetPropertyValue("BinderUniqueName", value); }
		}
		public byte IsDefaultCustomization {
			get { return this.Data.GetPropertyValue("IsDefaultCustomization", 1); }
			set { this.Data.SetPropertyValue("IsDefaultCustomization", value); }
		}
		public CollectionBinderColumnCustomizationCollection ColumnCustomizations {
			get { return this.Data.GetPropertyValueSorted("ColumnCustomizations", "Indis", SortDirection.Ascending); }
		}
		public Ophelia.Application.Framework.User User {
			get { return this.Data.GetPropertyValue("User"); }
			set { this.Data.SetPropertyValue("User", value); }
		}
		//Public ReadOnly Property ColumnCustomization(ByVal MemberName As String) As CollectionBinderColumnCustomization
		//    Get
		//        Dim n As Integer
		//        For n = 0 To Me.ColumnCustomizations.Count - 1
		//            If Me.ColumnCustomizations(n).MemberName = MemberName Then
		//                Return Me.ColumnCustomizations(n)
		//            End If
		//        Next
		//        Return Nothing
		//    End Get
		//End Property
	}
}
