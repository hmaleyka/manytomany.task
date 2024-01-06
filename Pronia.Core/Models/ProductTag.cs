using Pronia.Core.Models.Entity;

namespace Pronia.Core.Models
{
    public class ProductTag : BaseEntity
    {

        public int? ProductId { get; set; }
        public Product product { get; set; }
        public int? TagId { get; set; }
        public Tag tag { get; set; }
    }
}
