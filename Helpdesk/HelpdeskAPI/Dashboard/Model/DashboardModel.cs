namespace HelpdeskAPI.Dashboard.Model
{
    public class DashboardModel
    {
        public int AlleTickets { get; set; }

        public int OffeneTickets { get; set; }

        public int TicketsInBearbeitung { get; set; }

        public int GeschlosseneTickets { get; set; }

        public int HohePrioritaet { get; set; }

        public int NormalePrioritaet { get; set; }

        public int NiedrigePrioritaet { get; set; }

        public int HeuteErstellt { get; set; }

        public int DiesenMonatErstellt { get; set; }
    }
}