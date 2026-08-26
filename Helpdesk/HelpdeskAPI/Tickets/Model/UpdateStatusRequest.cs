namespace HelpdeskAPI.Tickets.Model
{
    public class UpdateStatusRequest
    {
        public int TicketId { get; set; }

        public int StatusId { get; set; }
    }
}