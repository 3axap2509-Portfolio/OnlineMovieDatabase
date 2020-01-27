using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Helpers;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(OMDB_Context _c, OauthManager _om)
        {
             db = _c; 
        }
        private OMDB_Context db;
        public IActionResult Index()
        {
            bool isContains = false;
            Random r = new Random();
            List<long> idList = new List<long>();
            List<long> resIdsList = new List<long>(20);
             List<Movie> model = new List<Movie>(20);
            int ind = 0;
            foreach (Movie s in db.Movies.ToList())
            {
                idList.Add(s.Id);
            }
            for(int i = 0; i < 20; i++)
            {
                isContains = true;
                while(isContains)
                {
                    ind = r.Next(idList.Count);
                    isContains = (resIdsList.Contains(ind));
                }
                resIdsList.Add(idList[ind]);
            }
            model.AddRange(db.Movies.Where(el => resIdsList.Contains(el.Id)));
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return new ContentResult() { Content = "Ошибка" };
        }
        

        
    }
}
