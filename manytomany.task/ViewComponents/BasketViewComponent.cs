using manytomany.task.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace manytomany.task.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        AppDbContext _db;
        IHttpContextAccessor _http;


        public BasketViewComponent(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var jsonCookie = _http.HttpContext.Request.Cookies["Basket"];
            List<BasketItemVM> basketItem = new List<BasketItemVM>();
            if (jsonCookie != null)
            {
                var cookieItems = JsonConvert.DeserializeObject<List<BasketCookieVM>>(jsonCookie);

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

            }
            return View(basketItem);
        }
    }
}
