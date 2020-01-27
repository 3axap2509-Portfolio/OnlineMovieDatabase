using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.ViewModels
{
    public class AccountDetailsViewModels
    {
        public AccountDetailsViewModels(User _user, int _reviewsCounter, int _commentsCounter)
        {
            User = _user;
            ReviewsCounter = _reviewsCounter;
            CommentsCounter = _commentsCounter;
        }
        public AccountDetailsViewModels()
        {
        }
        public User User;
        public int ReviewsCounter{ get; set; }
        public int CommentsCounter{ get; set; }
    }
}
