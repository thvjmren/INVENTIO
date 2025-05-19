using Invent_io.DAL;
using Invent_io.Models;
using Invent_io.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Invent_io.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM vm = new()
            {
                Employees = _context.Employees.ToList(),
                Positions = _context.Positions.ToList()
            };
            return View(vm);
        }
    }
}
