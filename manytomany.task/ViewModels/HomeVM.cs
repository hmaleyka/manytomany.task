using Pronia.Core.Models;

namespace manytomany.task.ViewModels
{
    public class HomeVM
    {

        public List<Product> products { get; set; }
        public List<Category> categories { get; set; }
        public List<ProductImage> images { get; set; }
        public List<Slider> sliders { get; set; }
    }
}
