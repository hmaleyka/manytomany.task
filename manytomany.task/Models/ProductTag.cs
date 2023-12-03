using manytomany.task.Models.Entity;

namespace manytomany.task.Models
{
    public class ProductTag : BaseEntity
    {
        
        public int? ProductId { get; set; }
        public Product product { get; set; }
        public int? TagId { get; set; }
        public Tag tag { get; set; }
    }
}
