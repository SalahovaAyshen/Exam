using Microsoft.AspNetCore.Mvc;

namespace Studio.Areas.Manage.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
