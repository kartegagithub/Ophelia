using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ophelia.Xml
{
    public class DynamicXmlParser : DynamicObject
    {
        private readonly XElement element;

        public DynamicXmlParser(string text): this(XElement.Parse(text)) { }

        private DynamicXmlParser(XElement element)
        {
            Guard.ArgumentNullException(element, "element");
            this.element = element;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            XElement sub = element.Element(binder.Name);
            var success = sub != null;
            result = success ? new DynamicXmlParser(sub) : null;
            return success;
        }

        public override string ToString()
        {
            return element.Value;
        }

        public string this[string attr]
        {
            get { return element.Attribute(attr).Value;  }
        }
    }
}
