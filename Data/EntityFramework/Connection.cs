using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ophelia.Data.EntityFramework
{
    public static class Connection
    {
        public static TransactionScope BeginTransaction()
        {
            return new TransactionScope(TransactionScopeOption.Required, 
                                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.DefaultTimeout });
        }
    }
}
