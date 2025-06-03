namespace TablesWebApi.DAL
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = default!;
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int PeopleCount { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = default!;
        public ReservationContact Contact { get; set; } = default!;

        public ICollection<ReservationTableLink> ReservationTableLinks { get; set; } = new List<ReservationTableLink>();
    }
}
