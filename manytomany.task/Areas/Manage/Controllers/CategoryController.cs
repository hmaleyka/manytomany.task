using manytomany.task.DAL;
using manytomany.task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
                _context= context;
        }
        public IActionResult Index()
        {
            List<Category> categories=_context.categories.Include(p=>p.Products).ToList();
            return View(categories);
        }

        //bu get metodu olaraq gedir httpget
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost] // bu artiq post metodumdu
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.categories.Add(category);
            _context.SaveChanges();          
            return RedirectToAction("Index");           
        }

        public IActionResult Update(int id) 
        {
            Category category = _context.categories.Find(id);

            return View(category);
            
        }

        [HttpPost]
        public IActionResult Update(Category newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Category oldcategory = _context.categories.Find(newCategory.Id);
            oldcategory.Name= newCategory.Name;
            
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {

            Category category = _context.categories.Find(id);
            _context.categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
