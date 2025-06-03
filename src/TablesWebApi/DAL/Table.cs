namespace TablesWebApi.DAL
{
    public class Table
    {
        public Guid Id { get; set; }
        public int TableNumber { get; set; }
        public int MaxSeats { get; set; }

        public ICollection<ReservationTableLink> ReservationTableLinks { get; set; } = new List<ReservationTableLink>();
    }
}
