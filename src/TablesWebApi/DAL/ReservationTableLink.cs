namespace TablesWebApi.DAL
{
    public class ReservationTableLink
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid TableId { get; set; }

        public Reservation Reservation { get; set; } = default!;
        public Table Table { get; set; } = default!;
    }
}
