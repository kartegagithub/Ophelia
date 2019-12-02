using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace Ophelia.Web.View.Binders.EntityBinder
{
	public abstract class EntityBinder : Base.Binders.Binder, IEntityBinder
	{
		private Ophelia.Web.View.Controls.Form.Form oFieldsForm = null;
		private GroupCollection oGroups = null;
		private EntityBinderFieldCollection oFields = null;
		private Menu oMenu = null;
		private Group oBaseGroup = null;
		private int iGroupColumnCount = 2;
		private int iEntityID = 0;
		private EntityBinderAjaxDelegate oAjaxDelegate;
		public int EntityID {
			get { return this.iEntityID; }
		}
		public virtual EntityBinderAjaxDelegate AjaxDelegate {
			get {
				if (this.oAjaxDelegate == null) {
					this.oAjaxDelegate = new EntityBinderAjaxDelegate();
					this.oAjaxDelegate.Binder = this;
				}
				return this.oAjaxDelegate;
			}
		}
		internal Ophelia.Web.View.Controls.Form.Form FieldsForm {
			get { return this.oFieldsForm; }
		}
		public GroupCollection Groups {
			get {
				if (oGroups == null)
					oGroups = new GroupCollection(this);
				return this.oGroups;
			}
		}
		public Entity Entity {
			get { return this.Binding.Item; }
		}
		public Client Client {
			get { return this.Page.Client; }
		}
		public Application.Framework.Dictionary Dictionary {
			get { return this.Client.Dictionary; }
		}
		public Application.Base.EntityCollection Collection {
			get { return this.Binding.Value; }
		}
		public EntityBinderFieldCollection Fields {
			//To collect all fields on EntityBinder.
			get { return this.oFields; }
		}
		public override void Bind()
		{
			if (this.BindState == BinderState.Pending) {
				this.BindState = BinderState.Binding;
				this.OnBindingStarted(new BindingEventArgs(this.Binding));
				for (int i = 0; i <= this.Fields.Count - 1; i++) {
					this.Fields(i).ControlField.Binding.Item = this.Entity;
					this.Fields(i).ControlField.Bind();
				}
				this.BindState = BinderState.Binded;
				this.OnBindingCompleted(new BindingEventArgs(this.Binding));
			}
		}
		internal override void OnBindingCompleted(BindingEventArgs e)
		{
			base.OnBindingCompleted(e);
		}
		protected virtual void CreateFields()
		{
			//Override this method to add fields of entity binder
		}

		protected virtual void SetEntity()
		{
		}
		public Group BaseGroup {
			get { return this.oBaseGroup; }
		}
		public override void OnBeforeDraw(Content Content)
		{
			this.Bind();
			Ophelia.Web.View.Controls.Structure.Structure EntityBinderStructure = new Ophelia.Web.View.Controls.Structure.Structure("EntityBinderStructure", 2, 2);
			this.Page.StyleSheet.AddIDBasedRule("EntityBinderStructure div", "white-space:normal");
			EntityBinderStructure(0, 0).Style.Class = "EntityBinderBaseGroup";
			//EntityBinderStructure(0, 0).ColumnSpan = 2
			EntityBinderStructure(0, 0).Style.BackgroundColor = "#F5F5F5";
			EntityBinderStructure(0, 0).Style.PaddingLeft = 10;
			EntityBinderStructure(0, 0).Style.Borders.Bottom.SetInput("#CCC", 1, Forms.BorderStyle.Solid);
			EntityBinderStructure(1, 0).Style.VerticalAlignment = VerticalAlignment.Top;
			EntityBinderStructure(1, 0).ColumnSpan = 2;
			EntityBinderStructure.Style.WidthInPercent = 100;

			if (this.BaseGroup != null) {
				for (int i = 0; i <= this.BaseGroup.Fields.Count - 1; i++) {
					EntityBinderStructure(0, 0).Controls.Add(this.BaseGroup.Fields.Form);
				}
			}

			Ophelia.Web.View.Controls.Form.Form EntityProperties = new Ophelia.Web.View.Controls.Form.Form("");
			EntityProperties.Style.Float = FloatType.Right;
			EntityProperties.FieldCellStyle.Font.Color = "#3D5A66";
			EntityProperties.FieldCellStyle.Font.Weight = Forms.FontWeight.Bold;
			EntityProperties.Fields.AddLabel(this.Client.Dictionary.GetWord("Concept.DateCreated") + ":", "DateCreated", this.Entity.DateCreated);
			EntityProperties.Fields.LastField.Header.Style.Float = FloatType.Left;
			EntityProperties.Fields.AddLabel(this.Client.Dictionary.GetWord("Concept.DateModified") + ":", "DateModified", this.Entity.DateModified);
			EntityBinderStructure(0, 1).Controls.Add(EntityProperties);
			EntityBinderStructure(0, 1).Style.Class = "EntityBinderBaseGroup";
			EntityBinderStructure(0, 1).Style.HorizontalAlignment = HorizontalAlignment.Right;
			EntityBinderStructure(0, 1).Style.PaddingRight = 10;
			EntityBinderStructure(0, 1).Style.BackgroundColor = "#F5F5F5";
			EntityBinderStructure(0, 1).Style.Borders.Bottom.SetInput("#CCC", 1, Forms.BorderStyle.Solid);

			EntityBinderStructure(1, 0).Controls.Add(this.Menu);
			//Dim ScriptTriggerImage As New Ophelia.Web.View.Controls.Image("scriptTriggerImage", Me.Page.Client.ApplicationBase & "?DisplayImage=ResourceName,,spacer.gif$$$Namespace,,Ophelia")
			//ScriptTriggerImage.Style.Display = DisplayMethod.Hidden
			//ScriptTriggerImage.Attributes.Add("onload", "SetSubElementsWidthWithMax('" & Me.Menu.ID & "_Container')")
			//Content.Add(ScriptTriggerImage.Draw())
			Content.Add(EntityBinderStructure.Draw());
			Content.Add(this.AjaxDelegate.Draw());
		}
		public Menu Menu {
			get { return this.oMenu; }
		}
		//Private Sub DrawSetSubElementsWidthWithMaxFunction()
		//Dim returnString As New System.Text.StringBuilder
		//returnString.AppendLine("function SetSubElementsWidthWithMax(parentControl){")
		//returnString.AppendLine("    $('#' + parentControl + ' .MenuItemGroup').each(function() {")
		//returnString.AppendLine("       var maxWidth = 0;")
		//returnString.AppendLine("       $(this).each(function() {")
		//returnString.AppendLine("       if ($(this).width() > maxWidth) {")
		//returnString.AppendLine("           maxWidth = $(this).width();")
		//returnString.AppendLine("        }")
		//returnString.AppendLine("        });")
		//returnString.AppendLine("    $('.MenuItemGroup td').width(maxWidth+15);")
		//returnString.AppendLine("    }); ")
		//returnString.AppendLine(" $('.MenuItemGroup div').css('white-space','nowrap');")
		//returnString.AppendLine("}")
		//Me.Page.ScriptManager.FirstScript.AppendLine(returnString.ToString())
		//End Sub
		public int GroupColumnCount {
			get { return this.iGroupColumnCount; }
			set { this.iGroupColumnCount = value; }
		}
		public EntityBinder(string ID, int EntityID)
		{
			this.ID = ID;
			this.iEntityID = EntityID;
			this.oFieldsForm = new Ophelia.Web.View.Controls.Form.Form(this.ID + "_FieldsForm", "", Controls.Form.Form.FormMethod.Post, Ophelia.Web.View.Controls.Form.Form.FormEncodeType.MultipartFormData, Ophelia.Web.View.Controls.Form.Form.FormMethod.Post, Ophelia.Web.View.Controls.Form.Form.FormEncodeType.MultipartFormData);
			this.oFields = new EntityBinderFieldCollection(this);
			this.oMenu = new Menu(this);
			this.oBaseGroup = this.Groups.AddGroup("BaseGroup");
			this.SetEntity();
			this.CreateFields();
			//Me.DrawSetSubElementsWidthWithMaxFunction()
		}
	}
}
