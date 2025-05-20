using Invent_io.DAL;
using Invent_io.Models;
using Invent_io.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invent_io.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _context;

        public PositionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetPositionVM> positionVMs = await _context.Positions.Select(p =>
            new GetPositionVM
            {
                Name = p.Name,
                Id = p.Id,
            }).ToListAsync();
            return View(positionVMs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool result = await _context.Positions.AnyAsync(p => p.Name == positionVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(CreatePositionVM.Name), "this name is already exists");
                return View(positionVM);
            }

            Position? position = new()
            {
                Name = positionVM.Name,
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (position is null) return NotFound();

            UpdatePositionVM positionVM = new()
            {
                Name = position.Name,
            };

            return View(positionVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int? id, UpdatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View();

            bool result = await _context.Positions.AnyAsync(p => p.Name == positionVM.Name && p.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(UpdatePositionVM.Name), $"this name: {positionVM.Name} is already exists");
                return View();
            }

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);

            position.Name = positionVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();

            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
            if (position is null) return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
