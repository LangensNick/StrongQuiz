using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using StrongQuiz.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrongQuiz.Models.Models;
using StrongQuiz.Models.Repositories;
using StrongQuiz.Models.Data;

namespace StrongQuiz.Web
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
            services.AddScoped<IUserScoreRepositories, UserScoreRepositories>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddDbContext<StrongQuizDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StrongQuizDbContext>();
            services.AddControllersWithViews();
            services.AddAuthorization(options => { options.AddPolicy("SuperUsersRole", policy => policy.RequireRole("Administrator", "COördinator")); });
            services.AddRazorPages();
            services.Configure<IdentityOptions>(options => { options.Password.RequiredLength = 8; options.User.RequireUniqueEmail = true; options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); });
            services.ConfigureApplicationCookie(options => { options.Cookie.HttpOnly = true; options.ExpireTimeSpan = TimeSpan.FromMinutes(120); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //StrongQuizDbContextExtensions.SeedRoles(roleMgr).Wait();
            //StrongQuizDbContextExtensions.SeedUsers(userMgr).Wait();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
