namespace HelpdeskAPI.Tickets.Model
{
    public class CreateTicketResponse
    {
        public bool Succeeded { get; set; }

        public int TicketId { get; set; }

        public string ErrorMessage { get; set; }
    }
}