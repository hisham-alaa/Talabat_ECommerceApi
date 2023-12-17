using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Reporitories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Repositories.Implementation;
using Talabat.Service;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            services.AddScoped(typeof(IProductService), typeof(ProductService));

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Where(ModelStateValue => ModelStateValue.Value?.Errors.Count > 0)
                                                  .SelectMany(ModelStateValue => ModelStateValue.Value.Errors)
                                                  .Select(Errors => Errors.ErrorMessage)
                                                  .ToArray();

                    return new BadRequestObjectResult(new ApiValidationErrorResponse(errors));
                };
            });

            return services;
        }
    }
}
