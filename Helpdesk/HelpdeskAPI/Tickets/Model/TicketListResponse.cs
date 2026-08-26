namespace HelpdeskAPI.Tickets.Model
{
    public class TicketListResponse
    {
        public bool Succeeded { get; set; }

        public List<TicketModel> Tickets { get; set; }
    }
}