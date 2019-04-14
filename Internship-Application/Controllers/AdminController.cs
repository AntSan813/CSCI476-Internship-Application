using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internship_Application.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]//only allow if admin role
        public IActionResult Index()
        {
            return View();
        }
    }
}