namespace ShopWebApi.DAL
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public byte[] Photo { get; set; }

        public string Description { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<UserOrderProduct> OrderProducts { get; set; }
    }
}
