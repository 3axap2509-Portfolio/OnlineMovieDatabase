using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading;
using System.Security;
using OnlineMovieDatabase.Models;
using System.Text;

namespace OnlineMovieDatabase.Helpers
{
    public class MailKitService
    {
        private static readonly string AuthLogin = "OmdbSeller@yandex.by";
        private static readonly string AuthPassword = "bgQ-X9w-w2d-EnQ";

        private static readonly string ConfirmEmailSubject = "Подтверждение регистрации";
        private readonly OMDB_Context db;
        public MailKitService(OMDB_Context omdb)
        {
            client = new SmtpClient();
            try
            {
                db = omdb;
                client.Connect("smtp.yandex.com", 25, false);
                client.Authenticate(AuthLogin, AuthPassword);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + " " + e.InnerException?.Message);
            }
        }
        SmtpClient client;

        public async Task SendEmailVerificationToUser(string _сonfirmUrl,User _user)
        {
            // приводим URL подтверждения в корректный формат:
            _сonfirmUrl = _сonfirmUrl.Trim();
            _сonfirmUrl += _сonfirmUrl.EndsWith('/') ? String.Empty : "/";
            StringBuilder sb = new StringBuilder();
            sb.Append("Здравствуйте, для завершения регистрации необходимо подтвердить Email адрес.<br>");
            sb.Append("Для этого перейдите по ");
            sb.Append($"<a href='{_сonfirmUrl + _user.GetHexHashCodeString()}'>ссылке</a><br>");
            sb.Append("Если вы не регистрировались на данном сервисе, пожалуйста, проигнорируйте это письмо.");
            await SendEmailAsync(_user.UserName, _user.EmailAddress, ConfirmEmailSubject, sb.ToString());
        }
        public async Task SendEmailAsync(string userName, string userEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Online Movie Database", "OmdbSeller@yandex.by"));
            emailMessage.To.Add(new MailboxAddress(userName, userEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            await client.SendAsync(emailMessage);
        }

        public async Task<int> SendUpdateNews(object o)
        {
            if (o is Movie)
            {
                Movie m = o as Movie;
                List<int> GenresId = (from g in db.MoviesGenres where g.MovieId == m.Id select g.GenreId).ToList();
                string tempGen;
                for(int i = 0; i < GenresId.Count; i++)
                {
                    tempGen = db.Genres.First(g => g.Id == GenresId[i]).RuGenreName;
                    m.Genres += i == 0 ? (tempGen.Substring(0, 1).ToUpper() + tempGen.Substring(1)) : (',' + tempGen.ToLower());
                }
                string subject = "Добавлен новый фильм";
                StringBuilder sb = new StringBuilder();
                sb.Append("Здравствуйте, на сайте <a href='http://onlinemoviedatabase.azurewebsites.net'>OnlineMovieDatabase</a> появился новый фильм!.<br>");
                sb.Append("<h3>Краткая информация:</h3>");
                sb.Append($"Название:{m.RuTitle}<br/>");
                sb.Append($"Год выхода на экраны:{m.ReleaseYear}<br/>");
                sb.Append($"Жанр:{m.Genres}<br/>");
                sb.Append($"Подробнее можно посмотреть по <a href='http://onlinemoviedatabase.azurewebsites.net/Titles/{m.Id}'>ссылке</a>");

                int usersCount = 0;
                foreach (User u in db.Users.Where(usr => usr.NeedToNotificate))
                {
                    await SendEmailAsync(u.UserName, u.EmailAddress, subject, sb.ToString());
                    usersCount += 1;
                }
                return usersCount;
            }
            else if (o is Genre)
            {
                Genre g = o as Genre;
                string subject = "Добавлен новый жанр";
                StringBuilder sb = new StringBuilder();
                sb.Append("Здравствуйте, на сайте <a href='http://onlinemoviedatabase.azurewebsites.net'>OnlineMovieDatabase</a> появился новый жанр!.<br>");
                sb.Append("<h3>Краткая информация:</h3>");
                sb.Append($"Жанр:{g.RuGenreName}<br/>");
                sb.Append($"<a href='http://onlinemoviedatabase.azurewebsites.net/Genres'>Список всех жанров</a>");

                int usersCount = 0;
                foreach (User u in db.Users.Where(usr => usr.NeedToNotificate))
                {
                    await SendEmailAsync(u.UserName, u.EmailAddress, subject, sb.ToString());
                    usersCount += 1;
                }
                return usersCount;
            }
            else
                throw new Exception("Invalid Object Parameter");
        }

        public async Task<int> SendUserBan(User u, DateTime stopblock, string message = null)
        {
            if (u.NeedToNotificate && !string.IsNullOrEmpty(u.EmailAddress) && !u.EmailAddress.EndsWith("@omdb.com"))
            {
                string subject = "Вы заблокированы";
                StringBuilder sb = new StringBuilder();
                sb.Append($"Здравствуйте, {u.UserName}<br/>");
                sb.Append($"К сожалению, Вы были заблокированы на <a href='http://onlinemoviedatabase.azurewebsites.net'>нашем ресурсе</a><br/>.");
                if (message == null)
                    sb.Append($"Сообщение администратора{message}<br/>");
                else
                    sb.Append("Администратор не указал причину блокировки<br/>");
                sb.Append($"Дата истечения блокировки:{stopblock.ToShortDateString()}<br/>");
                sb.Append("Вы по прежнему можете продолжать пользоваться ресурсом на анонимной основе<br/>");
                await SendEmailAsync(u.UserName, u.EmailAddress, subject, sb.ToString());
                return 1;
            }
            return 0;
        }
        public async Task<int> SendUserUnBan(User u)
        {
            if (u.NeedToNotificate && !string.IsNullOrEmpty(u.EmailAddress) && !u.EmailAddress.EndsWith("@omdb.com"))
            {
                string subject = "Вы разблокированы";
                StringBuilder sb = new StringBuilder();
                sb.Append($"Здравствуйте, {u.UserName}<br/>");
                sb.Append($"Ваша блокировка на <a href='http://onlinemoviedatabase.azurewebsites.net'>нашем ресурсе</a> была снята!<br/>");
                sb.Append($"Теперь вам доступна <a href='http://onlinemoviedatabase.azurewebsites.net/Account/Login'>авторизация</a><br/>");
                await SendEmailAsync(u.UserName, u.EmailAddress, subject, sb.ToString());
                return 1;
            }
            return 0;
        }
    }
}
