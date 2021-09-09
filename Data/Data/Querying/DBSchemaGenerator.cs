using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Data.Querying
{
    public class DBSchemaGenerator
    {
        public bool GenerateDatabaseScript(DataContext context, string assemblyName, string outputFilePath)
        {
            var directory = System.IO.Path.GetDirectoryName(outputFilePath);
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            var assemly = AppDomain.CurrentDomain.GetAssemblies().Where(op => op.GetName().Name.Contains(assemblyName)).FirstOrDefault();
            var types = assemly.GetTypes().Where(op => op.IsSubclassOf(typeof(Ophelia.Data.Model.DataEntity)) && !op.IsAbstract).ToList();
            var designer = new DataDesigner();
            designer.Context = context;
            foreach (var type in types)
            {
                designer.CreateSchema(type);
                designer.CreateTable(type);
            }
            System.IO.File.WriteAllText(outputFilePath, string.Join(";" + Environment.NewLine, designer.SQLList));
            return true;
        }
    }
}
