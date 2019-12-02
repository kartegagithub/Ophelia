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
    public class Facebook
    {
        public string ApplicationID { get; set; }

        public string AccessToken { get; set; }

        public string ApplicationSecret { get; set; }

        public string GetPage(string PageID, string Fields = "")
        {
            string Response = "";
            try
            {
                string URL = string.Format("https://graph.facebook.com/{0}?access_token={1}", PageID, this.AccessToken);

                if (!string.IsNullOrEmpty(Fields))
                    URL = URL + string.Format("&fields={0}", Fields);

                Response = URL.RequestURL("", "GET");
            }
            catch { }

            return Response;
        }

        public string Authenticate(string RedirectURL, string Scope)
        {
            string Response = "";
            try
            {
                if (string.IsNullOrEmpty(Scope))
                    Scope = "manage_pages,publish_actions,publish_pages,read_page_mailboxes,pages_show_list,pages_manage_cta";

                string URL = string.Format("https://www.facebook.com/v2.5/dialog/oauth?response_type=token&client_id={0}&redirect_uri={1}&scope={2}", this.ApplicationID, RedirectURL, Scope);
                Response = URL.RequestURL("", "GET");
            }
            catch { }

            return Response;
        }

        public string CreateFeed(string PageID, string Message = "", string Link = "")
        {
            string Response = "";
            try
            {
                string URL = string.Format("https://graph.facebook.com/{0}/feed?access_token={1}", PageID, this.AccessToken);

                if (!string.IsNullOrEmpty(Link))
                    URL = URL + string.Format("&link={0}", Link);

                if (!string.IsNullOrEmpty(Message))
                    URL = URL + string.Format("&message={0}", Message);

                Response = URL.RequestURL();
            }
            catch { }

            return Response;
        }

        public string CreateFeedComment(string FeedID, string Message = "")
        {
            string Response = "";
            try
            {
                string URL = string.Format("https://graph.facebook.com/{0}/comments?access_token={1}&message={2}", FeedID, this.AccessToken, Message);

                Response = URL.RequestURL();
            }
            catch { }

            return Response;
        }

        public string CreatePhoto(string PageID, string Link, string Message = "")
        {
            string Response = "";
            try
            {
                string URL = string.Format("https://graph.facebook.com/{0}/photos?access_token={1}&url={2}", PageID, this.AccessToken, Link);

                if (!string.IsNullOrEmpty(Message))
                    URL = URL + string.Format("&message={0}", Message);

                Response = URL.RequestURL();
            }
            catch { }

            return Response;
        }

        public string GetFeeds(string PageID, string Fields = "")
        {
            string Response = "";
            try
            {
                if (string.IsNullOrEmpty(Fields))
                    Fields = "link,message,picture,likes.summary(1).limit(0),comments.summary(1).limit(0)";

                string URL = string.Format("https://graph.facebook.com/{0}/feed?fields={1}&access_token={2}", PageID, Fields, this.AccessToken);

                Response = URL.RequestURL("", "GET");
            }
            catch { }

            return Response;
        }

        public string GetFeedLikedUsers(string FeedID)
        {
            string Response = "";
            try
            {
                string URL = string.Format("https://graph.facebook.com/{0}/likes?fields=name&access_token={1}", FeedID, this.AccessToken);

                Response = URL.RequestURL("", "GET");
            }
            catch { }

            return Response;
        }

        public string GetFeedComments(string FeedID, string Fields = "")
        {
            string Response = "";
            try
            {
                if (string.IsNullOrEmpty(Fields))
                    Fields = "comments{message,from,like_count,attachment,likes}";

                string URL = string.Format("https://graph.facebook.com/{0}?fields={1}&access_token={2}", FeedID, Fields, this.AccessToken);

                Response = URL.RequestURL("", "GET");
            }
            catch { }

            return Response;
        }
    }
}
