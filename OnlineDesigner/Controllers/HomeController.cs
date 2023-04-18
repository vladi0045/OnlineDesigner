using Microsoft.AspNetCore.Mvc;
using OnlineDesigner.Models;
using System.Diagnostics;
using OnlineDesigner.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace OnlineDesigner.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IdentityContext _identityContext;

        public HomeController(ILogger<HomeController> logger, IdentityContext identityContext, OnlineDesignerContext context)
        {
            _logger = logger;
            _identityContext = identityContext;
        }

        public async Task<IActionResult> Index()
        {
            await _identityContext.SeedDataAsync("administrator", "admin123");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}