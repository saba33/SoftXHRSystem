namespace HRSystem.Application.Interfaces.Services
{
    public interface IEmployeeSchedulerService
    {
        Task ScheduleActivationAsync(int employeeId);
    }
}
