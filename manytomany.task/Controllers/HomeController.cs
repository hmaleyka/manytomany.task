using manytomany.task.DAL;
using manytomany.task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace manytomany.task.Controllers
{
    public class HomeController : Controller
    {

        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Index()
        {
            HomeVM homevm = new HomeVM()
            {
                products = await _context.products.Include(p=>p.productImages).ToListAsync(),
            };
            return View(homevm);
        }
    }
}
