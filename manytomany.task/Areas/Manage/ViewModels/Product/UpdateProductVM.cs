namespace manytomany.task.Areas.Manage.ViewModels.Product
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }

        public List<int>? TagIds { get; set; }

        public IFormFile? mainphoto { get; set; }
        public IFormFile? hoverphoto { get; set; }
        public List<IFormFile> multiplephotos { get; set; }
        public List<ProductImagesVm> allproductImages { get; set; }    
    }

    public class ProductImagesVm
    {
        public int Id { get; set; }
        public bool? IsPrime { get; set; }
        public string ImgUrl { get; set; }
    }
}
