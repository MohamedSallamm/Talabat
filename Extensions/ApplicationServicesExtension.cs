using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            // Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IUnitOfWork<BaseEntity>), typeof(UnitOfWork<BaseEntity>));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IProductService), typeof(ProductService));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));
            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    //Model state >> Dictionary [Key value pair]
                    // key >> Name of parameter
                    //value >> Error
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                           .SelectMany(P => P.Value.Errors)
                                                           .Select(E => E.ErrorMessage)
                                                           .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            #endregion
            return Services;
        }
    }
}
