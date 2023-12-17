using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entites.Identity;
using Talabat.Repository.Data.Contexts;
using Talabat.Repository.Identity.Contexts;

namespace Talabat.APIs
{

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webAppBuilder = WebApplication.CreateBuilder(args);

            #region Configure Services

            webAppBuilder.Services.AddControllers();
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //});

            webAppBuilder.Services.AddSwaggerServices();

            webAppBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webAppBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webAppBuilder.Services.AddDbContext<AppIdentityDbContext>(identityOptions =>
            {
                identityOptions.UseSqlServer(webAppBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webAppBuilder.Services.AddAppServices();

            webAppBuilder.Services.AddIdentityServices(webAppBuilder.Configuration);

            webAppBuilder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                return ConnectionMultiplexer.Connect(webAppBuilder.Configuration.GetConnectionString("Redis"));
            });

            webAppBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            #endregion Configure Services

            var app = webAppBuilder.Build();

            #region AutoMigrate

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<StoreContext>();
            var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await dbContext.Database.MigrateAsync();
                await identityDbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedDataAsync(dbContext);
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedAppUserDataAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an Error Occured During Applying Migration");
            }

            #endregion AutoMigrate

            #region Configure Kestrel MiddleWares

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pi peline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.MapControllers(); //replaces the use routing and use endpoint with map controller inside it

            app.UseAuthentication();

            app.UseAuthorization();

            #endregion Configure Kestrel MiddleWares

            app.Run();
        }
    }
}