using Pronia.DAL.Context;

namespace Pronia.mvc.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = _db.setting.ToDictionary(x => x.Key, x => x.Value);
            return View(setting);
        }
    }
}
