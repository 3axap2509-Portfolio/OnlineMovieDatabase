using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Helpers;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Controllers
{
    public class GenresController : Controller
    {
        private readonly OMDB_Context db;
        private readonly MailKitService mailService;
        public GenresController(OMDB_Context context, MailKitService _mk)
        {
            db = context;
            mailService = _mk;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Genres.ToListAsync());
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RuGenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                if (!db.Genres.Any(gnr => gnr.RuGenreName == genre.RuGenreName))
                {
                    genre.OriginalGenreName = genre.RuGenreName;
                    await db.Genres.AddAsync(genre);
                    await db.SaveChangesAsync();
                    await mailService.SendUpdateNews(genre);

                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("Name", "Такой жанр уже существует");
            }
            return View(genre);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var genre = await db.Genres.FirstOrDefaultAsync(gen => gen.Id == id);
            if (genre == null)
                return NotFound();
            return View(genre);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            db.MoviesGenres.RemoveRange(db.MoviesGenres.Where(e => e.GenreId == id));
            await db.SaveChangesAsync();

            List<Movie> moviesToRemove = db.Movies.Where(mov => db.MoviesGenres.Count(movg => movg.MovieId == mov.Id) == 0).ToList();
            foreach(Movie mov in moviesToRemove)
            {
                db.Reviews.RemoveRange(db.Reviews.Where(rev => rev.MovieId == mov.Id));
                db.Comments.RemoveRange(db.Comments.Where(com => com.MovieId == mov.Id));
            }
            await db.SaveChangesAsync();
            db.Movies.RemoveRange(moviesToRemove);
            Genre genre = await db.Genres.FindAsync(id);
            db.Genres.Remove(genre);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool GenreExists(string id)
        {
            return db.Genres.Any(e => e.RuGenreName == id);
        }
    }
}
