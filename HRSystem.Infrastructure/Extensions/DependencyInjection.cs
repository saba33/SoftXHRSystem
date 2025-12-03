using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Application.Mapping;
using HRSystem.Application.Services;
using HRSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HRSystem.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddAutoMapper(typeof(EmployeeProfile).Assembly);
            return services;
        }
    }
}
