namespace HelpdeskAPI.Tickets.Model
{
    public class UpdatePriorityRequest
    {
        public int TicketId { get; set; }

        public int PriorityId { get; set; }
    }
}