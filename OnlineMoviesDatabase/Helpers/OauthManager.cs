using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieDatabase.Helpers
{
    public class OauthManager
    {   public OauthManager(IHostingEnvironment _ie)
        {
                VkOauthHelper   =   new VkOauthHelper(  "Vk",
                                                        _ie.WebRootPath,
                                                        RedirectEndPoint + "Vk");
            GithubOauthHelper = new GithubOauthHelper(  "Github",
                                                        _ie.WebRootPath,
                                                        RedirectEndPoint + "Github");
        }
        public VkOauthHelper VkOauthHelper;
        public GithubOauthHelper GithubOauthHelper;

        public string RedirectEndPoint;
    }
}
