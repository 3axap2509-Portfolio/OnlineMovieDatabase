using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MiniGuids;
using OnlineMovieDatabase.Helpers;

namespace OnlineMovieDatabase.ViewModels
{
    public class LoginViewModel
    {
        public OauthHelper VkOauthHelper;
        public OauthHelper GoogleOauthHelper;
        public OauthHelper GithubOauthHelper;
        public LoginViewModel()
        {
            
        }
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
