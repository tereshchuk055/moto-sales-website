using MotoShop.Data;
using MotoShop.Repository;

namespace MotoShop.Services
{
    public static class ServiceProviderExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services) 
        {
            services.AddSingleton<DbContext>();
            services.AddScoped<BrandRepository>();
            services.AddScoped<ModelRepository>();
            services.AddScoped<TypeRepository>();
            services.AddScoped<CustomerRepository>();
            services.AddScoped<MotorcycleRepository>();
            services.AddScoped<OrderRepository>();
        }
    }
}
