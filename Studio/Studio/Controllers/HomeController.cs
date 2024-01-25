using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studio.DAL;
using Studio.Models;
using Studio.Services;
using Studio.ViewModels;

namespace Studio.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _service;

        public HomeController(AppDbContext context, LayoutService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e => e.Position).ToListAsync();
            Dictionary<string, string> settings = await _service.GetSettingsAsync();
            HomeVM homeVM = new HomeVM
            {
                Employees = employees,
                Service = settings
            };
            return View(homeVM);
        }
    }
}
