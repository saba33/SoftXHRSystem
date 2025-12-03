using HRSystem.API.Extensions;
using HRSystem.Application.DTOs.Employees;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;
using HRSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService,
            ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create(EmployeeCreateRequest request)
        {
            var userId = User.GetUserId();
            var result = await _employeeService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Update(int id, EmployeeUpdateRequest request)
        {
            int userId = User.GetUserId();
            var result = await _employeeService.UpdateAsync(id, request, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
        {
            var result = await _employeeService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EmployeeResponse>>> Search([FromQuery] string keyword)
        {
            var result = await _employeeService.SearchAsync(keyword);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<EmployeeResponse>>> Filter(
            [FromBody] EmployeeFilterRequest filter,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "personalnumber",
            [FromQuery] string sortDirection = "asc")
        {
            var result = await _employeeService.FilterAsync(
                filter,
                page,
                pageSize,
                sortBy,
                sortDirection
            );

            return Ok(result);
        }
    }
}