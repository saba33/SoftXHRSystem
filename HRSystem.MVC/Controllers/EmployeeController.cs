using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;
using HRSystem.Application.DTOs.Position;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class EmployeeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IHttpClientFactory httpClientFactory, ILogger<EmployeeController> logger)
    {
        _httpClient = httpClientFactory.CreateClient("API");
        _logger = logger;
    }

    private async Task PopulatePositionsTree()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Position/tree");

            if (response.IsSuccessStatusCode)
            {
                var positions = await response.Content.ReadFromJsonAsync<List<PositionResponse>>() ?? new List<PositionResponse>();
                ViewBag.Positions = positions;
            }
            else
            {
                ViewBag.Positions = new List<PositionResponse>();
                _logger.LogWarning("Failed to fetch position tree. Status: {Status}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load positions tree for dropdown.");
            ViewBag.Positions = new List<PositionResponse>();
        }
    }


    public async Task<IActionResult> Index([FromQuery] string SearchString)
    {
        List<EmployeeResponse> employees = new List<EmployeeResponse>();
        string endpoint = string.IsNullOrEmpty(SearchString)
                            ? "api/Employee/all"
                            : $"api/Employee/search?keyword={SearchString}";

        try
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                employees = await response.Content.ReadFromJsonAsync<List<EmployeeResponse>>() ?? new List<EmployeeResponse>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Account");
            }
            else
            {
                TempData["Error"] = $"მონაცემების წამოღება ვერ მოხერხდა. სტატუსი: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while fetching employees.");
            TempData["Error"] = "კავშირის შეცდომა API-სთან.";
        }

        ViewBag.CurrentSearch = SearchString;
        return View(employees);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "🗑️ თანამშრომელი წარმატებით წაიშალა!";
            }
            else
            {
                TempData["Error"] = $"წაშლა ვერ მოხერხდა. სტატუსი: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Employee Delete failed for Id={Id}.", id);
            TempData["Error"] = "კავშირის შეცდომა API-სთან.";
        }

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulatePositionsTree();
        return View(new EmployeeCreateRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            await PopulatePositionsTree();
            return View(request);
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Employee", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "თანამშრომელი წარმატებით დაემატა!";
                return RedirectToAction(nameof(Index));
            }

            string errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"დამატება ვერ მოხერხდა. შეამოწმეთ პირადი ნომერი. სტატუსი: {response.StatusCode}. დეტალები: {errorMessage}");

            await PopulatePositionsTree();
            return View(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Employee Create failed.");
            ModelState.AddModelError(string.Empty, "კავშირის შეცდომა API-სთან.");
            await PopulatePositionsTree();
            return View(request);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                var employeeResponse = await response.Content.ReadFromJsonAsync<EmployeeResponse>();
                if (employeeResponse == null) return NotFound();

                await PopulatePositionsTree();

                return View(employeeResponse);
            }

            TempData["Error"] = $"თანამშრომელი ID={id} ვერ მოიძებნა.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Employee Edit (GET) failed for Id={Id}.", id);
            TempData["Error"] = "კავშირის შეცდომა API-სთან მონაცემების წამოღებისას.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EmployeeUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            await PopulatePositionsTree();
            return View(request);
        }

        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Employee/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "📝 თანამშრომლის მონაცემები წარმატებით განახლდა!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, $"განახლება ვერ მოხერხდა. სტატუსი: {response.StatusCode}");
            await PopulatePositionsTree();
            return View(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP Employee Edit (POST) failed for Id={Id}.", id);
            ModelState.AddModelError(string.Empty, "კავშირის შეცდომა API-სთან.");
            await PopulatePositionsTree();
            return View(request);
        }
    }
}