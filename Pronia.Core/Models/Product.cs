using Pronia.Core.Models.Entity;

namespace Pronia.Core.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
        public int? CategoryId { get; set; }
        public Category category { get; set; }

        public List<ProductTag>? productTags { get; set; }
        public List<ProductImage>? productImages { get; set; }
    }
}
