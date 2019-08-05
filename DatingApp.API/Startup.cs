 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.api.Data;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.API.Helpers;

namespace DatingApp.API
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

            services.AddDbContext <DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddScoped<IAuthRepository , AuthRepository>(); // add singleton= one abject for the course. traisnt = each time request, scoped once per request     
          
          // here we add the part to athunticate the key or the request coming from the server and to make sure the token is vaild
          // also to didnt allow user to make their token from outside.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline. here order is importnat
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
             //   app.UseHsts(); // this method is for securiy for the app .  HTTPS 
            // here we can add the global expextion mode 
                 app.UseExceptionHandler(builder => {
                   builder.Run(async context =>{
                     context.Response.StatusCode= (int) HttpStatusCode.InternalServerError;
                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                      }
                   });


                 });
            }

            //app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // to allow access for the header. 
            app.UseAuthentication();
            app.UseMvc(); // this is the one we use, MCV for the rooting  
            // whihc the freamwkr that we use mildware that connect client request with netowrk 
            // it can be between the client req and api. also it rout req to the right contoller. 
        }
    }
}
 