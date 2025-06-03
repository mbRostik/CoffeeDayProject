namespace TablesWebApi.DAL
{
    public class User
    {
        public string Id { get; set; } = default!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
