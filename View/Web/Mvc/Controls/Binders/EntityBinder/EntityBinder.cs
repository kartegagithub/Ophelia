﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ophelia.Web.UI.Controls;
using Ophelia.Reflection;
using Ophelia.Web.Service;
using System.Web;
using System.Linq.Expressions;

namespace Ophelia.Web.View.Mvc.Controls.Binders.EntityBinder
{
    public class EntityBinder<T> : Panel, IDisposable where T : class
    {
        private UrlHelper _Url;
        private readonly ViewContext viewContext;
        private Boolean isDisposed;

        public UrlHelper Url
        {
            get
            {
                if (this._Url == null)
                {
                    if (this.viewContext == null)
                        throw new Exception("Please override onViewContextSet function. ViewContext is not ready yet.");
                    this._Url = new UrlHelper(this.viewContext.RequestContext);
                }
                return this._Url;
            }
        }
        public HttpRequest Request { get { return this.Client.Request; } }
        public T Entity { get; private set; }
        public long EntityID { get; private set; }
        public Controllers.Base.Controller Controller { get; set; }
        public Client Client { get; private set; }
        public Panel Content { get; set; }
        public Form Form { get; set; }
        public string Title { get; set; }
        public TabControl<T> TabControl { get; protected set; }
        public List Breadcrumb { get; private set; }
        public List ActionButtons { get; private set; }
        public Configuration Configuration { get; private set; }
        public List<ServiceResultMessage> Messages { get; set; }
        public Dictionary<string, List<Expression<Func<T, object>>>> ModeFields { get; set; }
        public Dictionary<string, List<string>> ModeTextFields { get; set; }
        public virtual bool IsAjaxEntityBinderRequest
        {
            get
            {
                return this.Request["ajaxentitybinder"] == "1";
            }
        }
        public bool Has_i18n
        {
            get
            {
                return this.Entity.GetType().GetProperty("I18n") != null || this.Entity.GetType().GetProperty(this.Entity.GetType().Name + "_i18n") != null;
            }
        }
        public virtual string[] DefaultEntityProperties
        {
            get
            {
                return null;
            }
        }
        public virtual int CurrentLanguageID
        {
            get
            {
                return this.Client.CurrentLanguageID;
            }
        }

        public EntityBinder(Client client, T entity, string title)
        {
            this.Entity = entity;
            this.ModeFields = new Dictionary<string, List<Expression<Func<T, object>>>>();
            this.ModeTextFields = new Dictionary<string, List<string>>();
            this.EntityID = Convert.ToInt64(entity.GetPropertyValue("ID"));
            this.Client = client;
            this.Configuration = new Configuration();
            this.CreateTabControl();
            this.Content = new Panel();
            this.Form = new Form();
            this.Breadcrumb = new List();
            this.Breadcrumb.CssClass = "breadcrumb";
            this.ActionButtons = new List();
            this.ActionButtons.CssClass = "breadcrumb-elements";

            this.Controls.Add(this.Breadcrumb);
            this.Controls.Add(this.ActionButtons);
            this.Controls.Add(this.Content);
            this.Content.Controls.Add(this.Form);
            this.Form.Controls.Add(this.TabControl);

            this.Form.Attributes.Add("method", "post");
            this.Form.Attributes.Add("enctype", "multipart/form-data");
            this.Form.CssClass = "form-horizontal";

            this.Content.CssClass = "binder-body panel-body";
            this.CssClass = "entity-binder panel panel-flat";

            this.ID = title;
            this.Title = this.Client.TranslateText(title);
            this.OnBeforeConfigure();
            this.Configure();
            this.OnAfterConfigure();
        }

        public EntityBinder(ViewContext viewContext, T entity, string title) : this(((Controllers.Base.Controller)viewContext.Controller).Client, entity, title)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");

            this.viewContext = viewContext;
            if (viewContext.ViewBag.Messages != null)
            {
                this.Messages = viewContext.ViewBag.Messages;
            }
            this.Output = this.viewContext.Writer;
            this.Controller = (Controllers.Base.Controller)this.viewContext.Controller;
            this.onViewContextSet();
        }
        public EntityBinder<T> DiscardDocumentation(Expression<Func<T, object>> expression)
        {
            return this.DiscardDocumentation(expression.Body.ParsePath());
        }
        public EntityBinder<T> DiscardDocumentation(string Key)
        {
            if (!this.Configuration.Help.DiscardedDocumentation.Contains(Key))
                this.Configuration.Help.DiscardedDocumentation.Add(Key);

            return this;
        }
        public EntityBinder<T> AddSearchHelp(Expression<Func<T, object>> expression, string URL, string Callback = "")
        {
            return this.AddSearchHelp(expression.Body.ParsePath(), URL, Callback);
        }
        public EntityBinder<T> AddSearchHelp(string Key, string URL, string Callback = "")
        {
            if (!this.Configuration.Help.SearchHelps.Where(op => op.Path == Key).Any())
                this.Configuration.Help.SearchHelps.Add(new SearchHelp() { Path = Key, URL = URL, Callback = Callback });
            
            return this;
        }
        public EntityBinder<T> AddDocumentation(Expression<Func<T, object>> expression, string HelpTip)
        {
            return this.AddDocumentation(expression.Body.ParsePath(), HelpTip);
        }
        public EntityBinder<T> AddDocumentation(string Key, string HelpTip)
        {
            if (!this.Configuration.Help.Documentation.ContainsKey(Key))
                this.Configuration.Help.Documentation.Add(Key, HelpTip);
            else
                this.Configuration.Help.Documentation[Key] = HelpTip;

            return this;
        }
        public EntityBinder<T> RemoveDocumentation(Expression<Func<T, object>> expression)
        {
            return this.RemoveDocumentation(expression.Body.ParsePath());
        }
        public EntityBinder<T> RemoveDocumentation(string Key)
        {
            if (this.Configuration.Help.Documentation.ContainsKey(Key))
                this.Configuration.Help.Documentation.Remove(Key);

            return this;
        }
        public EntityBinder<T> AddFieldToMode(string Mode, string key)
        {
            if (!this.ModeTextFields.ContainsKey(Mode))
                this.ModeTextFields.Add(Mode, new List<string>());

            if (key != null)
                this.ModeTextFields[Mode].Add(key);
            return this;
        }
        public EntityBinder<T> AddFieldsToMode(string Mode, params string[] keys)
        {
            if (keys != null)
            {
                foreach (var item in keys)
                {
                    this.AddFieldToMode(Mode, item);
                }
            }
            return this;
        }
        public EntityBinder<T> AddFieldToMode(string Mode, Expression<Func<T, object>> expression)
        {
            if (!this.ModeFields.ContainsKey(Mode))
                this.ModeFields.Add(Mode, new List<Expression<Func<T, object>>>());

            if (expression != null)
                this.ModeFields[Mode].Add(expression);
            return this;
        }
        public EntityBinder<T> AddFieldsToMode(string Mode, params Expression<Func<T, object>>[] expressions)
        {
            if (expressions != null)
            {
                foreach (var item in expressions)
                {
                    this.AddFieldToMode(Mode, item);
                }
            }
            return this;
        }
        protected virtual void Configure()
        {

        }
        protected virtual void OnBeforeConfigure()
        {

        }
        protected virtual void OnAfterConfigure()
        {

        }
        protected virtual void onViewContextSet()
        {

        }

        public MvcHtmlString Render()
        {
            return new MvcHtmlString(base.Draw());
        }
        protected virtual void CreateTabControl()
        {
            this.TabControl = new TabControl<T>(this);
        }

        public virtual void RenderHeader()
        {
            this.RenderBreadcrumb();
            this.Output.Write("<div class=\"content\">");
            this.Output.Write("<form class=\"form-horizontal\">");
            this.TabControl.RenderHeader();
        }

        public virtual void RenderFooter()
        {
            this.TabControl.RenderFooter();

            this.Output.Write("</form>");
            this.Output.Write("</div>"); /* content */
        }
        public virtual void RenderBreadcrumb()
        {
            this.Output.Write("<div class=\"page-header\">");
            this.Output.Write("<div class=\"breadcrumb-line breadcrumb-line-component\">");
            this.Output.Write(this.Breadcrumb.Draw());
            this.Output.Write(this.ActionButtons.Draw());
            this.Output.Write("</div>");
            this.Output.Write("</div>");
        }
        public override void Dispose()
        {
            this.Dispose(true);

            this.TabControl.Dispose();
            this.TabControl = null;

            GC.SuppressFinalize(this);
        }
        protected long GetPreviousEntityID()
        {
            if (!string.IsNullOrEmpty(this.Request["IDList"]))
            {
                var IDs = this.Request["IDList"].ToLongList();
                for (int i = 0; i < IDs.Count; i++)
                {
                    if (IDs[i] == this.EntityID)
                    {
                        if (i == 0)
                            return 0;
                        else
                            return IDs[i - 1];
                    }
                }
            }
            return 0;
        }
        protected long GetNextEntityID()
        {
            if (!string.IsNullOrEmpty(this.Request["IDList"]))
            {
                var IDs = this.Request["IDList"].ToLongList();
                for (int i = 0; i < IDs.Count; i++)
                {
                    if (IDs[i] == this.EntityID)
                    {
                        if (i + 1 < IDs.Count)
                            return IDs[i + 1];
                        else
                            return 0;
                    }
                }
            }
            return 0;
        }
        public virtual bool CanDrawField(Fields.BaseField<T> field)
        {
            if (this.Configuration.ReadOnly)
                field.Editable = false;
            return true;
        }

        public virtual bool CanDrawTab(Tab<T> tab)
        {
            return true;
        }

        public virtual void Dispose(Boolean disposing)
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
            }
        }
    }
}
