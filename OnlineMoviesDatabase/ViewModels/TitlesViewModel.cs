using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineMovieDatabase.Controllers;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.ViewModels
{
    public class TitlesViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortType sortType { get; set; }
        public string sortTypeStr { get; set; }
        public string sortParm { get; set; }
        public List<string> Genres { get; set; }
    } 
}
