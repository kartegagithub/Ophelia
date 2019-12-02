﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ophelia.Web.Service
{
    [DataContract(IsReference = true)]
    public class ServiceCollectionResult<TEntity> : ServiceCollectionResult
    {
        [DataMember]
        public List<TEntity> Data { get; set; }

        public void SetData(int totalCount, List<TEntity> list)
        {
            this.SetData(Convert.ToInt64(totalCount), list);
        }

        public void SetData(long totalCount, List<TEntity> list)
        {
            this.TotalDataCount = totalCount;
            this.Data = list;
            if (!this.HasFailed)
                this.HasFailed = false;
        }

        public void SetData(List<TEntity> list)
        {
            this.Data = list;
            if (list != null)
                this.TotalDataCount = list.Count;
            if (!this.HasFailed)
                this.HasFailed = false;
        }
        [DataMember]
        public bool HasData
        {
            get
            {
                if (this.Data != null && this.Data.Count > 0)
                    return true;
                return false;
            }
            set
            {

            }
        }
    }
}
