using Editlio.Infrastructure.Context;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Infrastructure.Repositories.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Editlio.Infrastructure
{
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
        {
            // DbContext registration
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(connectionString));

            // Repository registrations
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IRealTimeUpdateRepository, RealTimeUpdateRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
