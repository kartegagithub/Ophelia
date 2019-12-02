using Ophelia.Extensions;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace Ophelia.Net
{
    public class FtpClient
    {
        public string FtpServerAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public FtpClient(string serverAddress, string userName, string password)
        {
            this.FtpServerAddress = serverAddress;
            this.UserName = userName;
            this.Password = password;
        }

        private FtpWebRequest GetRequest(Uri uri, string method, bool? keepAlive = null, bool? useBinary = null, long? contentLength = null)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(uri);
            ftpRequest.Credentials = new NetworkCredential(this.UserName, this.Password);
            ftpRequest.Method = method;
            if (keepAlive.HasValue)
                ftpRequest.KeepAlive = keepAlive.Value;
            if (useBinary.HasValue)
                ftpRequest.UseBinary = useBinary.Value;
            if (contentLength.HasValue)
                ftpRequest.ContentLength = contentLength.Value;
            return ftpRequest;
        }
        public bool CheckConnection(string fileDirectory)
        {
            bool result = false;
            try
            {
                Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + fileDirectory);
                FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.ListDirectory, null, true);

                using (WebResponse ftpResponse = ftpRequest.GetResponse())
                {
                    ftpResponse.Close();
                }
                result = true;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return result;
        }
        public void Upload(string filename)
        {
            FileInfo file = new FileInfo(filename);
            Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + file.Name);
            FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.UploadFile, false, true, file.Length);

            int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];
            int contentLength;

            try
            {
                using (FileStream fileStream = file.OpenRead())
                {
                    using (Stream requestStream = ftpRequest.GetRequestStream())
                    {
                        contentLength = fileStream.Read(buffer, 0, bufferLength);

                        while (contentLength != 0)
                        {
                            requestStream.Write(buffer, 0, contentLength);
                            contentLength = fileStream.Read(buffer, 0, bufferLength);
                        }

                        requestStream.Close();
                    }
                    fileStream.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
        public void UploadImage(Bitmap image, string filePath)
        {
            byte[] bytes = image.ToByteArray();
            Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + filePath);
            FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.UploadFile, false, true, bytes.Length);

            try
            {
                using (Stream requestStream = ftpRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
        public void Download(string filePath, string fileName)
        {
            try
            {
                Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + fileName);
                FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.DownloadFile, false, true);

                using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    using (FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create))
                    {
                        using (Stream ftpStream = ftpResponse.GetResponseStream())
                        {
                            long contentLength = ftpResponse.ContentLength;
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[bufferSize];

                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                outputStream.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }

                            ftpStream.Close();
                        }
                        outputStream.Close();
                    }
                    ftpResponse.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
        public void DeleteFile(string fileName)
        {
            try
            {
                Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + fileName);
                FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.DeleteFile, false);
                ftpRequest.GetResponse();
            }
            catch (Exception)
            {
                
            }
        }

        public string[] ListDirectoryDetails()
        {
            string[] files = null;
            try
            {
                Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/");
                FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.ListDirectoryDetails);

                using (WebResponse response = ftpRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        StringBuilder result = new StringBuilder();
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line).Append("\n");
                            line = reader.ReadLine();
                        }
                        if (result.Length > 0)
                        {
                            result.Remove(result.ToString().LastIndexOf("\n"), 1);
                            files = result.ToString().Split('\n');
                        }
                        reader.Close();
                    }
                    response.Close();
                }
            }
            catch (Exception)
            {
                files = null;
            }
            return files;
        }

        public string[] ListDirectory()
        {
            string[] files = null;
            StringBuilder result = new StringBuilder();
            try
            {
                Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/");
                FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.ListDirectory, null, true);

                using (WebResponse ftpResponse = ftpRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(ftpResponse.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line).Append("\n");
                            line = reader.ReadLine();
                        }
                        if (result.Length > 0)
                        {
                            result.Remove(result.ToString().LastIndexOf('\n'), 1);
                            files = result.ToString().Split('\n');
                        }
                        reader.Close();
                    }
                    ftpResponse.Close();
                }
            }
            catch
            {
                files = null;
            }
            return files;
        }

        public long GetFileSize(string fileName)
        {
            long fileSize = 0;
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + fileName);
                    FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.GetFileSize, null, true);
                    using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                    {
                        fileSize = ftpResponse.ContentLength;
                        ftpResponse.Close();
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return fileSize;
        }

        public void Rename(string oldName, string newName)
        {
            try
            {
                if (!string.IsNullOrEmpty(oldName) && !string.IsNullOrEmpty(newName))
                {
                    Uri uri = new Uri("ftp://" + this.FtpServerAddress + "/" + oldName);
                    FtpWebRequest ftpRequest = this.GetRequest(uri, WebRequestMethods.Ftp.Rename, null, true);
                    ftpRequest.RenameTo = newName;
                    ftpRequest.GetResponse();
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void CreateDirectory(string directoryName)
        {
            try
            {
                if (!string.IsNullOrEmpty(directoryName))
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.FtpServerAddress + "/" + directoryName));
                    ftpRequest.Credentials = new NetworkCredential(this.UserName, this.Password);
                    ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpRequest.UseBinary = true;

                    using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                        ftpResponse.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void RemoveDirectory(string directoryName)
        {
            try
            {
                if (!string.IsNullOrEmpty(directoryName))
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.FtpServerAddress + "/" + directoryName));
                    ftpRequest.Credentials = new NetworkCredential(this.UserName, this.Password);
                    ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                    ftpRequest.UseBinary = true;

                    using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                        ftpResponse.Close();
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
