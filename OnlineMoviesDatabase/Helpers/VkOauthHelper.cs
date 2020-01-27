using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Helpers
{
    public class VkUserInfo
    {
        public Response[] response { get; set; }
    }

    public class Response
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string photo_max_orig { get; set; }
    }

    public class VkOauthHelper : OauthHelper
    {
        public VkOauthHelper(string _serviceName,
                            string _contentFolderPath,
                            string _redirectEndPoint)
            : base(_serviceName,
                    _contentFolderPath, 
                    "https://oauth.vk.com/authorize", 
                    "https://oauth.vk.com/access_token",
                    "7194336",
                    null,
                    _redirectEndPoint,
                    "offline,email")
        {
            ResponseType = "token";
            UserInfoHref = "https://api.vk.com/method/users.get";
        }
        private string ResponseType;
        private string UserInfoHref;
        private JsonSerializer JsonHelper = new JsonSerializer();
        public override string GetOauthHref(string _hostUri)
        {
            return base.GetOauthHref(_hostUri) + (string.IsNullOrEmpty(ResponseType) ? "" : "&response_type=" + ResponseType);
        }

        public bool GetOauthUser(int _userId, string _code, string _accessToken, out User _newUser, out Image _newUserAvatar)
        {
            _newUserAvatar = null;
            _newUser = null;
            VkUserInfo info;
            StringBuilder requestString = new StringBuilder(UserInfoHref);
            requestString.Append('?');
            requestString.Append("v=5.52");
            requestString.Append("&user_id=" + _userId.ToString());
            requestString.Append("&access_token=" + _accessToken.ToString());
            requestString.Append("&fields=photo_max_orig");
            WebRequest req = WebRequest.Create(requestString.ToString());
            req.Method = WebRequestMethods.Http.Get;
            req.Timeout = 30000;
            WebResponse resp = req.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string body = reader.ReadToEnd();
                    info = JsonHelper.Deserialize<VkUserInfo>(new JsonTextReader(new StringReader(body)));
                }
            }
            resp.Close();
            _newUser = User.CreateOauthUser("Vk",
                                            info.response[0].id,
                                            info.response[0].first_name);
            if (_newUser == null)
                throw new Exception("Can't create oauth user");
            if (!string.IsNullOrEmpty(info.response[0].photo_max_orig))
            {
                req = WebRequest.Create(info.response[0].photo_max_orig);
                req.Method = WebRequestMethods.Http.Get;
                req.Timeout = 30000;
                resp = req.GetResponse();
                using (Stream stream = resp.GetResponseStream())
                {
                    _newUserAvatar = Image.FromStream(stream);
                }
                resp.Close();
            }
            else
                _newUserAvatar = null;
            return (_newUser != null);
        }
    }
}