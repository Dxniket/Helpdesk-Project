namespace HelpdeskAPI.Tickets.Model
{
    public class TicketLogListResponse
    {
        public bool Succeeded { get; set; }

        public List<TicketLogModel> Logs { get; set; }
    }
}