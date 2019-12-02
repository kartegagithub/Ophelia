using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Ophelia.Web.View.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int nMaxSize;

        public FileSizeAttribute(int maxSize)
        {
            this.nMaxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            return (value as HttpPostedFileBase).ContentLength <= nMaxSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Dosya boyutu {0} değerinden fazla olamaz.", this.nMaxSize);
        }
    }
}
