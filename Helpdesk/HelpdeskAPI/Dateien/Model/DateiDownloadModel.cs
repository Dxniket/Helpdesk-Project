namespace HelpdeskAPI.Dateien.Model
{
    public class DateiDownloadModel
    {
        public int DateiId { get; set; }

        public int TicketId { get; set; }

        public string Dateiname { get; set; }

        public string ContentType { get; set; }

        public byte[] Datei { get; set; }
    }
}