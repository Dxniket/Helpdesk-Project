namespace HelpdeskAPI.Tickets.Model
{
    public class SaveAdminTicketRequest
    {
        public int TicketId { get; set; }

        public int? BearbeiterId { get; set; }

        public int StatusId { get; set; }

        public int PriorityId { get; set; }
    }
}