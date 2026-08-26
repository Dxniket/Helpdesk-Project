namespace HelpdeskAPI.Benutzer.Model
{
    public class BenutzerStatistikModel
    {
        public int ErstellteTickets { get; set; }

        public int OffeneTickets { get; set; }

        public int TicketsInBearbeitung { get; set; }

        public int GeschlosseneTickets { get; set; }

        public int HohePrioritaet { get; set; }
    }
}
