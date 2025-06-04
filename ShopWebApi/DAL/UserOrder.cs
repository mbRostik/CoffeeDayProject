namespace ShopWebApi.DAL
{
    public class UserOrder
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; } = new DateTime();
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
        public ICollection<UserOrderProduct> OrderProducts { get; set; }
    }
}
