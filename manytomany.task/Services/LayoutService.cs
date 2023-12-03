namespace manytomany.task.Services
{
    public class LayoutService
    {
        AppDbContext _db;

        public LayoutService(AppDbContext db)
        {
            _db = db;
        }

        public async Task <Dictionary<string, string>> GetSetting()
        {
            Dictionary<string, string> setting = _db.setting.ToDictionary(s=>s.Key, s=>s.Value);
            return setting;
        }
    }
}
