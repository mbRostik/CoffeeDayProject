namespace ShopWebApi.DAL
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
