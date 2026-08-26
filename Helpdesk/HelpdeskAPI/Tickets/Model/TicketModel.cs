namespace HelpdeskAPI.Tickets.Model
{
    public class TicketModel
    {
        public int TicketId { get; set; }

        public int BenutzerId { get; set; }

        public int? BearbeiterId { get; set; }

        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        public int StatusId { get; set; }

        public string WertStatus { get; set; }

        public int PriorityId { get; set; }

        public string WertPriority { get; set; }

        public DateTime ErstelltAm { get; set; }

        public DateTime? AktualisiertAm { get; set; }

        public string BearbeiterVorname { get; set; }

        public string BearbeiterNachname { get; set; }
    }
}