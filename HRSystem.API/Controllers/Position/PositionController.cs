using HRSystem.Application.DTOs.Position;
using HRSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.API.Controllers.Position
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<PositionResponse>>> GetAll()
        {
            var result = await _positionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("tree")]
        public async Task<ActionResult<List<PositionResponse>>> GetTree()
        {
            var result = await _positionService.GetTreeAsync();
            return Ok(result);
        }
    }
}
