using HRSystem.Application.DTOs.Auth.Requsets;
using HRSystem.Application.DTOs.Auth.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRSystem.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    await SignInUser(result.Username, result.Token);

                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    string message = "მომხმარებელი ან პაროლი არასწორია.";
                    _logger.LogWarning("API Login failed: {Status}", response.StatusCode);
                    ModelState.AddModelError(string.Empty, message);
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "დაფიქსირდა კავშირის შეცდომა.");
                _logger.LogError(ex, "HTTP Login failed.");
                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                request.Username = request.Email;
            }

            if (!ModelState.IsValid)
            {
                return View(request);
            }
            if (request.Password != request.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(request.ConfirmPassword), "პაროლები არ ემთხვევა.");
                return View(request);
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/Register", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    await SignInUser(result.Username, result.Token);

                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    string errorDetail = "რეგისტრაცია ვერ მოხერხდა. შეამოწმეთ, რომ ყველა ველი სწორად არის შევსებული.";
                    string content = await response.Content.ReadAsStringAsync();

                    _logger.LogError("API Register failed with status {Status}. Response body: {Content}", response.StatusCode, content);

                    if (content.Contains("უკვე გამოყენებულია") || content.Contains("უკვე არსებობს"))
                    {
                        errorDetail = "მოცემული Username, Email ან პირადი ნომერი უკვე გამოყენებულია.";
                    }

                    ModelState.AddModelError(string.Empty, errorDetail);
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "დაფიქსირდა კავშირის შეცდომა.");
                _logger.LogError(ex, "HTTP Register failed.", ex);
                return View(request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }

        private async Task SignInUser(string username, string token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("JwtToken", token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            authProperties.StoreTokens(new[]
            {
                new AuthenticationToken { Name = "JwtToken", Value = token }
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}