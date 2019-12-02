using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Ophelia.Web.View.Binders;
using Ophelia.Web.View.Controls;
namespace Ophelia.Web.View.Forms
{
	public abstract class EntityForm : AjaxControl
	{
		private Ophelia.Web.View.Binders.BinderManager oBinderManager = null;
		private int sEntityID = 0;
		private string sEntityType = "";
		private bool bLoadWithAjax = true;
		private string sCloseEvent = "";
		private bool bUseDefaultStyle = true;
		private EntityCollection oCollection = null;
		private bool bHasCloseButton = true;
		//Protected sID As String = ""
		//Public Overloads ReadOnly Property ID() As String
		//    Get
		//        Return Me.sID
		//    End Get
		//End Property
		public bool LoadWithAjax {
			//Sheets will be loaded on click event if it is true.
			get { return this.bLoadWithAjax; }
		}
		public int EntityID {
			get { return this.sEntityID; }
			set { this.sEntityID = value; }
		}
		public string CloseEvent {
			get { return this.sCloseEvent; }
			set { this.sCloseEvent = value; }
		}
		public string CurrentSheet {
			get {
				if (!string.IsNullOrEmpty(this.Page.QueryString("sheetID")) && this.Page.QueryString("sheetID") != "undefined") {
					return this.Page.QueryString("sheetID").Replace("_Sheet", "");
				} else {
					return "";
				}
			}
		}
		public string EntityType {
			get { return this.sEntityType; }
			set { this.sEntityType = value; }
		}
		public Ophelia.Web.View.Binders.BinderManager BinderManager {
			get { return this.oBinderManager; }
		}
		public Client Client {
			get { return this.Page.Client; }
		}
		public override sealed void OnBeforeDraw(Content Content)
		{
			base.OnBeforeDraw(Content);
		}

		public virtual void OnSheetLoaded(BinderGroup LoadedBinderGroup)
		{
		}
		public void LoadSheetContent()
		{
			this.EntityID = this.Page.QueryString(this.ID + "_CurrentEntityID");
			this.EntityType = this.Page.QueryString(this.ID + "_CurrentEntityTypeFullName");
			this.Collection = this.Client.Application.GetCollection(this.Page.QueryString(this.ID + "_CurrentEntityTypeFullName").ToString);
			this.OnBeforeLoad();
			this.SetDefaultParameters();
			for (int i = 0; i <= this.BinderManager.BinderGroups.Count - 1; i++) {
				if (this.BinderManager.BinderGroups(i).ID == this.CurrentSheet) {
					this.Controls.Add(this.BinderManager.BinderGroups(i));
					this.OnSheetLoaded(this.BinderManager.BinderGroups(i));
					this.Page.ScriptManager.FirstScript.AppendLine("$('#" + this.ID + " .sheetContainer').hide(); $('#" + this.ID + " #' + '" + this.BinderManager.BinderGroups(i).ID + "_Sheet').show();");
					break; // TODO: might not be correct. Was : Exit For
				}
			}
		}
		public bool UseDefaultStyle {
			get { return this.bUseDefaultStyle; }
			set { this.bUseDefaultStyle = value; }
		}
		public bool HasCloseButton {
			get { return this.bHasCloseButton; }
			set { this.bHasCloseButton = value; }
		}
		protected virtual void SetDefaultParameters()
		{
			if (this.UseDefaultStyle) {
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets #EntityFormCloseImage", "margin:6px; height: 20px; width:20px; background-image:url(\"" + this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,uiicons222222256240.png$$$Namespace,,Ophelia\"); background-position: -95px -125px;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets #EntityFormPreviousEntityImage", "position:relative; float:left; height: 18px; width:18px;margin:6px; background-image:url(\"" + this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,uiicons222222256240.png$$$Namespace,,Ophelia\"); background-position: -95px -125px;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets #EntityFormNextEntityImage", "position:relative; float:left; height: 18px; width:18px;margin:6px; background-image:url(\"" + this.Page.Client.ApplicationBase + "?DisplayImage=ResourceName,,uiicons222222256240.png$$$Namespace,,Ophelia\"); background-position: -95px -125px;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets ul", "float:left;padding:0;margin:0;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets ul li", "background-color:Gray;border: 1px groove Ivory; list-style: none outside none;text-align: center;padding: 7px; float:right ;color:#CCCCCC; cursor:pointer;-webkit-user-select: none; -moz-user-select: none; -ms-user-select: none;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets ul li.active", "background-color:#CCC;color:Black;");
				this.Page.Header.StyleSheet.AddCustomRule("#EntityFormSheets ul li:hover", "background-color:#CCC;color:Black;");
				this.Page.Header.StyleSheet.AddCustomRule(".sheetContainer", "clear:both;display:none;overflow-y:auto;width:100%;");
			}
		}
		public EntityCollection Collection {
			get { return this.oCollection; }
			set { this.oCollection = value; }
		}
		private string GetPreviousOrNextEntityID(bool IsPrevious)
		{
			if (this.Collection != null) {
				for (int i = 0; i <= this.Collection.Count - 1; i++) {
					if (this.Collection(i).ID == this.EntityID) {
						if (IsPrevious) {
							if (i > 0) {
								return this.Collection(i - 1).ID.ToString();
							} else {
								return "";
							}
						}
						if (i < this.Collection.Count - 1) {
							return this.Collection(i + 1).ID.ToString();
						} else {
							return "";
						}
					}
				}
			}
			return "";
		}
		protected override sealed void OnLoad()
		{
			this.OnBeforeLoad();
			this.SetDefaultParameters();
			this.Page.ScriptManager.FirstScript.AddFunction("ChangeSheet", this.DrawChangeSheetFunction(), "sheetID, loadWithAjax");
			this.AddAjax("LoadSheetContent", this.ID + "_CurrentEntityID," + this.ID + "_CurrentEntityTypeFullName", "sheetID");

			Panel Panel = new Panel("EntityFormSheets");
			Panel.SetStyle(this.Style);
			Panel.Style.Float = FloatType.Left;
			Panel.Style.WidthInPercent = 100;

			HiddenBox SelectedSheetIDHiddenBox = new HiddenBox("SelectedSheetID");
			SelectedSheetIDHiddenBox.Value = this.CurrentSheet;
			Panel.Controls.Add(SelectedSheetIDHiddenBox);

			Panel MenuPanel = new Panel("MenuPanel");
			MenuPanel.Style.WidthInPercent = 100;
			MenuPanel.Style.Float = FloatType.Left;
			MenuPanel.Style.BackgroundColor = "#AAA";

			MenuPanel.Content.Add("<ul>");
			for (int i = 0; i <= this.BinderManager.BinderGroups.Count - 1; i++) {
				this.BinderManager.BinderGroups(i).Style.Class = "sheetContainer";
				MenuPanel.Content.Add("<li");
				MenuPanel.Content.Add(" id=\"" + this.BinderManager.BinderGroups(i).ID + "\" ");
				if ((string.IsNullOrEmpty(this.CurrentSheet) && this.BinderManager.BinderGroups(i).IsDefault) || (!string.IsNullOrEmpty(this.CurrentSheet) && this.BinderManager.BinderGroups(i).ID == this.CurrentSheet)) {
					this.BinderManager.BinderGroups(i).Style.Display = DisplayMethod.Block;
					MenuPanel.Content.Add(" class=\"active\" ");
				}
				MenuPanel.Content.Add(" onclick=\"$('#" + this.ID + " #SelectedSheetID').val('" + this.BinderManager.BinderGroups(i).ID + "_Sheet'); $('#" + this.ID + " #EntityFormSheets ul li').removeClass('active'); $(this).addClass('active'); ChangeSheet('" + this.BinderManager.BinderGroups(i).ID + "_Sheet'," + (this.LoadWithAjax ? "1" : "0") + ");\" >");
				MenuPanel.Content.Add(this.BinderManager.BinderGroups(i).Title);
				MenuPanel.Content.Add("</li>");
			}
			MenuPanel.Content.Add("</ul>");

			Panel EntityFormImageCommandsContainer = new Panel("");
			EntityFormImageCommandsContainer.Style.Float = FloatType.Right;
			EntityFormImageCommandsContainer.Style.Width = 100;

			Panel PreviousEntity = new Panel("EntityFormPreviousEntityImage");
			PreviousEntity.Style.CursorStyle = Cursor.Pointer;
			PreviousEntity.Style.Float = FloatType.Left;
			EntityFormImageCommandsContainer.Controls.Add(PreviousEntity);

			Panel NextEntity = new Panel("EntityFormNextEntityImage");
			NextEntity.Style.CursorStyle = Cursor.Pointer;
			NextEntity.Style.Float = FloatType.Left;
			EntityFormImageCommandsContainer.Controls.Add(NextEntity);

			if (this.Collection != null) {
				if (!string.IsNullOrEmpty(this.GetPreviousOrNextEntityID(true))) {
					PreviousEntity.OnClickEvent = "DrawEntityForm('" + this.EntityType + "','" + this.GetPreviousOrNextEntityID(true) + "',$('#" + this.ID + " #SelectedSheetID').val(),'" + this.Collection.ToString("ID") + "');";
				} else {
					PreviousEntity.Style.Opacity = 0;
					PreviousEntity.Style.CursorStyle = Cursor.Default;
				}
				if (!string.IsNullOrEmpty(this.GetPreviousOrNextEntityID(false))) {
					NextEntity.OnClickEvent = "DrawEntityForm('" + this.EntityType + "','" + this.GetPreviousOrNextEntityID(false) + "',$('#" + this.ID + " #SelectedSheetID').val(),'" + this.Collection.ToString("ID") + "');";
				} else {
					NextEntity.Style.Opacity = 0;
					NextEntity.Style.CursorStyle = Cursor.Default;
				}
			}
			if (this.HasCloseButton) {
				dynamic CloseImage = new Panel("EntityFormCloseImage");
				CloseImage.Style.CursorStyle = Cursor.Pointer;
				CloseImage.OnClickEvent = this.CloseEvent + " $('#" + this.ID + " #EntityFormSheets').fadeOut(); ";
				EntityFormImageCommandsContainer.Controls.Add(CloseImage);
				MenuPanel.Controls.Add(EntityFormImageCommandsContainer);
			}

			Panel.Controls.Add(MenuPanel);
			Panel SheetContentsPanel = new Panel("ContentContainer");
			SheetContentsPanel.Style.Class = "sheetContentsContainer";
			SheetContentsPanel.Style.WidthInPercent = 100;
			SheetContentsPanel.Style.Float = FloatType.Left;
			SheetContentsPanel.Style.Clear = ClearStyle.Both;
			Panel SheetContent = null;
			for (int i = 0; i <= this.BinderManager.BinderGroups.Count - 1; i++) {
				SheetContent = new Panel("");
				//If content will be loaded with Ajax, then only set the main container for content
				if (this.LoadWithAjax && !((string.IsNullOrEmpty(this.CurrentSheet) && this.BinderManager.BinderGroups(i).IsDefault) || (!string.IsNullOrEmpty(this.CurrentSheet) && this.BinderManager.BinderGroups(i).ID == this.CurrentSheet))) {
					SheetContent.ID = this.BinderManager.BinderGroups(i).ID + "_Sheet";
				} else {
					SheetContent.Controls.Add(this.BinderManager.BinderGroups(i));
				}
				SheetContentsPanel.Controls.Add(SheetContent);
			}
			Panel.Controls.Add(SheetContentsPanel);

			HiddenBox EntityIDHiddenBox = new HiddenBox(this.ID + "_CurrentEntityID");
			EntityIDHiddenBox.Value = this.EntityID;
			Panel.Controls.Add(EntityIDHiddenBox);

			HiddenBox EntityTypeFullNameHiddenBox = new HiddenBox(this.ID + "_CurrentEntityTypeFullName");
			EntityTypeFullNameHiddenBox.Value = this.EntityType;
			Panel.Controls.Add(EntityTypeFullNameHiddenBox);

			Panel EntityForm = new Panel(this.ID);
			EntityForm.Attributes.Add("entityID", this.EntityID.ToString());
			EntityForm.Controls.Add(Panel);
			this.Controls.Add(EntityForm);
		}
		public override void CustomizeAjaxFunctionProperties(Controls.ServerSide.ScriptManager.AjaxFunction AjaxFunction)
		{
			base.CustomizeAjaxFunctionProperties(AjaxFunction);
			AjaxFunction.OverlayStyle.PositionStyle = Ophelia.Web.View.Position.Absolute;
			AjaxFunction.OverlayStyle.WidthInPercent = 100;
			AjaxFunction.OverlayStyle.HeightInPercent = 100;
			AjaxFunction.OverlayStyle.BackgroundColor = "gray";
			AjaxFunction.OverlayStyle.Opacity = 0.34;
			AjaxFunction.OverlayStyle.ZIndex = 999;
			AjaxFunction.OverlayStyle.Filter = "alpha(opacity=34)";
			AjaxFunction.OverlayElementID = "ContentContainer";
		}
		private string DrawChangeSheetFunction()
		{
			System.Text.StringBuilder returnString = new System.Text.StringBuilder();
			returnString.AppendLine("if ($('#" + this.ID + " #' + sheetID) != null && $('#" + this.ID + " #' + sheetID).length > 0) { ");
			returnString.AppendLine("  if (loadWithAjax == 1 && $('#" + this.ID + " #' + sheetID).html().trim() == '') { ");
			returnString.AppendLine("      LoadSheetContent(sheetID); }else{ ");
			returnString.AppendLine(" $('#" + this.ID + " .sheetContainer').hide(); $('#" + this.ID + " #' + sheetID).show();}");
			returnString.AppendLine(" } ");
			return returnString.ToString();
		}
		private void Configure()
		{
			this.oBinderManager = new BinderManager(this);
		}

		protected virtual void OnBeforeLoad()
		{
		}
		public EntityForm()
		{
			this.Configure();
		}
	}
}
