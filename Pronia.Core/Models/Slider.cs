using Microsoft.AspNetCore.Http;
using Pronia.Core.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Core.Models
{
    public class Slider : BaseEntity
    {

        [Required, StringLength(25, ErrorMessage = "Length should have max 25 characters")]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        [StringLength(maximumLength: 100)]
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
