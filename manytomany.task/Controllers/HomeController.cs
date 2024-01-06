using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL.Context;
using Pronia.mvc.ViewModels;

namespace Pronia.mvc.Controllers
{
    public class HomeController : Controller
    {

        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //Response.Cookies.Append("Name", "Bsu", new CookieOptions()
            //{
            //    MaxAge = TimeSpan.FromSeconds(5)
            //}); ;

            //  HttpContext.Session.SetString("Name", "Bsu");

            HomeVM homevm = new HomeVM()
            {
                sliders = await _context.sliders.ToListAsync(),
                products = await _context.products.Where(p => p.IsDeleted == false).Include(p => p.productImages).ToListAsync(),
            };
            return View(homevm);
        }
    }
}
