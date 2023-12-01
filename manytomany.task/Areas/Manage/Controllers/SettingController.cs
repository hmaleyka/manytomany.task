
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace manytomany.task.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        AppDbContext _db;
        public SettingController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Setting> settingList = _db.setting.ToList();
            return View(settingList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            await _db.setting.AddAsync(setting);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var setting = _db.setting.FirstOrDefault(s => s.Id == id);

            _db.setting.Remove(setting);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Update(int id)
        {
            Setting setting = _db.setting.Find(id);
            return View(setting);
        }

        [HttpPost]
        public async Task <IActionResult> Update (Setting newSetting)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            

            Setting oldSetting = await _db.setting.FindAsync(newSetting.Id);
            oldSetting.Key = newSetting.Key;
            oldSetting.Value = newSetting.Value;
            

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
