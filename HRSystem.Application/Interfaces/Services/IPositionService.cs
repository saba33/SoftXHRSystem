using HRSystem.Application.DTOs.Position;

namespace HRSystem.Application.Interfaces.Services
{
    public interface IPositionService
    {
        Task<List<PositionResponse>> GetAllAsync();
        Task<List<PositionResponse>> GetTreeAsync();
    }
}
