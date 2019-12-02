using System;
using System.Data.Entity;
using System.Data.Common;
using System.Data;

namespace Ophelia.Data.EntityFramework
{
    public static class DbContextExtensions
    {
        public static bool EnableLazyLoad(this DbContext context)
        {
            bool oldValue = context.Configuration.LazyLoadingEnabled;
            context.Configuration.LazyLoadingEnabled = true;
            return oldValue;
        }

        public static void ResetLazyLoad(this DbContext context, bool oldValue)
        {
            context.Configuration.LazyLoadingEnabled = oldValue;
        }

        public static bool CheckConnection(this DbContext context)
        {
            bool result = false;
            try
            {
                context.Database.Connection.Open();
                result = context.Database.Connection.State == System.Data.ConnectionState.Open;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public static DbCommand CreateCommand(this DbContext context, string commandText)
        {
            return CreateCommand(context, commandText, CommandType.Text);
        }

        public static DbCommand CreateCommand(this DbContext context, string commandText, params object[] parameters)
        {
            return CreateCommand(context, commandText, CommandType.Text, parameters);
        }

        public static DbCommand CreateCommand(this DbContext context, string commandText, CommandType commandType, params object[] parameters)
        {
            Guard.ArgumentNullException(context, "context");

            DbConnection connection = context.Database.Connection;
            DbCommand storeCommand = connection.CreateCommand();

            storeCommand.CommandText = commandText;
            storeCommand.CommandType = commandType;
            if (parameters != null && parameters.Length > 0)
                storeCommand.Parameters.AddRange(parameters);

            return storeCommand;
        }
    }
}