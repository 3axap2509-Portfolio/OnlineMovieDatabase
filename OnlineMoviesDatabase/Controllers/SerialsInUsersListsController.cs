using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Models;

namespace OnlineMovieDatabase.Controllers
{
    public class MoviesInUsersListsController : Controller
    {
        private readonly OMDB_Context db;

        public MoviesInUsersListsController(OMDB_Context context)
        {
            db = context;
        }

        //[HttpGet, Route("/SerialInUserList/Delete")]
        //[Authorize]
        //public async Task<string> Delete(long? serialId, long? userId)
        //{
        //    if (serialId == null || userId == null)
        //    {
        //        return "1 из переданных Id оказался null";
        //    }

        //    var serialInUserList = await db.MoviesInUsersLists.FirstOrDefaultAsync(m => m.UserId == userId && m.SerialId == serialId);
        //    if (serialInUserList == null)
        //    {
        //        return "Данный сериал не содержится в списке пользователя";
        //    }
        //    db.MoviesInUsersLists.Remove(serialInUserList);
        //    await db.SaveChangesAsync();
        //    List<MoviesInUsersLists> buf = db.MoviesInUsersLists.Where(el => el.SerialId == serialId && el.Rating.HasValue).ToList();
        //    double sumRating = 0;
        //    foreach (MoviesInUsersLists sss in buf)
        //    {
        //        sumRating += sss.Rating.HasValue ? sss.Rating.Value : 0;
        //    }
        //    sumRating /= buf.Count > 0 ? buf.Count : 1;
        //    Serial srl = db.Movies.FirstOrDefaultAsync(el => el.Id == serialId);
        //    srl.Rating = sumRating;
        //    db.Movies.Update(srl);

        //    await db.SaveChangesAsync();
        //    return "ок";
        //}


        //[HttpGet, Route("Titles/{serialId}/AddOrEditInUserList")]
        //[Authorize]
        //public async Task<string> AddOrEditInUserList(long serialId, string status)
        //{
        //    User u = db.Users.FirstOrDefaultAsync(el => el.Nickname == User.Identity.Name);
        //    if (u != null)
        //    {
        //        MoviesInUsersLists sius = db.MoviesInUsersLists.FirstOrDefaultAsync(el => el.SerialId == serialId && el.UserId == u.Id);
        //        if (sius != null)
        //        {
        //            sius.Status = status;
        //            if (status == Moviestatus.Planing)
        //            {
        //                sius.Rating = null;
        //                sius.Review = null;
        //                List<MoviesInUsersLists> buf = db.MoviesInUsersLists.Where(el => el.SerialId == serialId && el.Rating.HasValue).ToList();
        //                double sumRating = 0;
        //                foreach (MoviesInUsersLists sss in buf)
        //                {
        //                    sumRating += sss.Rating.HasValue ? sss.Rating.Value : 0;
        //                }
        //                sumRating /= buf.Count > 0 ? buf.Count : 1;
        //                Serial srl = db.Movies.FirstOrDefaultAsync(el => el.Id == serialId);
        //                srl.Rating = sumRating;
        //                db.Movies.Update(srl);
        //            }
        //            db.MoviesInUsersLists.Update(sius);
        //            await db.SaveChangesAsync();
        //            return $" Сериал:{db.Movies.FirstOrDefaultAsync(el => el.Id == serialId).RuTitle}\n перенесен в список \"{status}\"";
        //        }
        //        else
        //        {
        //            db.MoviesInUsersLists.Add(new MoviesInUsersLists()
        //            {
        //                SerialId = serialId,
        //                UserId = u.Id,
        //                Status = status
        //            });
        //            db.SaveChanges();
        //            return $" Сериал:{db.Movies.FirstOrDefaultAsync(el => el.Id == serialId).RuTitle}\n теперь у вас в списке  \"{status}\"";
        //        }
        //    }
        //    else
        //    {
        //        return "Вы не авторизваны";
        //    }
        //}

        //[HttpGet, Route("Titles/{serialId}/EditSerialRatingInUserList")]
        //[Authorize]
        //public async Task<string> EditSerialRatingInUserList(long serialId, byte rating)
        //{
        //    Serial srl = await db.Movies.FirstOrDefaultAsync(el => el.Id == serialId);
        //    if (srl != null)
        //    {
        //        User u = await db.Users.FirstOrDefaultAsync(el => el.Nickname == User.Identity.Name);
        //        if (u != null)
        //        {
        //            MoviesInUsersLists sius = await db.MoviesInUsersLists.FirstOrDefaultAsync(el => el.SerialId == serialId && el.UserId == u.Id);
        //            if (sius != null)
        //            {
        //                if (sius.Status != Moviestatus.Planing)
        //                {
        //                    sius.Rating = rating;
        //                    db.MoviesInUsersLists.Update(sius);
        //                    await db.SaveChangesAsync();
        //                    List<MoviesInUsersLists> buf = db.MoviesInUsersLists.Where(el => el.SerialId == serialId && el.Rating != null).ToList();
        //                    double sumRating = 0;
        //                    foreach (MoviesInUsersLists sss in buf)
        //                    {
        //                        sumRating += sss.Rating.Value;
        //                    }
        //                    sumRating /= buf.Count > 0 ? buf.Count : 1;
        //                    srl.Rating = sumRating;
        //                    db.Movies.Update(srl);
        //                    await db.SaveChangesAsync();
        //                    return "Ваша оценка успешно установлена/изменена!";
        //                }
        //                else
        //                    return "Нельзя дать оценку сериала в списке \"Запланировано\"";
        //            }
        //            else
        //                return "Для выставления оценки сериалу, необходимо добавить его в 1 из ваших списков!";
        //        }
        //        else
        //            return "Вы не авторизованы!";
        //    }
        //    else
        //        return "Что-то пошло не так(неверный Id сериала)";
        //}

        //[HttpPost, Route("Titles/{serialId}/EditSerialReviewInUserList")]
        //[Authorize]
        //public async Task<string> EditSerialReviewInUserList(long serialId, string review)
        //{
        //    Serial srl = await db.Movies.FirstOrDefaultAsync(el => el.Id == serialId);
        //    if (srl != null)
        //    {
        //        User u = await db.Users.FirstOrDefaultAsync(el => el.Nickname == User.Identity.Name);
        //        if (u != null)
        //        {
        //            MoviesInUsersLists sius = await db.MoviesInUsersLists.FirstOrDefaultAsync(el => el.SerialId == serialId && el.UserId == u.Id);
        //            if (sius != null)
        //            {
        //                if (sius.Status != Moviestatus.Planing)
        //                {
        //                    sius.Review = string.IsNullOrEmpty(review) ? "" : review;
        //                    if (string.IsNullOrEmpty(review))
        //                    {
        //                        return "Введенный комментарий пуст";
        //                    }
        //                    else
        //                    {
        //                        db.MoviesInUsersLists.Update(sius);
        //                        await db.SaveChangesAsync();
        //                    }

        //                    return "Ваш комментарий успешно добавлен/изменён";
        //                }
        //                else
        //                    return ("Нельзя оставить отзыв на сериал в списке \"Запланировано\"");
        //            }
        //            else
        //                return "Нельзя оставить отзыв на сериал, которого нет ни в 1 из ваших списков";
        //        }
        //        else
        //            return "Вы не авторизованы, для оставления отзыва авторизуйтесь";
        //    }
        //    else
        //        return "Что-то пошло не так(По заданному айди сериал не обнаружен. Возможно, он только что был удален из базы данных\nПерезагрузите страницу и повторите попытку)";
        //}

    }
}
