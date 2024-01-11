using manytomany.task.Areas.Manage.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Core.Models;
using Pronia.DAL.Context;
using manytomany.task.Areas.Manage.ViewModels.Setting;

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
        public async Task<IActionResult> Create(CreateSettingVM settingvm)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            Setting setting = new Setting()
            {
                Key = settingvm.Key,
                Value = settingvm.Value,
            };
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
            if (setting == null)
            {
                return View("Error");
            }
            UpdateSettingVM updateSettingVM = new UpdateSettingVM
            {
                Id = setting.Id,
                Key = setting.Key,
                Value = setting.Value
            };
            return View(updateSettingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSettingVM updatevm)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            UpdateSettingVM setting = new UpdateSettingVM()
            {
                Key = updatevm.Key,
                Value = updatevm.Value,

            };

            Setting oldSetting = await _db.setting.Where(p => p.Id == updatevm.Id).FirstOrDefaultAsync();
            oldSetting.Key = updatevm.Key;
            oldSetting.Value = updatevm.Value;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
