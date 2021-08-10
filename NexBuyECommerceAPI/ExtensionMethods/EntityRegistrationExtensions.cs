using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexBuyECommerceAPI.Interfaces;
using NexBuyECommerceAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexBuyECommerceAPI.ExtensionMethods
{
    public static class EntityRegistrationExtensions
    {

        public static void AddEntityServices(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient(typeof(AuthorQueries));
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IRentalService, RentalServices>();
            services.AddTransient<IResourceUtil, ResourceUtil>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        }
    }

}
}
