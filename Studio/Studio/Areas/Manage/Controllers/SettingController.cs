using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studio.Areas.Manage.ViewModels;
using Studio.DAL;
using Studio.Models;
using Studio.Utilities.Enums;

namespace Studio.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles =nameof(UserRole.Admin))]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();

            UpdateSettingVM settingVM = new UpdateSettingVM
            {
                Value = setting.Value,
            };
            return View(settingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVM settingVM)
        {
            if (id <= 0) return BadRequest();
            Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();
            if (!ModelState.IsValid) return View();
            setting.Value = settingVM.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
