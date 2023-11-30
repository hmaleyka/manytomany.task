using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace manytomany.task.Areas.Manage.ViewModels.Product
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public int? CategoryId { get; set; }
        
        public List<int>? TagIds { get; set; }
        [Required]
        public IFormFile mainPhoto { get; set; }
        [Required]
        public IFormFile hoverPhoto { get; set; }
        public List<IFormFile>? multipleImages { get; set; }
        



    }
}
