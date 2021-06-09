using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snackis.Models;
using Snackis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Snackis.Pages.ThreadViewModel;

namespace Snackis
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

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<PostFormService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy",
                     policy => policy.RequireRole("Admin"));
            });
            services.AddRazorPages(options =>
            {
                //options.Conventions.AuthorizePage("/Privacy","MustBeUser");
                options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
                options.Conventions.AllowAnonymousToPage("/Admin/Info");
                options.Conventions.AllowAnonymousToFolder("/Admin/PublicAdmin");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();//Lägg till, glöm ej!!!

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
