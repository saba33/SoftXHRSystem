using HRSystem.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HRSystem.Worker
{
    [DisallowConcurrentExecution]
    public class EmployeeActivationJob : IJob
    {
        private readonly ILogger<EmployeeActivationJob> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeActivationJob(
            ILogger<EmployeeActivationJob> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            int employeeId = context.MergedJobDataMap.GetInt("EmployeeId");

            _logger.LogInformation("Executing ActivationJob for EmployeeId={EmployeeId}", employeeId);

            try
            {
                await _employeeService.ActivateEmployeeByIdAsync(employeeId);
                _logger.LogInformation("Employee {EmployeeId} activated successfully", employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to activate Employee {EmployeeId}", employeeId);
            }
        }
    }
}