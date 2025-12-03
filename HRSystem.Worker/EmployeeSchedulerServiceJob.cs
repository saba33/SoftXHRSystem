using HRSystem.Application.Interfaces.Services;
using Quartz;

namespace HRSystem.Worker
{
    public class EmployeeSchedulerServiceJob : IEmployeeSchedulerService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public EmployeeSchedulerServiceJob(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task ScheduleActivationAsync(int employeeId)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey($"activation-job-{employeeId}");

            var job = JobBuilder.Create<EmployeeActivationJob>()
                .WithIdentity(jobKey)
                .UsingJobData("EmployeeId", employeeId)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"activation-trigger-{employeeId}")
                .StartAt(DateTimeOffset.UtcNow.AddHours(1))
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
