using HRSystem.API.Services;
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
        private readonly EmployeeSchedulerService _employeeSchedulerService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService,
            ILogger<EmployeeController> logger,
            EmployeeSchedulerService employeeSchedulerService)
        {
            _employeeService = employeeService;
            _logger = logger;
            _employeeSchedulerService = employeeSchedulerService;
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> Create(EmployeeCreateRequest request)
        {
            _logger.LogInformation("{Controller}.{Action} invoked with {@Request}",
                nameof(EmployeeController), nameof(Create), request);

            var result = await _employeeService.CreateAsync(request);

            _logger.LogInformation("{Controller}.{Action} succeeded: {@Employee}",
                nameof(EmployeeController), nameof(Create), result);

            await _employeeSchedulerService.ScheduleActivationAsync(result.Id);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> Update(int id, EmployeeUpdateRequest request)
        {
            _logger.LogInformation("{Controller}.{Action} invoked for Id={Id} with {@Request}",
                nameof(EmployeeController), nameof(Update), id, request);

            var result = await _employeeService.UpdateAsync(id, request);

            _logger.LogInformation("{Controller}.{Action} succeeded: {@Employee}",
                nameof(EmployeeController), nameof(Update), result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("{Controller}.{Action} invoked for Id={Id}",
                nameof(EmployeeController), nameof(Delete), id);

            await _employeeService.DeleteAsync(id);

            _logger.LogInformation("{Controller}.{Action} succeeded for Id={Id}",
                nameof(EmployeeController), nameof(Delete), id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetById(int id)
        {
            _logger.LogInformation("{Controller}.{Action} invoked for Id={Id}",
                nameof(EmployeeController), nameof(GetById), id);

            var result = await _employeeService.GetByIdAsync(id);

            _logger.LogInformation("{Controller}.{Action} succeeded: {@Employee}",
                nameof(EmployeeController), nameof(GetById), result);

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
        {
            _logger.LogInformation("{Controller}.{Action} invoked",
                nameof(EmployeeController), nameof(GetAll));

            var result = await _employeeService.GetAllAsync();

            _logger.LogInformation("{Controller}.{Action} succeeded: Count={Count}",
                nameof(EmployeeController), nameof(GetAll), result.Count);

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EmployeeResponse>>> Search([FromQuery] string keyword)
        {
            _logger.LogInformation("{Controller}.{Action} invoked with keyword: {Keyword}",
                nameof(EmployeeController), nameof(Search), keyword);

            var result = await _employeeService.SearchAsync(keyword);

            _logger.LogInformation("{Controller}.{Action} succeeded: Found={Count}",
                nameof(EmployeeController), nameof(Search), result.Count);

            return Ok(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<List<EmployeeResponse>>> Filter(
            [FromBody] EmployeeFilterRequest filter,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("{Controller}.{Action} invoked with {@Filter}, Page={Page}, PageSize={PageSize}",
                nameof(EmployeeController), nameof(Filter), filter, page, pageSize);

            var result = await _employeeService.FilterAsync(filter, page, pageSize);

            _logger.LogInformation("{Controller}.{Action} succeeded: FilteredCount={Count}",
                nameof(EmployeeController), nameof(Filter), result.Count);

            return Ok(result);
        }
    }
}