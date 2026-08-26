namespace HelpdeskAPI.Tickets.Model
{
    public class AssignBearbeiterRequest
    {
        public int TicketId { get; set; }

        public int BearbeiterId { get; set; }
    }
}
