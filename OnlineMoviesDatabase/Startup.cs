using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineMovieDatabase.Models;
using Microsoft.EntityFrameworkCore;
using OnlineMovieDatabase.Helpers;
using System.Threading;
using System;
using OnlineMovieDatabase.Controllers;
using System.Threading.Tasks;

namespace OnlineMovieDatabase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<MailKitService>();
            services.AddScoped<OauthManager>();
            services.AddHttpContextAccessor();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
           .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<OMDB_Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("OMDB"));
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, OMDB_Context db, IHttpContextAccessor hca)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/StatusCodes/Index", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Use(async (context, next) =>
            //{
            //    if (context.User.Identity.IsAuthenticated)
            //    {
            //        context.Response.Redirect("../Account/Login");
            //        string sId = context.User.FindFirst("Id").Value;
            //        User checkUser = await db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == sId);
            //        if (checkUser.IsBanned || checkUser.BannedFor.Value > DateTime.Now)
            //        {
            //            hca.HttpContext.Response.Redirect("../Account/Login");
            //        }
            //    }
            //    await next.Invoke();
            //});
        }
    }
}
