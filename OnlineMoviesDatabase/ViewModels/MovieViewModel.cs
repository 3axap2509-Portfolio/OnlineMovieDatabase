using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.ViewModels
{
    public class MovieViewModel
    {
        public MovieViewModel(Movie m, User u, List<KeyValuePair<UserMinimalInfo, Comment>> uc, List<KeyValuePair<UserMinimalInfo, Review>> ur)
        {
            reviews = ur;
            comments = uc;
            user = u;
            movie = m;
        }
        public Movie movie;
        public User user;
        public List<KeyValuePair<UserMinimalInfo, Comment>> comments;
        public List<KeyValuePair<UserMinimalInfo, Review>> reviews;
    }
}
