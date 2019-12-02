using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Ophelia.Diagnostics;
using Ophelia.Extensions;
using System.Web;

namespace Ophelia
{
    public static class LogsManager
    {
        private static bool LogsEnabled
        {
            get { return ConfigurationManager.GetParameter<bool>("LogsEnabled", true); }
        }
        private static object _FileLocker = new object();
        public static void InsertEntry(string entry, Exception exception = null, string fileName = "")
        {
            if (LogsEnabled)
            {
                try
                {
                    string baseDirectory = string.Empty;
                    if (HttpContext.Current != null)
                        baseDirectory = HttpContext.Current.Server.MapPath("~");
                    else
                        baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string dailyDirectory = DateTime.Today.AsDirectoryName();
                    string filePath = baseDirectory + "\\ApplicationLogs\\" + dailyDirectory + "\\" + DateTime.Today.AsFileName(fileName) + ".txt";
                    lock ((_FileLocker))
                    {
                        if (!Directory.Exists(baseDirectory + "\\ApplicationLogs"))
                            Directory.CreateDirectory(baseDirectory + "\\ApplicationLogs");

                        if (!Directory.Exists(baseDirectory + "\\ApplicationLogs\\" + dailyDirectory))
                            Directory.CreateDirectory(baseDirectory + "\\ApplicationLogs\\" + dailyDirectory);

                        if (!File.Exists(filePath))
                            File.Create(filePath).Close();

                        using (StreamWriter oWriter = File.AppendText(filePath))
                            oWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now, entry));
                    }
                }
                catch (Exception exc)
                {
                    EventLogger.Current.Exclamation(string.Format("{0} - {1}", DateTime.Now, entry));
                    throw exc;
                }
                finally
                {
                    if (exception != null)
                        InsertEntry(string.Format("Hata Detayları: {0}\r\n", exception), null, fileName);
                }
            }
        }
    }
}