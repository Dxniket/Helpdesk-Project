namespace HelpdeskAPI.Dateien.Model
{
    public class DateiModel
    {
        public int DateiId { get; set; }

        public int TicketId { get; set; }

        public string Dateiname { get; set; }

        public string ContentType { get; set; }

        public DateTime ErstelltAm { get; set; }
    }
}