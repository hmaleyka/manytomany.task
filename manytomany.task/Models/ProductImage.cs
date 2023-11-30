namespace manytomany.task.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
        public int? ProductId { get; set; }
        public Product product { get; set; }
    }
}
