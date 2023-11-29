namespace manytomany.task.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        public Category category { get; set; }
        
        public List <ProductTag>? productTags { get; set; }
        public List<ProductImage> productImages { get; set; }
    }
}
