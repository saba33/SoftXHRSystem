using AutoMapper;
using HRSystem.Application.DTOs.Position;
using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Application.Interfaces.Services;

namespace HRSystem.Application.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository positionRepository, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<List<PositionResponse>> GetAllAsync()
        {
            var list = await _positionRepository.GetAllAsync();
            return _mapper.Map<List<PositionResponse>>(list);
        }

        public async Task<List<PositionResponse>> GetTreeAsync()
        {
            var list = await _positionRepository.GetTreeAsync();

            var allDtos = _mapper.Map<List<PositionResponse>>(list);

            var rootNodes = allDtos.Where(x => x.ParentId == null).ToList();

            return rootNodes;
        }
    }
}
