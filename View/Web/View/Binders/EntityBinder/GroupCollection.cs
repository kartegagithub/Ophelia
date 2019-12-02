using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Application.Base;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public class GroupCollection : CollectionBase
	{
		private EntityBinder oEntityBinder = null;
		private string sTitle = "";
		public new Group this[int Index] {
			get {
				if (Index > -1 && Index < this.List.Count) {
					return this.List[Index];
				}
				return null;
			}
			set {
				if (Index > -1 && Index < this.List.Count) {
					this.List[Index] = value;
				}
			}
		}
		public new Group this[string Name] {
			get {
				for (int i = 0; i <= this.List.Count - 1; i++) {
					if (this.List[i] != null && this.List[i].ID == Name) {
						return this.List[i];
					}
				}
				return null;
			}
			set {
				for (int i = 0; i <= this.List.Count - 1; i++) {
					if (this.List[i] != null && this.List[i].ID == Name) {
						this.List[i] = value;
						break; // TODO: might not be correct. Was : Exit For
					}
				}
			}
		}
		public EntityBinder EntityBinder {
			get { return this.oEntityBinder; }
		}
		public Group AddGroup(string ID)
		{
			if (string.IsNullOrEmpty(ID.Trim())) {
				return this.AddGroup(ID, ID);
			} else {
				return this.AddGroup(ID, this.EntityBinder.Client.Dictionary.GetWord("Concept." + ID));
			}
		}
		public Group AddGroup(string ID, string Title)
		{
			Group Group = new Group(this, ID, Title);
			this.List.Add(Group);
			return Group;
		}
		public Group AddGroup(ref Group Group)
		{
			this.List.Add(Group);
			return Group;
		}
		public GroupCollection(EntityBinder EntityBinder)
		{
			this.oEntityBinder = EntityBinder;
		}
	}
}
