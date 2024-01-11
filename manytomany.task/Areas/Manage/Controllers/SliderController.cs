using Pronia.Business.Helpers;
using Pronia.Business.Services.Interfaces;
using Pronia.Core.Models;
using Pronia.DAL.Context;

namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ISliderService _service;
        public SliderController(IWebHostEnvironment environment, ISliderService service)
        {

            _environment = environment;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _service.GetAllAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {

            slider.ImgUrl = slider.ImageFile.Upload(_environment.WebRootPath, @"\Upload\SliderImage\");
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _service.Create(slider);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(Slider slider)
        {
            Slider sliders = await _service.GetByIdAsync(slider.Id);
            _service.Delete(sliders);
            //FileManager.DeleteFile(slider.ImgUrl, _environment.WebRootPath, @"\Upload\SliderImage\");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var slider = await _service.GetByIdAsync(id);
            return View(slider);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Slider newSlider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            newSlider.ImgUrl = newSlider.ImageFile.Upload(_environment.WebRootPath, @"\Upload\SliderImage\");
            await _service.Update(newSlider);
            return RedirectToAction("Index");
        }
    }
}
