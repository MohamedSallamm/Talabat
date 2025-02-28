////using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Talabat.Core.Entities.Identity;
//using Talabat.Core.Services.Contract;
//using Talabat.Services;
////using Talabat.Core.Service;



//namespace Talabat.API.Extensions
//{
//    public static class IdentityServicesExtension
//    {
//        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration configuration)
//        {
//            Services.AddAuthentication();
//            Services.AddScoped(typeof(IAuthService), typeof(AuthService));
//            Services.AddIdentity<AppUser, IdentityRole>(Options =>
//            {
//                //Options.Password.RequiredDigit = true;
//            });

//            //    .AddEntityFrameworkStores<AppIdentityDbContext>();
//            //Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//            //    .AddJwtBearer(Options =>
//            //    {
//            //        Options.TokenValidationParameters = new TokenValidationParameters()
//            //        {
//            //            ValidateIssuer = true,
//            //            ValidIssuer = configuration["JWT:ValidIssuer"],
//            //            ValidateAudience = true,
//            //            ValidAudience = configuration["JWT:MySecureKey"],
//            //            ValidateLifetime = true,
//            //        };
//            //    });

//            //Services.AddScoped<ITokenService, TokenService>();
//            return Services;
//        }
//    }
//}
