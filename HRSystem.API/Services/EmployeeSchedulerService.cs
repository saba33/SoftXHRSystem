using Quartz;

namespace HRSystem.API.Services
{
    public class EmployeeSchedulerService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public EmployeeSchedulerService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task ScheduleActivationAsync(int employeeId)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey($"activation-job-{employeeId}");

            var job = JobBuilder.Create<HRSystem.Worker.EmployeeActivationJob>()
                .WithIdentity(jobKey)
                .UsingJobData("EmployeeId", employeeId)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"activation-trigger-{employeeId}")
                .StartAt(DateTimeOffset.UtcNow.AddSeconds(30))
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}