namespace HelpdeskAPI.Tickets.Model
{
    public class AdminTicketModel
    {
        public int TicketId { get; set; }

        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }

        public int? BearbeiterId { get; set; }

        public string BearbeiterVorname { get; set; }

        public string BearbeiterNachname { get; set; }

        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        public int? StatusId { get; set; }

        public string WertStatus { get; set; }

        public int? PriorityId { get; set; }

        public string WertPriority { get; set; }

        public DateTime ErstelltAm { get; set; }

        public DateTime? AktualisiertAm { get; set; }
    }
}
