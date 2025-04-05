namespace Talabat.API.Extensions
{
    public static class SwaggerServices
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {
            Services.AddControllers();
            Services.AddEndpointsApiExplorer();
            return Services;
        }
        
    }
}
