using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Facebook;

namespace FinalWebApp.SocialMedia
{
    public class FacebookApi
    {
        private const string FacebookApiId = "255179092051265";
        private const string FacebookApiSecret = "017fd6c5440c10bde765f901c83a52f2";

        private const string AuthenticationUrlFormat =
            "https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials&scope=manage_pages,offline_access,publish_stream";

        public static void foo()
        {
            var client = new HttpClient();
            var token = client.GetStringAsync("https://graph.facebook.com/your-page-id?fields=access_token&access_token=431422607399723|FIxvQjKp4yf5N763JsFj9i-1k6o");



        }

        public static void PostMessage(string msg)
        {
            string accessToken = "EAAGIYGdDGysBAExrAuUOqtozEUsZAUp0hHVzuQz8wrFwDQStq6wl9gR8Vfvtx9As9DCr6bCZAVFicXehpRq0F1rf0vxZCwm7VKROmPVZB5vJiQTcipizufQU8X5j2Y7oqhTuD2n0nCHpTZAAAM8VxLLhZBOrX2k7Kk4Yth6ykH3KdcGcxFSDtE9PDu3tGkwTiaeRePvGArVoGEFxurwWgZBUZCyaKygzyEhoCZAq1KUdDaCZChKZCh0ZADKe8LpmxjTKu2wZD";

            var client = new FacebookClient();
            client.AppId = FacebookApiId;
            client.AppSecret = FacebookApiSecret;

            PostMessage(accessToken, msg);
        }

        static string GetAccessToken(string apiId, string apiSecret)
        {
            string accessToken = string.Empty;
            string url = string.Format(AuthenticationUrlFormat, apiId, apiSecret);

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                var responseString = reader.ReadToEnd();

                NameValueCollection query = HttpUtility.ParseQueryString(responseString);

                accessToken = query["access_token"];
            }

            if (accessToken.Trim().Length == 0)
                throw new Exception("There is no Access Token");

            return accessToken;
        }

        static void PostMessage(string accessToken, string message)
        {
            try
            {
                FacebookClient facebookClient = new FacebookClient(accessToken);

                dynamic messagePost = new ExpandoObject();
                messagePost.access_token = accessToken;
                //messagePost.picture = "[A_PICTURE]";
                //messagePost.link = "[SOME_LINK]";
                //messagePost.name = "[SOME_NAME]";
                //messagePost.caption = "my caption"; 
                messagePost.message = message;
            //messagePost.description = "my description";

            var result = facebookClient.Post("/411946416233851/feed", messagePost);
            }
            catch (FacebookOAuthException ex)
            {
                //handle something
            }
            catch (Exception ex)
            {
                //handle something else
            }

        }

    }
}
