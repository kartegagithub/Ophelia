using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Ophelia.Web.View.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileTypesAttribute : ValidationAttribute
    {
        private readonly List<string> typeList;

        public FileTypesAttribute(string types)
        {
            this.typeList = types.Split(',').ToList();
        }

        public override bool IsValid(object value)
        {
            if (value == null || this.typeList == null || this.typeList.Count == 0) return true;

            var fileExtension = System.IO.Path.GetExtension((value as HttpPostedFileBase).FileName).Substring(1);
            return this.typeList.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Geçersiz dosya uzantısı! Sadece '{0}' tipindeki dosyalar desteklenmektedir.", String.Join(", ", this.typeList));
        }
    }
}
