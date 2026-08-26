namespace HelpdeskAPI.Dashboard.Model
{
    public class DashboardTicketModel
    {
        public int TicketId { get; set; }

        public string Titel { get; set; }

        public string WertStatus { get; set; }

        public string WertPriority { get; set; }

        public DateTime ErstelltAm { get; set; }
    }
}