using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Base.DataGrid;
namespace Ophelia.Web.View.Binders
{
	public class CheckListBinder : Binders.Binder
	{
		private CollectionBinder oCollectionBinder;
		private EntityCollection oBaseCollection;
		public event RowAddedEventHandler RowAdded;
		public delegate void RowAddedEventHandler(object Sender, ref Ophelia.Web.View.Base.DataGrid.RowEventArgs e);
		public Client Client {
			get { return this.Page.Client; }
		}
		public EntityCollection Collection {
			get { return this.Binding.Value; }
		}
		public CollectionBinder CollectionBinder {
			get { return this.oCollectionBinder; }
		}
		public EntityCollection BaseCollection {
			get { return this.oBaseCollection; }
			set {
				this.oBaseCollection = value;
				this.CollectionBinder.BindState = BinderState.Pending;
				this.CollectionBinder.Binding.Value = value;
				this.CollectionBinder.HierarchicDisplay = value.IsHierarchical;
			}
		}
		public string Title {
			get { return this.CollectionBinder.Header.Title; }
			set { this.CollectionBinder.Header.Title = value; }
		}
		public void Bind(EntityCollection Collection)
		{
			this.BindState = BinderState.Pending;
			this.Binding.UpdateValue(Collection);
			this.Bind();
		}
		public virtual void OnRowAdded(Ophelia.Web.View.Base.DataGrid.RowEventArgs e)
		{
			if (RowAdded != null) {
				RowAdded(this, e);
			}
		}
		public override void Bind()
		{
			BindingEventArgs EventArgs = new BindingEventArgs(this.Binding);
			this.OnBindingStarted(EventArgs);
			this.BindState = BinderState.Binding;
			//Dim ExceptionList As New Ophelia.Web.View.Base.DataGrid.AnyRowCollection(Me.CollectionBinder)
			//Dim ConditionRow As Row = Nothing
			if ((this.Collection != null) && (this.BaseCollection != null)) {
				int Index = 0;
				int i = 0;
				Row Row = default(Row);
				if (this.CollectionBinder.BindState != BinderState.Binded)
					this.CollectionBinder.Bind();
				for (i = 0; i <= this.BaseCollection.Count - 1; i++) {
					//ConditionRow = Nothing
					Row = this.CollectionBinder.Rows(Index);
					//If Not ExceptionList.Contains(Row) Then Row.IsSelected = False
					int n = 0;
					for (n = 0; n <= this.Collection.Count - 1; n++) {
						bool IsChecked = false;
						//If Me.CreatesSubLevelObjects = 1 Then
						IsChecked = this.Collection(n).ID == this.BaseCollection(Index).ID;
						//Else
						//    If Me.SubLevelPropertyTypeName = "" Then
						//        IsChecked = Me.Collection(n).ID = Me.BaseCollection(Index).ID
						//    Else
						//        IsChecked = Me.Collection(n).GetType.GetProperty(Me.SubLevelPropertyTypeName).GetValue(Me.Collection(n), Nothing).ID = Me.BaseCollection(Index).ID
						//    End If
						//End If
						Row.IsSelected = IsChecked;
						if (IsChecked)
							break; // TODO: might not be correct. Was : Exit For
						//If IsChecked Then
						//    ExceptionList.AddRange(Row.SubRows)
						//    Dim BaseRows As New ArrayList
						//    Dim ParentRow As Row = Row.ParentRow
						//    Dim k As Integer
						//    Do Until ParentRow Is Nothing
						//        BaseRows.Add(ParentRow)
						//        ParentRow = ParentRow.ParentRow
						//    Loop
						//    For k = 1 To BaseRows.Count
						//        ParentRow = CType(BaseRows(BaseRows.Count - k), Row)
						//        ParentRow.Expand()
						//    Next
						//    Row.Checked = True
						//    Row.Expand()
						//    'ConditionRow = Row
						//End If
					}
					this.OnRowAdded(new RowEventArgs(Row));
					//If Not ConditionRow Is Nothing AndAlso Me.CollectionBinder.SelectionPattern = Base.TreeViewSelectionPatern.TopOfPath AndAlso Me.CollectionBinder.HierarchicDisplay Then Index += ConditionRow.SubRows.Count
					Index += 1;
					if (Index >= this.CollectionBinder.Rows.Count)
						break; // TODO: might not be correct. Was : Exit For
				}
			}
			this.BindState = BinderState.Binded;
			this.OnBindingCompleted(EventArgs);
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Bind();
			this.ConfigureSubControls(CollectionBinder);
			Content.Add(this.CollectionBinder.Draw());
		}
		private void CreateCollectionBinder()
		{
			this.oCollectionBinder = new CollectionBinder(this.ID);
			this.oCollectionBinder.CheckBoxes = true;
			this.oCollectionBinder.Configuration.AllowDelete = false;
			this.oCollectionBinder.Configuration.AllowNew = false;
			this.CollectionBinder.Columns.AddTextBoxColumn("Name");
		}
		public CheckListBinder(string ID)
		{
			this.ID = ID;
			this.CreateCollectionBinder();
		}
		public CheckListBinder(string ID, string Title) : this(ID)
		{
			this.Title = Title;
		}
		public CheckListBinder(string ID, string Title, string MemberName) : this(ID, Title)
		{
			this.Binding.MemberName = MemberName;
		}
		public CheckListBinder(string ID, Entity Entity, string MemberName) : this(ID)
		{
			this.Binding.Item = Entity;
			this.Binding.MemberName = MemberName;
		}
		public CheckListBinder(string ID, Entity Entity, string Title, string MemberName) : this(ID, Title)
		{
			this.Binding.Item = Entity;
			this.Binding.MemberName = MemberName;
		}
		public CheckListBinder(string ID, string Title, EntityCollection BaseCollection) : this(ID, Title)
		{
			this.BaseCollection = BaseCollection;
		}
		public CheckListBinder(string ID, EntityCollection BaseCollection) : this(ID)
		{
			this.BaseCollection = BaseCollection;
		}
	}
}
