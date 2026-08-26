namespace HelpdeskAPI.Kommentare.Model
{
    public class KommentarRequest
    {
        public int TicketId { get; set; }

        public int BenutzerId { get; set; }

        public string Text { get; set; }
    }
}