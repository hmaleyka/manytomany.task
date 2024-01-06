using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Core.Models;
using Pronia.DAL.Context;
using Pronia.mvc.ViewModels;

namespace Pronia.mvc.Services
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
            var jsonCookie = _http?.HttpContext?.Request?.Cookies["Basket"];
            List<BasketItemVM> basketItems = new List<BasketItemVM>();
            if (jsonCookie != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<BasketCookieVM>>(jsonCookie);

                bool countCheck = false;
                List<BasketCookieVM> deletedCookie = new List<BasketCookieVM>();
                foreach (var item in cookieItems)
                {
                    Product product = await _db.products.Where(p => p.IsDeleted == false).Include(p => p.productImages.Where(p => p.IsPrime == true)).FirstOrDefaultAsync(p => p.Id == item.Id);
                    if (product == null)
                    {
                        deletedCookie.Add(item);
                        continue;
                    }

                    basketItems.Add(new BasketItemVM()
                    {
                        Id = item.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Count = item.Count,
                        ImgUrl = product.productImages.FirstOrDefault().ImgUrl
                    });
                }
                if (deletedCookie.Count > 0)
                {
                    foreach (var delete in deletedCookie)
                    {
                        cookieItems.Remove(delete);
                    }
                    _http.HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
                }



            }
            return basketItems;
        }
    }
}
