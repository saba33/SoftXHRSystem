using FluentAssertions;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;

namespace HRSystem.Tests.Worker
{
    public class EmployeeActivationJobTests
    {
        private readonly Mock<ILogger<EmployeeActivationJob>> _loggerMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly EmployeeActivationJob _job;

        public EmployeeActivationJobTests()
        {
            _loggerMock = new Mock<ILogger<EmployeeActivationJob>>();
            _employeeServiceMock = new Mock<IEmployeeService>();

            _job = new EmployeeActivationJob(
                _loggerMock.Object,
                _employeeServiceMock.Object
            );
        }

        [Fact]
        public async Task Execute_ShouldActivateEmployee_WithCorrectEmployeeId()
        {
            var contextMock = new Mock<IJobExecutionContext>();
            var dataMap = new JobDataMap { { "EmployeeId", 5 } };

            contextMock.Setup(c => c.MergedJobDataMap)
                       .Returns(dataMap);

            await _job.Execute(contextMock.Object);

            _employeeServiceMock.Verify(
                s => s.ActivateEmployeeByIdAsync(5),
                Times.Once
            );
        }

        [Fact]
        public async Task Execute_ShouldNotThrow_WhenEmployeeServiceThrows()
        {
            var contextMock = new Mock<IJobExecutionContext>();
            var dataMap = new JobDataMap { { "EmployeeId", 10 } };

            contextMock.Setup(c => c.MergedJobDataMap)
                       .Returns(dataMap);

            _employeeServiceMock
                .Setup(s => s.ActivateEmployeeByIdAsync(10))
                .ThrowsAsync(new Exception("error"));

            Func<Task> act = async () => await _job.Execute(contextMock.Object);

            await act.Should().NotThrowAsync();

            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ),
                Times.AtLeastOnce
            );
        }
    }
}
