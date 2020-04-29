using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StrongQuiz.Models.Models;
using StrongQuiz.Models.Repositories;
using StrongQuiz.Web.Data;


namespace StrongQuiz.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserScoreRepositories, UserScoreRepositories>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<StrongQuizDbContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
               options =>
               {
                   options.Cookie.SameSite = SameSiteMode.None;
                   options.Events =
                     new CookieAuthenticationEvents()
                     {
                         OnRedirectToLogin = (ctx) =>
                         {
                             if (ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == 200) //redirect naar loginURL is 200
                              {
                                  //doe geen redirect naar een loginpagina bij een api call 
                                  //maar geef een 401 (unauthorized) als authenticatie faalt 
                                  ctx.Response.StatusCode = 401;
                                 ctx.Response.WriteAsync("{\"error\": " + ctx.Response.StatusCode + " Geen toegang}");
                             }

                             return Task.CompletedTask;
                         }
                     };
               }
        );
            if (!env.IsDevelopment())
            {
                services.AddHttpsRedirection(options =>
                {
                    //default: 307 redirect
                    // options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(40); //default 30
                });
            }
            services.AddDbContext<StrongQuizDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "ToDo_API", Version = "v1.0" }); });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           else {          //In productie            
                app.UseExceptionHandler("/Error");           
            
            app.UseHsts(); 
 
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger(); //enable swagger 
            app.UseSwaggerUI(c => {
                c.RoutePrefix = "swagger"; //path naar de UI pagina: /swagger/index.html    
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "ToDo_API v1.0");
            });
        }
    }
}
