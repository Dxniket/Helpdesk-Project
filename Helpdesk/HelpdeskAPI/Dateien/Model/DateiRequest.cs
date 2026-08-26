namespace HelpdeskAPI.Dateien.Model
{
    public class DateiRequest
    {
        public int TicketId { get; set; }

        public string Dateiname { get; set; }

        public string ContentType { get; set; }

        public byte[] Datei { get; set; }
    }
}