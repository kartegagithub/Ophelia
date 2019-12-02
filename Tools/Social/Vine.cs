using Ophelia.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ophelia.Social
{
    public class Vine : Base.IApplication
    {
        public Uri ApiUrl
        {
            get { return new Uri("https://api.vineapp.com/"); }
        }
        public string Login(string userName, string password)
        {
            string userData = string.Empty;
            try
            {
                string postData = string.Format("username={0}&password={1}", userName, password);
                byte[] dataStream = Encoding.UTF8.GetBytes(postData);
                WebRequest webRequest = WebRequest.Create(this.ApiUrl.AbsolutePath + "users/authenticate");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = dataStream.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(dataStream, 0, dataStream.Length);
                    requestStream.Close();
                }

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        userData = reader.ReadToEnd();
                        reader.Close();
                    }
                    webResponse.Close();
                }
            }
            catch (Exception)
            {
                //TODO: Handle Exception.
            }

            return userData;
        }
        public bool Logout()
        {
            bool success = false;
            try
            {
                WebRequest webRequest = WebRequest.Create(this.ApiUrl.AbsolutePath + "users/authenticate");
                webRequest.Method = "DELETE";
                webRequest.ContentType = "application/x-www-form-urlencoded";

                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        dynamic result = reader.ReadToEnd().FromJson<dynamic>();
                        success = (bool)result["success"];
                        reader.Close();
                    }
                    webResponse.Close();
                }
            }
            catch (Exception)
            {
                success = false;
                //TODO: Handle Exception.
            }

            return success;
        }
        public void GetPopular()
        {

        }
        public void SearchUser(string userName)
        {

        }
        public void GetUserData()
        {

        }
        public void GetUserProfile()
        {

        }
        public void GetUserTimeline()
        {

        }
        public void GetTag()
        {

        }
        public void GetSinglePost()
        {

        }
        public void GetNotifications()
        {

        }
        public void LikePost()
        {

        }
        public void FollowUser()
        {

        }
    }
}
