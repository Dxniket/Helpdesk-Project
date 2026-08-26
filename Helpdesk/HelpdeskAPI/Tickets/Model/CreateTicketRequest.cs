namespace HelpdeskAPI.Tickets.Model
{
    public class CreateTicketRequest
    {
        public int BenutzerId { get; set; }

        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        public int PriorityId { get; set; }
    }
}