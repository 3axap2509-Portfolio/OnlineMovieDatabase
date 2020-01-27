using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Helpers
{
    public class GithubOauthHelper : OauthHelper
    {
        public GithubOauthHelper(string _serviceName,
                            string _contentFolderPath,
                            string _redirectEndPoint,
                            string _scope = "user",
                            string _responseType = null)
            : base(_serviceName,
                    _contentFolderPath,
                    "https://github.com/login/oauth/authorize",
                    "https://github.com/login/oauth/access_token",
                    "2d3b3d2e2d385bbb1c7c",
                    "573b3fed287ee4aadfa5acd95dee320086ced61b",
                    _redirectEndPoint,
                    _scope)
        {
            ResponseType = "token";
            UserInfoHref = "https://api.github.com/user";
        }
        private string ResponseType;
        private string UserInfoHref;
        private JsonSerializer JsonHelper = new JsonSerializer();
        public override string GetOauthHref(string _hostUri)
        {
            return base.GetOauthHref(_hostUri) + (string.IsNullOrEmpty(ResponseType) ? "" : "&response_type=" + ResponseType);
        }

        public bool GetOauthUser(int _userId, string _code, string _accessToken, out User _user, out Image _userAvatar)
        {
            _user = null;
            VkUserInfo info;
            StringBuilder requestString = new StringBuilder(UserInfoHref);
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
            _user = User.CreateOauthUser("Vk", info.response[0].id, info.response[0].first_name);
            if (_user == null)
                throw new Exception("Can't create oauth user");
            req = WebRequest.Create(info.response[0].photo_max_orig);
            req.Method = WebRequestMethods.Http.Get;
            req.Timeout = 30000;
            resp = req.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                _userAvatar = Image.FromStream(stream);
            }
            resp.Close();
            return (_user != null);
        }
        public bool ResumeOauthWithCode(string _host, string _code, string state, out User _newUser, out Image _newUserAvatar)
        {
            _newUserAvatar = null;
            _newUser = null;
            string OauthTokenHref = GetOauthTokenHref(_host, state) + "&code=" + _code;
            WebRequest req = WebRequest.Create(OauthTokenHref);
            req.Headers.Set(HttpRequestHeader.Accept, "application/json");
            req.Method = WebRequestMethods.Http.Get;
            req.Timeout = 30000;
            WebResponse resp = req.GetResponse();
            JsonSerializer JsonHelper = new JsonSerializer();
            GithubToken token;
            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string body = reader.ReadToEnd();
                    token = JsonHelper.Deserialize<GithubToken>(new JsonTextReader(new StringReader(body)));
                }
                if (string.IsNullOrEmpty(token.access_token))
                    throw new Exception("Невозможно получить Oauth access token");
            }
            resp.Close();
            StringBuilder requestString = new StringBuilder(UserInfoHref);
            requestString.Append("?client_id=" + ClientId);
            requestString.Append("&client_secret=" + ClientSecret);
            req = WebRequest.Create(requestString.ToString());
            req.Method = WebRequestMethods.Http.Get;
            req.PreAuthenticate = true;
            req.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token.access_token);
            req.Headers.Add(HttpRequestHeader.Accept, "*/*");
            req.Headers.Add(HttpRequestHeader.UserAgent, "OnlineMovieDatabase");
            req.Timeout = 30000;
            resp = req.GetResponse();
            GithubUser user;
            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string body = reader.ReadToEnd();
                    user = JsonHelper.Deserialize<GithubUser>(new JsonTextReader(new StringReader(body)));
                }
            }
            resp.Close();
            if (user == null)
                throw new Exception("Can't take userInfo from Github profile");
            _newUser = User.CreateOauthUser("Github", user.id, user.login, user.avatar_url);
            if (!string.IsNullOrEmpty(user.avatar_url))
            {
                req = WebRequest.Create(user.avatar_url);
                req.Method = WebRequestMethods.Http.Get;
                req.Timeout = 30000;
                resp = req.GetResponse();
                using (Stream stream = resp.GetResponseStream())
                {
                    _newUserAvatar = Image.FromStream(stream);
                }
                resp.Close();
            }
            return (_newUser != null);
        }
    }
    public class GithubToken
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    public class GithubUser
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string blog { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public bool? hireable { get; set; }
        public string bio { get; set; }
        public int public_repos { get; set; }
        public int public_gists { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int private_gists { get; set; }
        public int total_private_repos { get; set; }
        public int owned_private_repos { get; set; }
        public int disk_usage { get; set; }
        public int collaborators { get; set; }
        public bool two_factor_authentication { get; set; }
        public Plan plan { get; set; }
    }

    public class Plan
    {
        public string name { get; set; }
        public int space { get; set; }
        public int private_repos { get; set; }
        public int collaborators { get; set; }
    }


}