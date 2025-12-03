using AutoMapper;
using FluentAssertions;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;
using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;
using HRSystem.Application.Services;
using HRSystem.Domain.Entities;
using HRSystem.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
namespace HRSystem.Tests.Application
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EmployeeService>> _loggerMock;
        private readonly Mock<IEmployeeSchedulerService> _schedulerMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EmployeeService>>();
            _schedulerMock = new Mock<IEmployeeSchedulerService>();
            _service = new EmployeeService(_repoMock.Object, _mapperMock.Object, _loggerMock.Object, _schedulerMock.Object);
        }


        [Fact]
        public async Task CreateAsync_ShouldCreateEmployee_WhenValid()
        {
            var request = new EmployeeCreateRequest
            {
                FirstName = "salome",
                LastName = "devdariani",
                PersonalNumber = "12345678901",
                Gender = Gender.Female,
                PositionId = 1,
                Status = EmployeeStatus.Inactive
            };

            var employee = new Employee { Id = 1, FirstName = "salome" };

            _mapperMock.Setup(m => m.Map<Employee>(request))
                       .Returns(employee);

            _repoMock.Setup(r => r.AddAsync(employee))
                     .ReturnsAsync(employee);

            _mapperMock.Setup(m => m.Map<EmployeeResponse>(employee))
                       .Returns(new EmployeeResponse { Id = 1, FirstName = "salome" });

            var result = await _service.CreateAsync(request, 1);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.FirstName.Should().Be("salome");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ShouldDelete_WhenEmployeeExists()
        {
            var employee = new Employee { Id = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(employee);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(employee), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenEmployeeNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Employee)null);

            Func<Task> act = async () => await _service.DeleteAsync(1);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("თანამშრომელი ვერ მოიძებნა");
        }
    }
}
