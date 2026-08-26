namespace HelpdeskAPI.Tickets.Model
{
    public class AdminTicketListResponse
    {
        public bool Succeeded { get; set; }

        public List<AdminTicketModel> Tickets { get; set; }
    }
}
