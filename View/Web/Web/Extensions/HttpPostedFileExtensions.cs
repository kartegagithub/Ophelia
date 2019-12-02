using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.Extensions
{
    public static class HttpPostedFileExtensions
    {
        public static byte[] ToArray(this HttpPostedFileBase file)
        {
            byte[] fileData = null;
            if(file != null)
            {
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
            }
            return fileData;
        }
        public static byte[] ToArray(this HttpPostedFile file)
        {
            byte[] fileData = null;
            if (file != null)
            {
                using (var binaryReader = new System.IO.BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
            }
            return fileData;
        }
    }
}
