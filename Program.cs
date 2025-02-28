using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Extensions;
using Talabat.API.Middlewares;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Services;

namespace Talabat.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddAutoMapper(M => M.AddProfile(MappingProfiles);
            // Configure Database Contexts
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Configure Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
                     ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            ;
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
            // Configure Identity and Authentication
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            //builder.Services.AddAuthentication();
            //builder.Services.AddAuthorization();
            #endregion

            var app = builder.Build();

            #region Update Database
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DBContext = services.GetRequiredService<StoreContext>();
                await DBContext.Database.MigrateAsync(); // Update Database

                var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityContext.Database.MigrateAsync(); // Update Identity Database

                await StoreContextSeed.SeedASync(DBContext);
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                await StoreContextSeed.SeedASync(DBContext);
            }

            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during database migration.");
            }
            #endregion

            #region Configure the HTTP Request Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>(); // Exception middleware in development
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            }

            else
            {
                app.UseMiddleware<ExceptionMiddleWare>(); // Ensure this is added for production too, if necessary
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting(); // Ensure routing is enabled before authentication and authorization

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
