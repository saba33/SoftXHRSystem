using HRSystem.API.Services;
using HRSystem.Worker;
using Moq;
using Quartz;

public class EmployeeSchedulerServiceTests
{
    private readonly Mock<ISchedulerFactory> _schedulerFactoryMock;
    private readonly Mock<IScheduler> _schedulerMock;
    private readonly EmployeeSchedulerService _service;

    public EmployeeSchedulerServiceTests()
    {
        _schedulerFactoryMock = new Mock<ISchedulerFactory>();
        _schedulerMock = new Mock<IScheduler>();

        _schedulerFactoryMock
            .Setup(f => f.GetScheduler(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_schedulerMock.Object);

        _service = new EmployeeSchedulerService(_schedulerFactoryMock.Object);
    }

    [Fact]
    public async Task ScheduleActivationAsync_ShouldScheduleJob()
    {
        await _service.ScheduleActivationAsync(1);

        _schedulerMock.Verify(s => s.ScheduleJob(
            It.Is<IJobDetail>(job =>
        job.JobType == typeof(EmployeeActivationJob) &&
        job.Key.Name == "activation-job-1"
              ),
             It.Is<ITrigger>(trigger =>
        trigger.Key.Name == "activation-trigger-1"
              ),
              It.IsAny<CancellationToken>()
          ), Times.Once);
    }
}