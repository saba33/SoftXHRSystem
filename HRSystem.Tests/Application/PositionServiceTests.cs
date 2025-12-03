using AutoMapper;
using FluentAssertions;
using HRSystem.Application.DTOs.Position;
using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Services;
using HRSystem.Domain.Entities;
using Moq;

namespace HRSystem.Tests.Application
{
    public class PositionServiceTests
    {
        private readonly Mock<IPositionRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PositionService _service;

        public PositionServiceTests()
        {
            _repoMock = new Mock<IPositionRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new PositionService(_repoMock.Object, _mapperMock.Object);
        }


        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedPositions()
        {
            var positions = new List<Position>
            {
                new Position { Id = 1, Name = "Manager" },
                new Position { Id = 2, Name = "Developer" }
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(positions);

            var mapped = new List<PositionResponse>
            {
                new PositionResponse { Id = 1, Name = "Manager" },
                new PositionResponse { Id = 2, Name = "Developer" }
            };

            _mapperMock.Setup(m => m.Map<List<PositionResponse>>(positions))
                       .Returns(mapped);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Manager");
            result[1].Name.Should().Be("Developer");

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTreeAsync_ShouldReturnOnlyRootNodes()
        {
            var positions = new List<Position>
            {
                new Position { Id = 1, Name = "CEO", ParentId = null },
                new Position { Id = 2, Name = "Manager", ParentId = 1 },
                new Position { Id = 3, Name = "Developer", ParentId = 2 }
            };

            _repoMock.Setup(r => r.GetTreeAsync())
                     .ReturnsAsync(positions);

            var mapped = new List<PositionResponse>
            {
                new PositionResponse { Id = 1, Name = "CEO", ParentId = null },
                new PositionResponse { Id = 2, Name = "Manager", ParentId = 1 },
                new PositionResponse { Id = 3, Name = "Developer", ParentId = 2 }
            };

            _mapperMock.Setup(m => m.Map<List<PositionResponse>>(positions))
                       .Returns(mapped);

            var result = await _service.GetTreeAsync();

            result.Should().HaveCount(1);
            result[0].Name.Should().Be("CEO");
            result[0].ParentId.Should().BeNull();

            _repoMock.Verify(r => r.GetTreeAsync(), Times.Once);
        }
    }
}
