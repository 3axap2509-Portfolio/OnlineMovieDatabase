using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя:")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль:")]
        public string currentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль:")]
        public string newPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Повторите новый пароль:")]
        public string newPasswordRepeat { get; set; }


        [Required]
        [Display(Name = "Уведомления на почтовый ящик:")]
        public bool NeedToNotify { get; set; }
        public User user { get; set; }
    }
}
