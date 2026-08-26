namespace HelpdeskAPI.Tickets.Model
{
    public class UpdateTicketRequest
    {
        public int TicketId { get; set; }

        public string Titel { get; set; }

        public string Beschreibung { get; set; }
    }
}