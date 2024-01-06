using Pronia.Core.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Core.Models
{
    public class Setting : BaseEntity
    {

        [StringLength(maximumLength: 15, ErrorMessage = "name should have 10 caharacters")]
        public string Key { get; set; }
        public string Value { get; set; }


    }
}
