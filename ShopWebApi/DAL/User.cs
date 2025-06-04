namespace ShopWebApi.DAL
{
    public class User
    {
        public string Id { get; set; }
        public ICollection<UserOrder> Orders { get; set; }

        public UserBag Bag { get; set; }
    }
}
