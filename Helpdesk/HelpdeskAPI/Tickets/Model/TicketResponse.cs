namespace HelpdeskAPI.Tickets.Model
{
    public class TicketResponse
    {
        public bool Succeeded { get; set; }

        public TicketModel? Ticket { get; set; }
    }
}