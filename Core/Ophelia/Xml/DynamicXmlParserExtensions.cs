using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Xml
{
    public static class DynamicXmlParserExtensions
    {
        public static dynamic XmlToDynamic(this string source)
        {
            return new DynamicXmlParser(source);
        }
    }
}
