using manytomany.task.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace manytomany.task.Controllers
{
    public class CardController : Controller
    {
        AppDbContext _context;

        public CardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jsonCookie = Request.Cookies["Basket"];
            List<BasketItemVM> basketItem = new List<BasketItemVM>();
            if(jsonCookie != null)
            {
                var cookieItems=JsonConvert.DeserializeObject<List<BasketCookieVM>>(jsonCookie);
                bool countcheck = false;
                List<BasketCookieVM> deletedcookie = new List<BasketCookieVM>(); 
                foreach (var item in cookieItems)
                {
                    Product product = _context.products.Include(p => p.productImages.Where(p => p.IsPrime == true)).FirstOrDefault(p => p.Id == item.Id);
                   if(product==null)
                    {
                        deletedcookie.Add(item);
                        

                        continue;
                    }
                    
                    basketItem.Add(new BasketItemVM()
                    {
                        Id = item.Id,
                        Name = product.Name,
                        Price= product.Price,
                        Count = item.Count,
                        ImgUrl = product.productImages.FirstOrDefault().ImgUrl,
                    });
                }
                if (deletedcookie.Count>0)
                {
                    foreach (var delete in deletedcookie)
                    {
                        cookieItems.Remove(delete);
                    }
                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieItems));
               
                }

            }
            return View(basketItem);
        }

        public IActionResult AddBasket(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = _context.products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            List<BasketCookieVM> basket;
            var json = Request.Cookies["Basket"];




            if (json != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieVM>>(json);
                var existproduct = basket.FirstOrDefault(p => p.Id == id);
                if (existproduct != null)
                {
                    existproduct.Count += 1;
                }
                else
                {
                    basket.Add(new BasketCookieVM()
                    {
                        Id = id,
                        Count = 1
                    });

                }

            }
            else
            {
                basket = new List<BasketCookieVM>();
                basket.Add(new BasketCookieVM()
                {
                    Id = id,
                    Count = 1
                });
            }
            var cookieBasket = JsonConvert.SerializeObject(basket);
            Response.Cookies.Append("Basket", cookieBasket);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult GetBasket()
        {
            var basketcookiejson = Request.Cookies["Basket"];


            return Content(basketcookiejson);
        }
    }
}
