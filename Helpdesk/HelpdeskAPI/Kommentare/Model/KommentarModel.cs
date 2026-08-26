namespace HelpdeskAPI.Kommentare.Model
{
    public class KommentarModel
    {
        public int KommentarId { get; set; }

        public int TicketId { get; set; }

        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public bool IsAdmin { get; set; }

        public string Text { get; set; }

        public DateTime ErstelltAm { get; set; }
    }
}