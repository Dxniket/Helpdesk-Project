namespace HelpdeskAPI.Tickets.Model
{
    public class AdminTicketResponse
    {
        public bool Succeeded { get; set; }

        public AdminTicketModel Ticket { get; set; }
    }
}
