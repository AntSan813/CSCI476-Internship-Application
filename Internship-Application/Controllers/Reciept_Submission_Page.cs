using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Internship_Application.Models;

namespace Internship_Application.Controllers
{
    public class Reciept_Submission_PageController : Controller
    {

        public IActionResult Index()
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
