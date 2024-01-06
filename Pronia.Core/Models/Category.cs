using Pronia.Core.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Core.Models
{
    public class Category : BaseEntity
    {

        [StringLength(maximumLength: 10, ErrorMessage = "name should have 10 caharacters")]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
