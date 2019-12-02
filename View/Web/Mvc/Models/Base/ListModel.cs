﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.View.Mvc.Models
{
    public abstract class ListModel : BaseModel, IPageable, ISortable
    {
        public PaginationModel Pagination { get; set; }
        public PaginationModel GroupPagination { get; set; }
        public CollocationModel Collocation { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            this.Pagination = null;
            this.GroupPagination = null;
            this.Collocation = null;
        }

        public ListModel()
        {
            this.Pagination = new PaginationModel();
            this.GroupPagination = new PaginationModel() { PageKey = "groupPage" };
            this.Collocation = new CollocationModel();
        }
    }
}
