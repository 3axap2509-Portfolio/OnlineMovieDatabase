using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Controllers
{
    public class SearchController : Controller
    {
        public SearchController(OMDB_Context _c)
        {
            db = _c;
        }
        private OMDB_Context db;

        [HttpGet]
        [Route("Search")]
        public IActionResult Index(string SearchString)
        {
            List<Movie> model = db.Movies.Where(e => e.RuTitle.Contains(SearchString)).ToList();
            if (model.Count > 0)
                return View(model);
            else
                return View(null);
        }
    }
}