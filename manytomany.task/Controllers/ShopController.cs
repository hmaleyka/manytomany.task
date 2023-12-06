using manytomany.task.DAL;
using manytomany.task.Models;
using manytomany.task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace manytomany.task.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();

            //string cookie = Request.Cookies["Name"];
            //return Content(cookie);

            //string session = HttpContext.Session.GetString("Name");
            //return Content("Name");

            Product product = _context.products
                .Where(p => p.IsDeleted == false)
                .Include(p => p.category)
                .Include(p => p.productImages)
                .Include(p => p.productTags)
                .ThenInclude(pt => pt.tag)
                .FirstOrDefault(product => product.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            DetailVM detailvm = new DetailVM()
            {
                product = product,
                products = _context.products.Include(p=> p.productImages).Include(p=>p.category).Where(p=>p.CategoryId==product.CategoryId&&p.Id!=product.Id).ToList()
            };
            
            return View(detailvm);
        }
    }
}
