using manytomany.task.Models.Entity;

namespace manytomany.task.Models
{
    public class Tag : BaseEntity
    {
        
        public string Name { get; set; }
        public List<ProductTag> productTags { get; set; }
    }
}
