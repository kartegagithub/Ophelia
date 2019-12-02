using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitterizer;

namespace Ophelia.Social
{
    public class Twitter
    {
        public Exception LastException { get; set; }
        public string OauthConsumerKey { get; set; }

        public string OauthConsumerSecret { get; set; }

        public string AccessToken { get; set; }

        public string AccessTokenSecret { get; set; }

        private OAuthTokens OAuthTokens { get; set; }

        public string GetOauthToken(string ResponseUrl)
        {
            try
            {
                OAuthTokenResponse Token = OAuthUtility.GetRequestToken(this.OauthConsumerKey, this.OauthConsumerSecret, ResponseUrl);
                if (Token != null && !string.IsNullOrEmpty(Token.Token))
                    return Token.Token;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public OAuthTokenResponse GetAccessToken(string OauthToken, string OauthVerifier)
        {
            try
            {
                return OAuthUtility.GetAccessToken(this.OauthConsumerKey, this.OauthConsumerSecret, OauthToken, OauthVerifier);
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return null;
        }

        public bool SetOAuthTokens(string OauthToken, string OauthVerifier)
        {
            try
            {
                var AccessToken = OAuthUtility.GetAccessToken(this.OauthConsumerKey, this.OauthConsumerSecret, OauthToken, OauthVerifier);

                this.OAuthTokens = new OAuthTokens()
                {
                    AccessToken = AccessToken.Token,
                    AccessTokenSecret = AccessToken.TokenSecret,
                    ConsumerKey = this.OauthConsumerKey,
                    ConsumerSecret = this.OauthConsumerSecret
                };
                return this.OAuthTokens != null;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }

        public bool SetOAuthTokens()
        {
            try
            {
                this.OAuthTokens = new OAuthTokens()
                {
                    AccessToken = this.AccessToken,
                    AccessTokenSecret = this.AccessTokenSecret,
                    ConsumerKey = this.OauthConsumerKey,
                    ConsumerSecret = this.OauthConsumerSecret
                };
                return this.OAuthTokens != null;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }

        public bool CreateTweet(string Message)
        {
            try
            {
                TwitterResponse<TwitterStatus> response = TwitterStatus.Update(this.OAuthTokens, Message, new StatusUpdateOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });

                if (response != null && response.Result == RequestResult.Success || (response.Result == RequestResult.Unknown && response.ErrorMessage == "Unable to parse JSON"))
                    return true;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }

        public bool CreatePhoto(string Message = "", string fileLocation = "", byte[] fileData = null)
        {
            try
            {
                TwitterResponse<TwitterStatus> response = null;

                if (!string.IsNullOrEmpty(fileLocation))
                    response = TwitterStatus.UpdateWithMedia(this.OAuthTokens, Message, fileLocation, new StatusUpdateOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                else if (fileData != null)
                    response = TwitterStatus.UpdateWithMedia(this.OAuthTokens, Message, fileData, new StatusUpdateOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });

                if (response != null && response.Result == RequestResult.Success || (response.Result == RequestResult.Unknown && response.ErrorMessage == "Unable to parse JSON"))
                    return true;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }

        public bool ReplyTweet(string Message, decimal InReplyToStatusId)
        {
            try
            {
                TwitterResponse<TwitterStatus> response = TwitterStatus.Update(this.OAuthTokens, Message, new StatusUpdateOptions() { APIBaseAddress = "https://api.twitter.com/1.1/", InReplyToStatusId = InReplyToStatusId });

                if (response != null && response.Result == RequestResult.Success || (response.Result == RequestResult.Unknown && response.ErrorMessage == "Unable to parse JSON"))
                    return true;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }

        public string UserTweetList()
        {
            try
            {
                var Result = TwitterTimeline.UserTimeline(this.OAuthTokens, new UserTimelineOptions() { APIBaseAddress = "https://api.twitter.com/1.1/", IncludeRetweets = true });
                if (Result != null)
                    return Result.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public string ViewTweet(decimal StatusID)
        {
            try
            {
                var Result = TwitterStatus.Show(this.OAuthTokens, StatusID, new OptionalProperties() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                if (Result != null)
                    return Result.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public string ReTweetList(decimal StatusID)
        {
            try
            {
                var Result = TwitterStatus.Retweets(this.OAuthTokens, StatusID, new RetweetsOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                if (Result != null)
                    return Result.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public string ShowUser(decimal UserID, string UserName)
        {
            try
            {
                TwitterResponse<TwitterUser> User = null;
                if (UserID > 0)
                    User = TwitterUser.Show(this.OAuthTokens, UserID, new OptionalProperties() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                else if (!string.IsNullOrEmpty(UserName))
                    User = TwitterUser.Show(this.OAuthTokens, UserName, new OptionalProperties() { APIBaseAddress = "https://api.twitter.com/1.1/" });

                if (User != null)
                    return User.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public string IncomingMessages()
        {
            try
            {
                var Result = TwitterDirectMessage.DirectMessages(this.OAuthTokens, new DirectMessagesOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                if (Result != null)
                    return Result.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public string OutgoingMessages()
        {
            try
            {
                var Result = TwitterDirectMessage.DirectMessagesSent(this.OAuthTokens, new DirectMessagesSentOptions() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                if (Result != null)
                    return Result.Content;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return "";
        }

        public bool SendMessage(decimal UserID, string UserName, string Message)
        {
            try
            {
                TwitterResponse<TwitterDirectMessage> Messages = null;

                if (UserID > 0)
                    Messages = TwitterDirectMessage.Send(this.OAuthTokens, UserID, Message, new OptionalProperties() { APIBaseAddress = "https://api.twitter.com/1.1/" });
                else if (!string.IsNullOrEmpty(UserName))
                    Messages = TwitterDirectMessage.Send(this.OAuthTokens, UserName, Message, new OptionalProperties() { APIBaseAddress = "https://api.twitter.com/1.1/" });

                if (Messages != null && Messages.Result == RequestResult.Success || (Messages.Result == RequestResult.Unknown && Messages.ErrorMessage == "Unable to parse JSON"))
                    return true;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
            }
            return false;
        }
    }
}
