using Quartz;

namespace HRSystem.API.Extensions
{
    public static class AddQuartzJobsExtension
    {
        public static IServiceCollection AddQuartzWorker(this IServiceCollection services)
        {
            services.AddQuartz(q => { });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}