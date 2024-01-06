using Pronia.Core.Models.Entity;

namespace Pronia.Core.Models
{
    public class Tag : BaseEntity
    {

        public string Name { get; set; }
        public List<ProductTag> productTags { get; set; }
    }
}
