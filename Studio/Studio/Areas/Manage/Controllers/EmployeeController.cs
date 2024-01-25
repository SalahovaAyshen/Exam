using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studio.Areas.Manage.ViewModels;
using Studio.DAL;
using Studio.Models;
using Studio.Utilities.Enums;
using Studio.Utilities.Extensions;

namespace Studio.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles =nameof(UserRole.Admin))]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page =1)
        {
            int count = await _context.Employees.CountAsync();
            List<Employee> employees = await _context.Employees.Skip((page-1)*2).Take(2).Include(e=>e.Position).ToListAsync();
            PaginationVM<Employee> paginationVM = new PaginationVM<Employee>
            {
                TotalPage = Math.Ceiling((double)count / 2),
                CurrentPage = page,
                Items = employees
            };
            return View(paginationVM);
        }
        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new CreateEmployeeVM
            {
                Positions = await _context.Positions.ToListAsync(),
            };
            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            employeeVM.Positions= await _context.Positions.ToListAsync();
            if (!ModelState.IsValid) return View(employeeVM);
            if (!employeeVM.Name.Check())
            {
                ModelState.AddModelError("Name", "Name can't contain number");
                return View(employeeVM);
            }
            if (!employeeVM.Surname.Check())
            {
                ModelState.AddModelError("Surname", "Surname can't contain number");
                return View(employeeVM);
            }
            if(await _context.Employees.AnyAsync(e=>e.Name==employeeVM.Name))
            {
                ModelState.AddModelError("Name", "Existed");
                return View(employeeVM);
            }
            if (await _context.Employees.AnyAsync(e => e.Surname == employeeVM.Surname))
            {
                ModelState.AddModelError("Surname", "Existed");
                return View(employeeVM);
            }
            if ( employeeVM.PositionId<=0 || employeeVM.Positions.FirstOrDefault(e => e.Id == employeeVM.PositionId)==null)
            {
                ModelState.AddModelError("PositionId", "Not found");
                return View(employeeVM);
            }
            if (!employeeVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Wrong type");
                return View(employeeVM);
            }
            if (!employeeVM.Photo.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Photo", "Wrong size");
                return View(employeeVM);
            }
            string image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            await _context.Employees.AddAsync(new Employee
            {
                Name = employeeVM.Name,
                Surname = employeeVM.Surname,
                PositionId =(int) employeeVM.PositionId,
                TwitterLink = employeeVM.TwitterLink,
                FacebookLink = employeeVM.FacebookLink,
                LinkedinLink = employeeVM.LinkedinLink,
                Image= image,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id);
            if (employee == null) return NotFound();
            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = employee.Name,
                Surname = employee.Surname,
                TwitterLink = employee.TwitterLink,
                FacebookLink = employee.FacebookLink,
                LinkedinLink = employee.LinkedinLink,
                Image = employee.Image,
                PositionId = (int)employee.PositionId,
                Positions = await _context.Positions.ToListAsync(),
            };
            return View(employeeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateEmployeeVM employeeVM)
        {
            employeeVM.Positions= await _context.Positions.ToListAsync();
            if (id <= 0) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();
            if (!employeeVM.Name.Check())
            {
                ModelState.AddModelError("Name", "Name can't contain number");
                return View(employeeVM);
            }
            if (!employeeVM.Surname.Check())
            {
                ModelState.AddModelError("Surname", "Surname can't contain number");
                return View(employeeVM);
            }
            if (employeeVM.PositionId <= 0 || employeeVM.Positions.FirstOrDefault(e => e.Id == employeeVM.PositionId) == null )
            {
                ModelState.AddModelError("PositionId", "Not found");
                return View(employeeVM);
            }
            if(employeeVM.Photo != null)
            {
                if (!employeeVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "Wrong type");
                    return View(employeeVM);
                }
                if (!employeeVM.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "Wrong size");
                    return View(employeeVM);
                }
                employee.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                employee.Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            employee.Name= employeeVM.Name;
            employee.Surname= employeeVM.Surname;
            employee.FacebookLink= employeeVM.FacebookLink;
            employee.TwitterLink= employeeVM.TwitterLink;
            employee.LinkedinLink= employeeVM.LinkedinLink;
            employee.PositionId =(int) employeeVM.PositionId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();
            employee.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
