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
        



    }
}
