using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MiniGuids;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Helpers
{
    public class OauthHelper
    {
        public OauthHelper(string _serviceName, string _configFolderPath, string _authEndPoint, string _tokenEndPoint, string _clientId, string _secret, string _redirectEndPoint, string _scope = null)
        {
            ServiceName = _serviceName;
            ContentFolderPath = _configFolderPath;
            AuthReqState = MiniGuid.NewGuid();
            RedirectEndPoint = _redirectEndPoint;
            ClientId = _clientId;
            AuthEndPoint = _authEndPoint;
            TokenEndPoint = _tokenEndPoint;
            Scope = _scope;
            ClientSecret = _secret;
        }

        public string ContentFolderPath;
        public string ServiceName;
        public string ClientSecret;
        public string AuthEndPoint;
        public string ClientId;
        private string RedirectEndPoint;
        private string Scope;
        private string TokenEndPoint;
        private MiniGuid AuthReqState;

        public virtual string GetOauthHref(string _hostUri)
        {
            _hostUri += _hostUri.EndsWith("/") ? "" : "/";
            return (AuthEndPoint + "?client_id=" + ClientId +
                    "&redirect_uri=" + _hostUri + RedirectEndPoint +
                    (string.IsNullOrEmpty(Scope) ? "" : "&scope=" + Scope) +
                    "&state=" + AuthReqState.ToString());
        }
        public virtual string GetOauthTokenHref(string _hostUri, string _state = null)
        {
            _hostUri += _hostUri.EndsWith("/") ? "" : "/";
            return (TokenEndPoint + "?client_id=" + ClientId +
                    "&client_secret=" + ClientSecret +
                    "&redirect_uri=" + _hostUri + RedirectEndPoint +
                    (string.IsNullOrEmpty(Scope) ? "" : "&scope=" + Scope) +
                    "&state=" + AuthReqState.ToString());
        }
        
    }
    
}