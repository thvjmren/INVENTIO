using Invent_io.DAL;
using Invent_io.Models;
using Invent_io.Utilities.Enums;
using Invent_io.Utilities.Extensions;
using Invent_io.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invent_io.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<GetEmployeeVM> employeeVMs = await _context.Employees.Select(e =>
                new GetEmployeeVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Facebook = e.Facebook,
                    Linkedin = e.Linkedin,
                    X = e.X,
                    Instagram = e.Instagram,
                    Image = e.Image,
                    PositionName = e.Position.Name,
                }
            ).ToListAsync();

            return View(employeeVMs);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (employeeVM.Photo is not null)
            {
                if (!employeeVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "ONLY IMAGE");
                    return View(employeeVM);
                }

                if (!employeeVM.Photo.ValidateSize(FileSize.MB, 5))
                {
                    ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "5MB");
                    return View(employeeVM);
                }
            }

            string fileName = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "person");

            Employee? employee = new()
            {
                Name = employeeVM.Name,
                Facebook = employeeVM.Facebook,
                Instagram = employeeVM.Instagram,
                Linkedin = employeeVM.Linkedin,
                X = employeeVM.X,
                Image = fileName,
                Description = employeeVM.Description,
                PositionId = employeeVM.PositionId.Value,

            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Employee? employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if (employee is null) return NotFound();

            employee.Image.DeleteFile(_env.WebRootPath, "assets", "img", "person");
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Employee? employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);

            if (employee is null) return NotFound();

            UpdateEmployeeVM vm = new()
            {
                Name = employee.Name,
                Image = employee.Image,
                Facebook = employee.Facebook,
                Linkedin = employee.Linkedin,
                X = employee.X,
                Instagram = employee.Instagram,
                PositionId = employee.PositionId,
                Positions = await _context.Positions.ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int? id,UpdateEmployeeVM employeeVM)
        {
            employeeVM.Positions = await _context.Positions.ToListAsync();

            if (!ModelState.IsValid) return View();

            if (employeeVM.Photo is not null)
            {
                if (!employeeVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "ONLY IMAGE");
                    return View(employeeVM);
                }

                if (!employeeVM.Photo.ValidateSize(FileSize.MB, 5))
                {
                    ModelState.AddModelError(nameof(CreateEmployeeVM.Photo), "5MB");
                    return View(employeeVM);
                }

                employeeVM.Image.DeleteFile(_env.WebRootPath, "assets", "img", "person");
                employeeVM.Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "person");
            }

            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id);

            employee.Name = employeeVM.Name;
            employee.Facebook = employeeVM.Facebook;
            employee.X = employeeVM.X;
            employee.Description = employeeVM.Description;
            employee.Instagram = employeeVM.Instagram;
            employee.Linkedin = employeeVM.Linkedin;
            employee.PositionId = employeeVM.PositionId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
