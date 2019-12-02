using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ophelia.Web.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void WriteFile(this HttpResponseBase response, byte[] fileContent, string fileName)
        {
            response.WriteFile(fileContent, System.IO.Path.GetFileNameWithoutExtension(fileName), System.IO.Path.GetExtension(fileName).Replace(".", ""));
        }
        public static void WriteFile(this HttpResponseBase response, byte[] fileContent, string fileName, string fileExtension)
        {
            if (fileContent != null)
            {
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = false;
                if (fileExtension.Equals("xml"))
                    response.ContentType = "text/xml";
                else if (fileExtension.Equals("txt"))
                    response.ContentType = "text/plain";
                else if (fileExtension.Equals("pdf"))
                    response.ContentType = "application/pdf";
                else if (fileExtension.Equals("xlsx") || fileExtension.Equals("csv"))
                    response.ContentType = "application/vnd.ms-excel";
                else
                    response.ContentType = "application/octet-stream";

                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + fileExtension);
                response.OutputStream.Write(fileContent, 0, fileContent.Length);
            }
            response.Flush();
            response.End();
        }
        public static void WriteFile(this HttpResponse response, byte[] fileContent, string fileName)
        {
            response.WriteFile(fileContent, System.IO.Path.GetFileNameWithoutExtension(fileName), System.IO.Path.GetExtension(fileName).Replace(".", ""));
        }
        public static void WriteFile(this HttpResponse response, byte[] fileContent, string fileName, string fileExtension)
        {
            if (fileContent != null)
            {
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = false;
                if (fileExtension.Equals("xml"))
                    response.ContentType = "text/xml";
                else if (fileExtension.Equals("txt"))
                    response.ContentType = "text/plain";
                else if (fileExtension.Equals("pdf"))
                    response.ContentType = "application/pdf";
                else if (fileExtension.Equals("xlsx") || fileExtension.Equals("csv"))
                    response.ContentType = "application/vnd.ms-excel";
                else
                    response.ContentType = "application/octet-stream";

                response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + fileExtension);
                response.OutputStream.Write(fileContent, 0, fileContent.Length);
            }
            response.Flush();
            response.End();
        }
    }
}
