using Ophelia.Web.View.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ophelia.Web.View.Mvc.Models
{
    public abstract class BaseModel : ISensible, IAuditable, IDisposable
    {
        public BaseModel()
        {
            this.StatusID = "";
            this.ID = 0;
            this.Breadcrumbs = new List<BreadcrumbsItemModel>();
            this.AlertMessage = new AlertMessageModel();
            this.Context = this.CreateSharedContext();
        }

        [Display(Name = "ID")]
        public long ID { get; set; }

        public string StatusID { get; set; }

        public bool IsNew { get { return this.ID == 0; } }
        public AuditModel History { get; set; }
        public IList<BreadcrumbsItemModel> Breadcrumbs { get; set; }
        public AlertMessageModel AlertMessage { get; set; }
        public SharedContext Context { get; set; }

        protected virtual SharedContext CreateSharedContext()
        {
            return new SharedContext();
        }
        public void CreateHistory(long? userCreatedId, DateTime dateCreated, long? userModifiedId, DateTime dateModified)
        {
            this.History = new AuditModel
            {
                UserCreatedID = userCreatedId,
                DateCreated = dateCreated,
                UserModifiedID = userModifiedId,
                DateModified = dateModified
            };
        }

        public virtual void Dispose()
        {
            
        }
    }
}
