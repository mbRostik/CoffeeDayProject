namespace ShopWebApi.DAL
{
    public class UserOrderProduct
    {
        public int UserOrderId { get; set; }
        public UserOrder UserOrder { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
