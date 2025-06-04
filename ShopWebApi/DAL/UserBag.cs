namespace ShopWebApi.DAL
{
    public class UserBag
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<UserBagProduct> BagProducts { get; set; }
    }
}
