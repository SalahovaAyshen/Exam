using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studio.DAL;
using Studio.Models;
using Studio.ViewModels;

namespace Studio.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.Include(e => e.Position).ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Employees = employees,
            };
            return View(homeVM);
        }
    }
}
