namespace ShopWebApi.DAL
{
    public class UserBagProduct
    {
        public int UserBagId { get; set; }
        public UserBag UserBag { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
