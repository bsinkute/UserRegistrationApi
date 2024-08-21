using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserRegistrationApi.Infrastructure.Database;
using UserRegistrationApi.Infrastructure.Repositories;

namespace UserRegistrationApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<UserRegistrationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IUserRepository, UserReposotory>();
            return services;
        }
    }
}
