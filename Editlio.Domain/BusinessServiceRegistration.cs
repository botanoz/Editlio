using Editlio.Domain.Services.Abstracts;
using Editlio.Domain.Services.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace Editlio.Domain
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            // Business katmanında kullanılan servisleri burada ekleyin
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IRealTimeUpdateService, RealTimeUpdateService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
