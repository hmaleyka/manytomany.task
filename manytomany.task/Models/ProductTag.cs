namespace manytomany.task.Models
{
    public class ProductTag
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public Product product { get; set; }
        public int? TagId { get; set; }
        public Tag tag { get; set; }
    }
}
