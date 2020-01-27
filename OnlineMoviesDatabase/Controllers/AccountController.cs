using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Models;
using OnlineMovieDatabase.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using System.Security.Claims;
using OnlineMovieDatabase.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiniGuids;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Drawing;
using System.Net.Http.Headers;

namespace OnlineMovieDatabase.Controllers
{

    public class AccountController : Controller
    {
        //constructor
        public AccountController(OMDB_Context _c, IHostingEnvironment _ae, MailKitService _mks, OauthManager _om)
        {
            db = _c;
            appEnvironment = _ae;
            mailService = _mks;
            oauthManager = _om;
        }
        //Dependency injections
        private readonly MailKitService mailService;
        private readonly OMDB_Context db;
        private readonly IHostingEnvironment appEnvironment;
        private readonly OauthManager oauthManager;
        //methods
        public static User CreateStandartUser(string _userName, string _password, string _email)
        {
            return new User()
            {
                UserName = _userName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(_password),
                EmailAddress = _email,
                UserRole = (byte)UserRoleEnum.User,
                IsConfirmed = false,
                IsBanned = false,
                OauthAuthorized = false,
                BannedFor = null,
                OauthServiceName = null,
                OauthSocialId = null
            };
        }

        [HttpGet]
        public IActionResult Login()
        {
            //await mailService.SendEmailAsync("3axap", "3axap2509@gmail.com", "login try", "hello, test");
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Account");
            else
                return View(new LoginViewModel());
        }

        public IActionResult AuthWith(string ServiceName)
        {
            string HostUri = Request.Scheme + "://" + Request.Host + this.Url.Action("ExtAuth", "Account");
            switch (ServiceName)
            {
                case "Vk":
                    {
                        return Redirect(oauthManager.VkOauthHelper.GetOauthHref(HostUri));
                    }
                case "Github":
                    {
                        return Redirect(oauthManager.GithubOauthHelper.GetOauthHref(HostUri));
                    }
                    //case "Google":
                    //    {
                    //        return Redirect(oauthManager.GoogleOauthHelper.GetOauthHref(HostUri));
                    //    }
            }
            return new StatusCodeResult(404);
        }

        [HttpGet, Route("Account/ExtAuth/{serviceName}")]
        public async Task<IActionResult> ExtAuth(string serviceName, string code, string state, string access_token, string user_id, string email)
        {
            User newUser;
            Image newUserAvatar = null;
            bool UserAccepted= false;
            string HostUri = Request.Scheme + "://" + Request.Host + this.Url.Action("ExtAuth", "Account");
            switch (serviceName.ToLower())
            {
                case "vk":
                    {
                        if(string.IsNullOrEmpty(access_token))
                            return View(model: "redirect_from_vk");
                        UserAccepted = oauthManager.VkOauthHelper.GetOauthUser(Convert.ToInt32(user_id), null, access_token, out newUser, out newUserAvatar);
                        break;
                    }
                case "github":
                    {
                        UserAccepted = oauthManager.GithubOauthHelper.ResumeOauthWithCode(HostUri, code, state, out newUser, out newUserAvatar);
                        break;
                    }
                default: return new StatusCodeResult(404);
            }
            if(UserAccepted)
            {
                User isUserAlredyExist = await db.Users.FirstOrDefaultAsync(u => u.OauthServiceName == newUser.OauthServiceName && u.OauthSocialId == newUser.OauthSocialId);
                if(isUserAlredyExist != null)
                {
                    if(isUserAlredyExist.IsBanned)
                    {
                        return new StatusCodeResult(403);
                    }
                    await Authenticate(isUserAlredyExist);
                    return RedirectToAction("Index", "Home");
                }
                newUser.EmailAddress = newUser.OauthServiceName + newUser.OauthSocialId.ToString() + "@omdb.com";
                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();
                if (newUserAvatar != null)
                    newUserAvatar.Save(appEnvironment.WebRootPath + "/static/images/UsersAvatars/" + newUser.Id.ToString() + ".jpg");
                await Authenticate(newUser);
                return RedirectToAction("Index", "Home");
            }
            else
                throw new Exception($"Can't register via Oauth-service({serviceName})");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.EmailAddress == model.Email);
                if (user != null)
                {
                    if (user.IsBanned)
                    {
                        if (user.BannedFor.Value > DateTime.Now)
                        {
                            ModelState.ClearValidationState("Email");
                            ModelState.ClearValidationState("Password");
                            ModelState.AddModelError("Email", $"Данный аккаунт заблокирован до {user.BannedFor.Value.ToShortDateString()}");
                            return View(model);
                        }
                        {
                            if (!user.OauthAuthorized)
                                if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                                    if (user.IsConfirmed.HasValue)
                                        if (user.IsConfirmed.Value)
                                        {
                                            await Authenticate(user);
                                            return RedirectToAction("Index", "Home");
                                        }
                                        else
                                            ModelState.AddModelError("Email", $"Для авторизации необходимо подтвердить e-mail адрес, проверьте почтовый ящик");
                                    else
                                        ModelState.AddModelError("Password", $"Неверный пароль");
                                else
                                    return new StatusCodeResult(301);
                        }
                    }
                    else
                    {
                        if (!user.OauthAuthorized)
                            if (BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                                if (user.IsConfirmed.HasValue)
                                    if (user.IsConfirmed.Value)
                                    {
                                        await Authenticate(user);
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                        ModelState.AddModelError("Email", $"Для авторизации необходимо подтвердить e-mail адрес, проверьте почтовый ящик");
                                else
                                    ModelState.AddModelError("Password", $"Неверный пароль");
                                else
                                    ModelState.AddModelError("Password", $"Неверный пароль");

                    }
                }
                else
                {
                    ModelState.AddModelError("Email", $"Пользователь с адресом электронной почты '{model.Email}' не существует, зарегистрируйтесь");
                }
                return View(model);
            }
            else
            {
                ModelState.ClearValidationState("Email");
                ModelState.ClearValidationState("Password");
                if (string.IsNullOrEmpty(model.Email))
                    ModelState.AddModelError("Email", "Введите адрес электронной почты");

                if (string.IsNullOrEmpty(model.Password))
                    ModelState.AddModelError("Password", "Введите пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.EmailAddress == model.Email);
                if (user == null)
                {
                    if (model.Name.Length < 3 || model.Name.Length > 20)
                    {
                        ModelState.AddModelError("Name", "Длина имени должна быть в диапазоне 3-20 символов");
                        return View(model);
                    }
                    if (model.Password.Length < 8 || model.Password.Length > 20)
                    {
                        ModelState.AddModelError("Password", "Длина пароля должна быть в диапазоне 8-20 символов");
                        return View(model);
                    }
                    User usr = CreateStandartUser(model.Name, model.Password, model.Email);
                    await db.Users.AddAsync(usr);
                    string fullUrl = this.Url.Action("ConfirmEmail", "Account", null, Request.Scheme);
                    await mailService.SendEmailVerificationToUser(fullUrl, usr);
                    await db.SaveChangesAsync();
                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                else
                    ModelState.AddModelError("Email", "Аккаунт с таким адресом электронной почты уже зарегистрирован");
            }
            else
            {
                ModelState.ClearValidationState("Name");
                ModelState.ClearValidationState("Password");
                ModelState.ClearValidationState("PasswordConfirm");
                if (String.IsNullOrEmpty(model.Name))
                    ModelState.AddModelError("Name", "Имя пользователя не введено");
                if (String.IsNullOrEmpty(model.Password))
                    ModelState.AddModelError("Password", "Пароль не введен");
                if (!String.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.PasswordConfirm))
                    ModelState.AddModelError("PasswordConfirm", "Повтор пароля не введен");
                else
                {
                    if(model.Password != model.PasswordConfirm)
                    {
                        ModelState.AddModelError("PasswordConfirm", "Пароли не совпадают");
                        ModelState.AddModelError("Password", "Пароли не совпадают");
                    }
                }
            }
            return View(model);
        }
        private async Task Authenticate(User _user)
        {
            User authUser = await db.Users.FirstOrDefaultAsync(u => u.EmailAddress == _user.EmailAddress);
            if (authUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Email", authUser.EmailAddress),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authUser.UserName),
                    new Claim("Id", authUser.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, authUser.RoleName.ToLower())
                };
                ClaimsIdentity id = new ClaimsIdentity(claims,
                                                       "ApplicationCookie",
                                                       ClaimsIdentity.DefaultNameClaimType,
                                                       ClaimsIdentity.DefaultRoleClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            else
                return new StatusCodeResult(404);
        }

        [HttpGet, Route("Users/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                string CurrentUserId = string.Empty;
                Claim claimForCheck = User.Claims.FirstOrDefault(cl => cl.Type.ToLower() == "id");
                if (claimForCheck != null)
                {
                    CurrentUserId = claimForCheck.Value;
                    User user = await db.Users.FirstOrDefaultAsync(el => el.Id == id);

                    if (user != null)
                    {
                        if (user.Id.ToString() == CurrentUserId)
                            return RedirectToAction("Index", "Account");
                    }
                    else
                        return StatusCode(404);
                }
            }
            User u = await db.Users.FirstOrDefaultAsync(el => el.Id == id);
            if (u != null)
            {
                int commentsCounter = db.Comments.Where(com => com.UserId == u.Id).Count();
                int reviewsCounter = db.Reviews.Where(com => com.UserId == u.Id).Count();

                AccountDetailsViewModels res = new AccountDetailsViewModels(u, reviewsCounter, commentsCounter);
                return View(res);
            }
            else
                return StatusCode(404);
        }

        [HttpGet, Route("Account/ConfirmEmail/{confirmCode}")]
        public async Task<IActionResult> ConfirmEmail(string confirmCode)
        {
            confirmCode = confirmCode.Trim();
            if (confirmCode.Contains("|") && confirmCode.Split("|").Length == 2 && !confirmCode.StartsWith("|") && !confirmCode.EndsWith("|"))
            {
                string[] values = confirmCode.Split("|");
                User userForConfirmEmail = await db.Users.FirstOrDefaultAsync(u => u.UserName == values[0]);
                if(userForConfirmEmail != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(userForConfirmEmail.GetStringForVerify(), values[1].Replace("~~", "/")))
                    {
                        userForConfirmEmail.ConfirmEmail();
                        await db.SaveChangesAsync();
                        return View();
                    }
                 }
            }
            return new StatusCodeResult(404);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //AccountDetailsViewModels res = new AccountDetailsViewModels();

            string CurrentUserId = string.Empty;
            Claim claimForCheck = User.Claims.FirstOrDefault(cl => cl.Type.ToLower() == "id");
            if (claimForCheck == null)
                throw new Exception("Claims problems on server");
            CurrentUserId = claimForCheck.Value;
            User u = await db.Users.FirstOrDefaultAsync(el => el.Id.ToString() == CurrentUserId);

            int commentsCounter = db.Comments.Where(com => com.UserId == u.Id).Count();
            int reviewsCounter = db.Reviews.Where(com => com.UserId == u.Id).Count();
            if (u != null)
            {
                AccountDetailsViewModels vm = new AccountDetailsViewModels(u, reviewsCounter, commentsCounter);
                return View(vm);
            }
            else
                return RedirectToAction("Login", "Account");
        }
        [HttpGet, Authorize/*, Route("Account/Edit")*/]
        public async Task<IActionResult> Edit()
        {
            int CurrentUserId = -1;
            Claim claimForCheck = User.Claims.FirstOrDefault(cl => cl.Type.ToLower() == "id");
            if (claimForCheck == null)
                throw new Exception("Claims problems on client side");
            CurrentUserId = int.Parse(claimForCheck.Value);
            User u = await db.Users.FirstAsync(el => el.Id == CurrentUserId);
            UserEditViewModel model = new UserEditViewModel();
            model.user = u;
            model.Name = u.UserName;
            return View(model);
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Edit(UserEditViewModel model, IFormFile newProfileAvatar = null)
        {
            string sId = User.FindFirst("Id").Value;
            User editEntity = await db.Users.FirstOrDefaultAsync(el => el.Id == model.user.Id);
            if(editEntity != null)
            {
                if (editEntity.Id.ToString() == sId)
                {
                    ModelState.ClearValidationState("newPassword");
                    ModelState.ClearValidationState("newPasswordRepeat");
                    ModelState.ClearValidationState("Name");
                    if (!(model.Name.Length <= 20 && model.Name.Length > 3))
                    {
                        ModelState.AddModelError("Name", "Длина имени должна быть в диапазоне 3-20 символов");
                        return View(model);
                    }
                    editEntity.UserName = model.Name;
                    editEntity.NeedToNotificate = model.NeedToNotify;
                    if (!string.IsNullOrEmpty(model.currentPassword))
                    {
                        if (BCrypt.Net.BCrypt.Verify(model.currentPassword, editEntity.PasswordHash))
                        {
                            if (model.newPassword == model.newPasswordRepeat)
                            {
                                if (model.newPassword.Length <= 20 && model.newPassword.Length >= 8)
                                    editEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.newPassword);
                                else
                                {
                                    ModelState.AddModelError("newPassword", "Длина нового пароля должна быть в диапазоне 8-20 символов");
                                    ModelState.AddModelError("newPasswordRepeat", "Длина нового пароля должна быть в диапазоне 8-20 символов");
                                    return View(model);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("newPassword", "Пароли должны совпадать");
                                ModelState.AddModelError("newPasswordRepeat", "Пароли должны совпадать");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.ClearValidationState("currentPassword");
                            ModelState.AddModelError("currentPassword", "Неверный пароль");
                            return View(model);
                        }
                    }
                    db.Users.Update(editEntity);
                    await db.SaveChangesAsync();
                    var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Name)
                };
                    if (editEntity.RoleName == Models.User.AdminRole)
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
                    claims.Add(new Claim("Id", editEntity.Id.ToString()));
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsIdentity idCI = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(idCI));
                    if (newProfileAvatar != null)
                    {
                        string path = "/static/images/UsersAvatars/" + editEntity.Id.ToString() + ".jpg";
                        using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await newProfileAvatar.CopyToAsync(fileStream);
                        }
                    }
                }
                else
                    return StatusCode(404);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            List<User> users = await db.Users.ToListAsync();
            return View(users);
        }
        [HttpGet, Route("/Users/{userId}/Ban")]
        public async Task<string> Ban(int? userId, string bannedFor, string reason )
        {
            if(userId.HasValue && !string.IsNullOrEmpty(bannedFor))
            {
                DateTime ban4 = DateTime.Parse(bannedFor);
                User u = await db.Users.FirstOrDefaultAsync(us => us.Id == userId);
                if(u != null)
                {
                    u.IsBanned = true;
                    u.BannedFor = ban4;
                    db.Users.Update(u);
                    await db.SaveChangesAsync();
                    await mailService.SendUserBan(u, ban4, reason);
                    return "ок";
                }
                else
                    return "Указанный идентификатор пользователя не найден";
            }
            else
                return "Дата не распознана";
        }
        [HttpGet, Route("/Users/{userId}/Unban")]
        public async Task<string> Unban(long? userId)
        {
            if (userId.HasValue)
            {
                User u = await db.Users.FirstOrDefaultAsync(us => us.Id == userId);
                if (u != null)
                {
                    u.IsBanned = false;
                    u.BannedFor = null;
                    db.Users.Update(u);
                    await db.SaveChangesAsync();
                    await mailService.SendUserUnBan(u);
                    return "ок";
                }
                else
                    return "Указанный идентификатор пользователя не найден";
            }
            else
                return "Идентификатор пользователя не распознан";
        }
    }
}