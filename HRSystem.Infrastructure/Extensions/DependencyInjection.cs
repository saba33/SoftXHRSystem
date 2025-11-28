using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Application.Services;
using HRSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
