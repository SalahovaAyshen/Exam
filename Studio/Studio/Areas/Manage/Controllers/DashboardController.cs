using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studio.Utilities.Enums;

namespace Studio.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
