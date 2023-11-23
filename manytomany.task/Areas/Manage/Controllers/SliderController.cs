

using manytomany.task.Models;

namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(AppDbContext context , IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            List<Slider> sliderList = _context.sliders.ToList();
            return View(sliderList);
        }

        public IActionResult Create() 
        {
            return View();
        
        }

        [HttpPost]
        public async Task <IActionResult> Create(Slider slider)
        {

            if(!slider.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile" ,"you should only apply the image");
                return View();
            }
            if(slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size should be max 2 MB");
                return View();
            }
            //string fileName = slider.ImageFile.FileName;

            //if(fileName.Length>64)
            //{
            //    fileName = fileName.Substring(fileName.Length - 64);
            //}
            //fileName= Guid.NewGuid().ToString()+fileName;

            ////string path = "C:\\Users\\owner\\Desktop\\manytomany.task\\manytomany.task\\wwwroot\\Upload\\SliderImage\\" + fileName;

            ////string path = _environment.WebRootPath + @"\Upload\SliderImage" + fileName;
            //using (FileStream stream = new FileStream(path, FileMode.Create))
            //{
            //    slider.ImageFile.CopyTo(stream);
            //}
            slider.ImgUrl = slider.ImageFile.Upload(_environment.WebRootPath, @"\Upload\SliderImage\");

            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(slider);
            await _context.SaveChangesAsync();

          return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var slider = _context.sliders.FirstOrDefault(s => s.Id == id);

            _context.sliders.Remove(slider);
            _context.SaveChanges();
            FileManager.DeleteFile(slider.ImgUrl, _environment.WebRootPath, @"\Upload\SliderImage\");
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Slider slider = _context.sliders.Find(id);

            return View(slider);

        }
        [HttpPost]
        public async Task <IActionResult> Update(Slider newSlider)
        {
            if (!newSlider.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "you should only apply the image");
                return View();
            }
            if (newSlider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The size should be max 2 MB");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            newSlider.ImgUrl =  newSlider.ImageFile.Upload(_environment.WebRootPath, @"\Upload\SliderImage\");

            Slider oldSlider = await _context.sliders.FindAsync(newSlider.Id);
            oldSlider.Title = newSlider.Title;
            oldSlider.SubTitle = newSlider.SubTitle;
            oldSlider.Description = newSlider.Description;
            oldSlider.ImgUrl = newSlider.ImgUrl;

            await  _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
