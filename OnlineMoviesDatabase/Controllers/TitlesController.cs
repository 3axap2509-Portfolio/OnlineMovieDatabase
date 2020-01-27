using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using OnlineMovieDatabase.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnlineMovieDatabase.Helpers;
using System.Threading;
using System.Globalization;

namespace OnlineMovieDatabase.Controllers
{
    public enum SortType
    {
        Name_asc,
        Name_desc,
        Rating_asc,
        Rating_desc,    
        Year_asc,
        Year_desc
    }
    public class TitlesController : Controller
    {
        private SortType sortType = 0;
        private string sortParm = string.Empty;
        private string sortTypeStr = string.Empty;
        private List<string> genres;
        public TitlesController(IHostingEnvironment _ae, OMDB_Context _c, MailKitService _mk)
        {
            appEnvironment = _ae;
            db = _c;
            mailService = _mk;
            genres = (from gen in _c.Genres select gen.RuGenreName).ToList();
            genres.Add("все");
        }
        private readonly IHostingEnvironment appEnvironment;
        private readonly MailKitService mailService;
        private OMDB_Context db;
        private static int pageSize = 20;
        [HttpGet]
        public async Task<IActionResult> Index(string sortTypeStr = "имени(возр)", string sortParm = "все", int page = 1)
        {
            this.sortParm = sortParm;
            this.sortTypeStr = sortTypeStr;
            List<Movie> source = new List<Movie>();
            if (sortParm != "все")
            {
                Genre g = await db.Genres.FirstAsync(el => el.RuGenreName.ToLower() == sortParm);
                List<MovieGenres> MoviesOfCurrentGenre = await db.MoviesGenres.Where(el => el.GenreId == g.Id).ToListAsync();
                foreach (MovieGenres el in MoviesOfCurrentGenre)
                {
                    source.Add(await db.Movies.FirstAsync(m => m.Id == el.MovieId));
                }
            }
            else
            {
                source = db.Movies.ToList();
            }
            switch (sortTypeStr)
            {
                case "имени(возр.)":
                    {
                        this.sortType = SortType.Name_asc;
                        source = source.OrderBy(s => s.RuTitle).ToList();
                        break;
                    }
                case "имени(убыв.)":
                    {
                        this.sortType = SortType.Name_desc;
                        source = source.OrderByDescending(s => s.RuTitle).ToList();
                        break;
                    }
                case "рейтингу(возр.)":
                    {
                        this.sortType = SortType.Rating_asc;
                        source = source.OrderBy(s => s.Rating).ToList();
                        break;
                    }
                case "рейтингу(убыв.)":
                    {
                        this.sortType = SortType.Rating_desc;
                        source = source.OrderByDescending(s => s.Rating).ToList();
                        break;
                    }
                case "дате выхода(возр.)":
                    {
                        this.sortType = SortType.Year_asc;
                        source = source.OrderBy(s => s.ReleaseDate).ToList();
                        break;
                    }
                case "дате выхода(убыв.)":
                    {
                        this.sortType = SortType.Year_desc;
                        source = source.OrderByDescending(s => s.ReleaseDate).ToList();
                        break;
                    }
                default:
                    break;
            }
            int count = source.Count;
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            TitlesViewModel viewModel = new TitlesViewModel
            {
                PageViewModel = pageViewModel,
                Movies = items,
                sortType = this.sortType,
                sortParm = this.sortParm,
                Genres = genres,
                sortTypeStr = this.sortTypeStr
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("Titles/{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || id < 0)
                return NotFound();

            User user = User.Identity.IsAuthenticated ? await db.Users.FirstOrDefaultAsync(el=>el.UserName == User.Identity.Name): null;
            var movie = await db.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();

            movie.GenresList = db.MoviesGenres.Where(el => el.MovieId == movie.Id).ToList();
            List<int> IdTempList = (from genreElement in movie.GenresList select genreElement.GenreId).ToList();
            foreach (int el in IdTempList)
            {
                Genre tempGenre =await db.Genres.FirstOrDefaultAsync(g => g.Id == el);
                if (tempGenre != null)
                    movie.Genres += string.IsNullOrEmpty(movie.Genres) ?
                        tempGenre.RuGenreName : ", " + tempGenre.RuGenreName;
            }
            List<KeyValuePair<UserMinimalInfo, Comment>> Comments = new List<KeyValuePair<UserMinimalInfo, Comment>>();
            List<KeyValuePair<UserMinimalInfo, Review>> Reviews = new List<KeyValuePair<UserMinimalInfo, Review>>();
            foreach(Comment com in db.Comments.Where(c=>c.MovieId == id))
                Comments.Add(new KeyValuePair<UserMinimalInfo, Comment>((await db.Users.FirstOrDefaultAsync(u => u.Id == com.UserId)).GetUserMinimalInfo(), com));
            foreach (Review rev in db.Reviews.Where(r => r.MovieId == id))
                Reviews.Add(new KeyValuePair<UserMinimalInfo, Review>((await db.Users.FirstOrDefaultAsync(u => u.Id == rev.UserId)).GetUserMinimalInfo(), rev));
            MovieViewModel model = new MovieViewModel(movie, user, Comments, Reviews);
            return View(model);
        }

        [HttpGet, Route("Titles/Create")]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("RuTitle,RuDescription,ReleaseDate,Rating")] Movie movie, string Genres, IFormFile poster)
        {
            Movie sss = new Movie();
            if (ModelState.IsValid)
            {
                List<string> sList = new List<string>();
                List<Genre> gList = await db.Genres.ToListAsync();
                foreach (Genre g in gList)
                {
                    sList.Add(g.RuGenreName.ToLower());
                }
                foreach (string s in Genres.Split(", "))
                {
                    if (!sList.Contains(s.Trim().ToLower()))
                    {
                        ModelState.ClearValidationState("Genres");
                        ModelState.AddModelError("Genres", "Не распознан 1 из жанров, исправьте(или создайте отсутствующий жанр) и повторите попытку");
                        return View(movie);
                    }
                }
                movie.OriginalDescription = movie.RuDescription;
                movie.OriginalTitle = movie.RuTitle;
                await db.Movies.AddAsync(movie); 
                await db.SaveChangesAsync();
                List<Genre> AddedMovieGenres = new List<Genre>();
                Genre genre;
                foreach (string s in Genres.Split(","))
                {
                    genre = await db.Genres.FirstOrDefaultAsync(el => el.RuGenreName.Trim().ToLower() == s.Trim().ToLower());
                    if(genre != null)
                        AddedMovieGenres.Add(genre);
                }
                foreach (Genre el in AddedMovieGenres)
                {
                    db.MoviesGenres.Add(new MovieGenres()
                    {
                        MovieId = movie.Id,
                        GenreId = el.Id
                    });
                }

                if (poster != null)
                {
                    string path = "/static/images/Posters/" + (movie.Id).ToString() + ".jpg";
                    using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await poster.CopyToAsync(fileStream);
                    }
                }
                await db.SaveChangesAsync();
                await mailService.SendUpdateNews(movie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        [HttpGet, Route("Titles/{id}/Edit")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            List<int> genresIds = (from mgens in db.MoviesGenres.Where(mg => mg.MovieId == id) select mgens.GenreId).ToList();
            List<string> genreNames = (from genre in db.Genres where genresIds.Contains(genre.Id) select genre.RuGenreName).ToList();
            for (int i = 0; i < genreNames.Count; i++)
            {
                movie.Genres += i == 0 ? genreNames[i] + ", " : (i == genreNames.Count - 1 ? genreNames[i].ToLower() : genreNames[i].ToLower() + ", ");
            }
            return View(movie);
        }  

        [HttpPost, Route("Titles/{id}/Edit")]
        [Authorize(Roles = "admin")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,OriginalTitle,RuTitle,OriginalDescription,RuDescription,ReleaseDate,Rating")] Movie movie, string Genres, IFormFile newPoster)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if(!string.IsNullOrEmpty(Genres))
                    {
                        List<string> sList = new List<string>();
                        List<Genre> gList = await db.Genres.ToListAsync();
                        foreach (Genre genre in gList)
                        {
                            sList.Add(genre.RuGenreName.ToLower());
                        }
                        foreach (string genre in Genres.Split(", "))
                        {
                            if (!sList.Contains(genre.Trim().ToLower()))
                            {
                                ModelState.ClearValidationState("Genres");
                                ModelState.AddModelError("Genres", "Не распознан 1 из жанров, исправьте и повторите попытку");
                                return View(movie);
                            }
                        }
                        List<MovieGenres> sgList = await db.MoviesGenres.Where(el => el.MovieId == movie.Id).ToListAsync();
                        if (sgList.Count > 0)
                            db.MoviesGenres.RemoveRange(sgList);
                        foreach (string genre in Genres.Split(", "))
                        {
                            db.MoviesGenres.Add(new MovieGenres()
                            {
                                MovieId = movie.Id,
                                GenreId = (await db.Genres.FirstAsync(gen => gen.RuGenreName.ToLower() == genre.Trim().ToLower())).Id
                            });
                        }
                    }
                    db.Movies.Update(movie);
                    db.SaveChanges();

                    if (newPoster != null)
                    {
                        string path = "/static/images/Posters/" + id.ToString() + ".jpg";
                        using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await newPoster.CopyToAsync(fileStream);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/Titles/{movie.Id}");
            }
            return RedirectToAction($"/Titles/{movie.Id}");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var Movies = await db.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Movies == null)
                return NotFound();

            return View(Movies);
        }


        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            db.MoviesGenres.RemoveRange(db.MoviesGenres.Where(mg => mg.MovieId == id));
            db.Reviews.RemoveRange(db.Reviews.Where(rev => rev.MovieId == id));
            db.Comments.RemoveRange(db.Comments.Where(com => com.MovieId == id));
            await db.SaveChangesAsync();
            var movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, Authorize, Route("Titles/{MovieId}/AddCommentOrReview")]
        public async Task<IActionResult> AddCommentOrReview(int MovieId, int UserId, string ComRevText, string CommentOrReview, string Rating)
        {
            bool check4MovieId = await db.Movies.FirstOrDefaultAsync(movie => movie.Id == MovieId) != null;
            bool check4UserId = await db.Users.FirstOrDefaultAsync(user => user.Id == UserId) != null && User.FindFirst("Id").Value == UserId.ToString();
            bool userAlreadyHaveReview = false;
            if (check4MovieId)
            {
                if (check4UserId)
                {
                    CommentOrReview = CommentOrReview.ToLower().Trim();
                    userAlreadyHaveReview = await db.Reviews.AnyAsync(rev => rev.UserId == UserId && rev.MovieId == MovieId);
                    if (CommentOrReview == "com")
                    {
                        await db.Comments.AddAsync(new Comment(MovieId, UserId, ComRevText));
                        await db.SaveChangesAsync();
                        List<KeyValuePair<UserMinimalInfo, Comment>> Entities = new List<KeyValuePair<UserMinimalInfo, Comment>>();
                        foreach (Comment com in db.Comments.Where(c => c.MovieId == MovieId).ToList())
                        {
                            Entities.Add(new KeyValuePair<UserMinimalInfo, Comment>(db.Users.First(u => u.Id == com.UserId).GetUserMinimalInfo(), com));
                        }
                        double sumRating = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr.Rating).Sum();
                        int reviewsCount = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr).Count();
                        double resultRating = 0;
                        if (reviewsCount > 0 && sumRating > 0)
                        {
                            resultRating = sumRating / reviewsCount;
                        }
                        (await db.Movies.FirstAsync(mov => mov.Id == MovieId)).Rating = resultRating;
                        await db.SaveChangesAsync();
                        return Json(new
                        {
                            result = true,
                            rating = Math.Round(resultRating, 2),
                            entityList = Entities.OrderByDescending(e => e.Value.CommentDate).ToList(),
                            haveReview = userAlreadyHaveReview,
                            message = "Ваш комментарий успешно добавлен"
                        });
                    }
                    else if(CommentOrReview == "rev")
                    {
                        Review existingReview = await db.Reviews.FirstOrDefaultAsync(rev => rev.UserId == UserId && rev.MovieId == MovieId);
                        if (existingReview == null)
                        {
                            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
                            await db.Reviews.AddAsync(new Review(MovieId, UserId, Convert.ToDouble(Rating/*.Replace('.', ',')*/), ComRevText));
                            await db.SaveChangesAsync();
                            Thread.CurrentThread.CurrentCulture = currentCulture;
                            List<KeyValuePair<UserMinimalInfo, Review>> Entities = new List<KeyValuePair<UserMinimalInfo, Review>>();
                            foreach (Review rev in db.Reviews.Where(r => r.MovieId == MovieId).ToList())
                            {
                                Entities.Add(new KeyValuePair<UserMinimalInfo, Review>(db.Users.First(u => u.Id == rev.UserId).GetUserMinimalInfo(), rev));
                            }

                            double sumRating = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr.Rating).Sum();
                            int reviewsCount = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr).Count();
                            double resultRating = 0;
                            if (reviewsCount > 0 && sumRating > 0)
                            {
                                resultRating = sumRating / reviewsCount;
                            }
                            (await db.Movies.FirstAsync(mov => mov.Id == MovieId)).Rating = resultRating;
                            await db.SaveChangesAsync();
                            return Json(new
                            {
                                result = true,
                                rating = Math.Round(resultRating,2),
                                entityList = Entities.OrderByDescending(e=> e.Value.ReviewDate).ToList(),
                                haveReview = true,
                                message = "Ваш отзыв успешно добавлен"
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                result = false,
                                message = "Отзыв не может быть добавлен, так как вы уже оставили отзыв к этому фильму/сериалу."
                            });
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            result=false,
                            message="Неверный параметр 'CommentOrReview', перезагрузите страницу и повторите попытку"
                        });
                    }
                }
                else
                    return Json(new
                    {
                        result = true,
                        message = "Id пользователя, отправленный с " +
                        CommentOrReview == "rev"? "отзывом" : "комментарием" +
                        " не совпадает с вашим Id." +
                        "\nЭто может быть вызвано изменением HTML-кода страницы." +
                        "\nПерезагрузите страницу и повторите попытку."
                    });
            }
            else
                return Json(new
                {
                    result = true,
                    message = "Неверный Id фильма(скорее всего изменён HTML-код)\n" +
                    "Перезагрузите страницу и повторите попытку"
                });
        }

        [HttpPost, Authorize, Route("Titles/{MovieId}/RemoveCommentOrReview")]
        public async Task<IActionResult> RemoveCommentOrReview(int MovieId, int UserId, int EntityId, string CommentOrReview)
        {
            bool check4MovieId = await db.Movies.FirstOrDefaultAsync(movie => movie.Id == MovieId) != null;
            bool check4UserId = await db.Users.FirstOrDefaultAsync(user => user.Id == UserId) != null && User.FindFirst("Id").Value == UserId.ToString();
            CommentOrReview = CommentOrReview.ToLower().Trim();
            bool userAlreadyHaveReview = false;
            if (check4MovieId)
            {
                if (check4UserId)
                {
                    userAlreadyHaveReview = await db.Reviews.AnyAsync(rev => rev.UserId == UserId && rev.MovieId == MovieId);
                    if (CommentOrReview == "com")
                    {
                        Comment commentForRemove = await db.Comments.FirstOrDefaultAsync(com => com.Id == EntityId);
                        if (commentForRemove != null)
                        {
                            db.Comments.Remove(commentForRemove);
                            await db.SaveChangesAsync();
                            List<KeyValuePair<UserMinimalInfo, Comment>> Comments = new List<KeyValuePair<UserMinimalInfo, Comment>>();
                            foreach (Comment com in db.Comments.Where(c => c.MovieId == MovieId).ToList())
                            {
                                Comments.Add(new KeyValuePair<UserMinimalInfo, Comment>(db.Users.First(u => u.Id == com.UserId).GetUserMinimalInfo(), com));
                            }
                            double sumRating = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr.Rating).Sum();
                            int reviewsCount = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr).Count();
                            double resultRating = 0;
                            if (reviewsCount > 0 && sumRating > 0)
                            {
                                resultRating = sumRating / reviewsCount;
                            }
                            (await db.Movies.FirstAsync(mov => mov.Id == MovieId)).Rating = resultRating;
                            await db.SaveChangesAsync();
                            return Json(new
                            {
                                result = true,
                                rating = Math.Round(resultRating,2),
                                entityList = Comments,
                                haveReview = userAlreadyHaveReview,
                                message = "Ваш комментарий успешно удалён"
                            });
                        }
                        else
                            return Json(new
                            {
                                result = false,
                                message = "Id комментария для удаления не был найден." +
                                "\nВозможно, был изменён HTML-код." +
                                "\nПерезагрузите страницу и повторите попытку"
                            });
                    }
                    else if (CommentOrReview == "rev")
                    {
                        Review reviewForRemove = await db.Reviews.FirstOrDefaultAsync(rev => rev.Id == EntityId);
                        if (reviewForRemove != null)
                        {
                            db.Reviews.Remove(reviewForRemove);
                            await db.SaveChangesAsync();
                            List<KeyValuePair<UserMinimalInfo, Review>> Comments = new List<KeyValuePair<UserMinimalInfo, Review>>();
                            foreach (Review rev in db.Reviews.Where(c => c.MovieId == MovieId).ToList())
                            {
                                Comments.Add(new KeyValuePair<UserMinimalInfo, Review>(db.Users.First(u => u.Id == rev.UserId).GetUserMinimalInfo(), rev));
                            }
                            double sumRating = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr.Rating).Sum();
                            int reviewsCount = (from rr in db.Reviews.Where(rev => rev.MovieId == MovieId) select rr).Count();
                            double resultRating = 0;
                            if (reviewsCount > 0 && sumRating > 0)
                            {
                                resultRating = sumRating / reviewsCount;
                            }
                            (await db.Movies.FirstAsync(mov => mov.Id == MovieId)).Rating = resultRating;
                            await db.SaveChangesAsync();
                            userAlreadyHaveReview = await db.Reviews.AnyAsync(rev => rev.UserId == UserId && rev.MovieId == MovieId);
                            return Json(new
                            {
                                result = true,
                                rating = Math.Round(resultRating, 2),
                                entityList = Comments,
                                haveReview = false,
                                message = "Ваш отзыв успешно удалён"
                            });
                        }
                        else
                            return Json(new
                            {
                                result = false,
                                message = "Id отзыва для удаления не был найден." +
                                "\nВозможно, был изменён HTML-код." +
                                "\nПерезагрузите страницу и повторите попытку"
                            });
                    }
                    else
                    {
                        return Json(new
                        {
                            result = false,
                            message = "Неверный параметр 'CommentOrReview', перезагрузите страницу и повторите попытку"
                        });
                    }
                }
                else
                    return Json(new
                    {
                        result = false,
                        message = "Id пользователя, отправленный с запросом на удаление " + CommentOrReview == "rev" ? "отзыва" : "комментария" + " не совпадает с вашим Id." +
                        "\nЭто может быть вызвано изменением HTML-кода страницы." +
                        "\nПерезагрузите страницу и повторите попытку."
                    });
            }
            else
                return Json(new
                {
                    result = false,
                    message = "Неверный Id фильма(скорее всего изменён HTML-код)." +
                    "\nПерезагрузите страницу и повторите попытку"
                });
        }

        private bool MoviesExists(long id)
        {
            return db.Movies.Any(e => e.Id == id);
        }
    }
}
