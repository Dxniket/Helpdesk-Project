namespace HelpdeskAPI.Tickets.Model
{
    public class TicketLogModel
    {
        public int TicketLogId { get; set; }

        public int TicketId { get; set; }

        public string Beschreibung { get; set; }

        public DateTime ErstelltAm { get; set; }
    }
}