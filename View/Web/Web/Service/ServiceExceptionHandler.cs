﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Web.Service
{
    public abstract class ServiceExceptionHandler
    {
        public abstract void HandleException(Exception ex);
    }
}
