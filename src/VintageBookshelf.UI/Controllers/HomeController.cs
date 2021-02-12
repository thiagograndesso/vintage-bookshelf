using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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

        [Route("error/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var errorViewModel = new ErrorViewModel();

            if (id == 500)
            {
                errorViewModel.Title = "Unexpected error";
                errorViewModel.Message = "Whoops! An error has occurred. Please try again!";
            }
            else if (id == 404)
            {
                errorViewModel.Title = "Not found";
                errorViewModel.Message = "This page does not exist!";
            }
            else if (id == 403)
            {
                errorViewModel.Title = "Not allowed";
                errorViewModel.Message = "Access is not allowed!";
            }
            else
            {
                return StatusCode(500);
            }

            errorViewModel.ErrorStatusCode = id;

            return View("Error", errorViewModel);
        }
    } 
}
