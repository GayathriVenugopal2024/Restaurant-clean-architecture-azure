using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions
{
    public static class ServiceCollectionExtenstion
    {
        public static void AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            var RestaurantdbConnectionString = configuration.GetConnectionString("RestaurantsDB");
            var AuthenticationdbConnectionString = configuration.GetConnectionString("AuthenticationDB");
            services.AddDbContext<RestaurantsDbContext>(option=>option.UseSqlServer(RestaurantdbConnectionString).EnableSensitiveDataLogging());
            services.AddDbContext<AuthenticationDbContext>(option => option.UseSqlServer(AuthenticationdbConnectionString));
             
            

            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantsRepository, RestaurantsRepositories>();
            services.AddScoped<IDishesRepository, DishesRepository>();
        }
    }
}
