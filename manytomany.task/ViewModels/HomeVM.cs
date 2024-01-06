using Pronia.Core.Models;

namespace Pronia.mvc.ViewModels
{
    public class HomeVM
    {

        public List<Product> products { get; set; }
        public List<Category> categories { get; set; }
        public List<ProductImage> images { get; set; }
        public List<Slider> sliders { get; set; }
    }
}
