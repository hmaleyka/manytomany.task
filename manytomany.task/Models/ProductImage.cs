using manytomany.task.Models.Entity;

namespace manytomany.task.Models
{
    public class ProductImage : BaseEntity
    {
       
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
        public int? ProductId { get; set; }
        public Product product { get; set; }
    }
}
