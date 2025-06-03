namespace TablesWebApi.DAL
{
    public class ReservationContact
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public Reservation Reservation { get; set; } = default!;
    }
}
