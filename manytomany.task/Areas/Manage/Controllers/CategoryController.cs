using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Business.Services.Interfaces;
using Pronia.Core.Models;
using Pronia.DAL.Context;

namespace Pronia.mvc.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Index()
        {
            var categories = await _service.GetAllAsync();
            return View(categories);
        }


        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Create()
        {
            return View();

        }
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _service.Create(category);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _service.GetByIdAsync(id);

            return View(category);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Category newCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.Update(newCategory);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            Category categories = await _service.GetByIdAsync(id);
            _service.Delete(categories);

            return RedirectToAction("Index");
        }
    }
}
