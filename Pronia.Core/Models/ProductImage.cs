using Pronia.Core.Models.Entity;

namespace Pronia.Core.Models
{
    public class ProductImage : BaseEntity
    {

        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
        public int? ProductId { get; set; }
        public Product product { get; set; }
    }
}
