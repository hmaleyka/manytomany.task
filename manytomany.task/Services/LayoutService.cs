using Azure;
using Azure.Core;
using manytomany.task.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace manytomany.task.Services
{
    public class LayoutService
    {
        AppDbContext _db;
        IHttpContextAccessor _http;
        public LayoutService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Dictionary<string, string>> GetSetting()
        {
            Dictionary<string, string> setting = _db.setting.ToDictionary(s => s.Key, s => s.Value);
            return setting;
        }
        public async Task<List<BasketItemVM>> GetBasket()
        {
            var jsonCookie = _http.HttpContext.Request.Cookies["Basket"];
            List<BasketItemVM> basketItem = new List<BasketItemVM>();
            if (jsonCookie != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<BasketCookieVM>>(jsonCookie);
                bool countcheck = false;
                List<BasketCookieVM> deletedcookie = new List<BasketCookieVM>();
                foreach (var item in cookieItems)
                {
                    Product product = await _db.products.Include(p => p.productImages.Where(p => p.IsPrime == true)).FirstOrDefaultAsync(p => p.Id == item.Id);
                    if (product == null)
                    {
                        deletedcookie.Add(item);


                        continue;
                    }

                    basketItem.Add(new BasketItemVM()
                    {
                        Id = item.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Count = item.Count,
                        ImgUrl = product.productImages.FirstOrDefault().ImgUrl,
                    });
                }
                if (deletedcookie.Count > 0)
                {
                    foreach (var delete in deletedcookie)
                    {
                        cookieItems.Remove(delete);
                    }
                    _http.HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));

                }

            }
            return basketItem;
        }
    }
}
