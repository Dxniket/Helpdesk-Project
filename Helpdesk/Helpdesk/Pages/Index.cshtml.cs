using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;

namespace Helpdesk.Pages
{
    public class IndexModel : HelpdeskPageBase
    {
        public bool IsLoggedIn { get; set; }

        public string Vorname { get; set; } = "";

        public string Nachname { get; set; } = "";

        public DashboardModel Dashboard { get; set; } = new DashboardModel();

        public List<DashboardTicketModel> LetzteTickets { get; set; } = new List<DashboardTicketModel>();

        public void OnGet()
        {
            int? benutzerId = GetUserIdFromRequest(Request);

            IsLoggedIn = benutzerId != null;

            if (!IsLoggedIn)
            {
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<ProfileResponse> profileTask = api.GetProfileAsync(UserId.Value);

                Task<DashboardResponse> dashboardTask = api.GetDashboardAsync(UserId.Value);

                Task<DashboardTicketListResponse> ticketsTask = api.GetDashboardTicketsAsync(UserId.Value);

                Task.WaitAll(profileTask, dashboardTask, ticketsTask);

                Vorname = profileTask.Result.Details.Vorname;

                Nachname = profileTask.Result.Details.Nachname;

                Dashboard = dashboardTask.Result.Dashboard;

                LetzteTickets = ticketsTask.Result.Tickets.ToList();
            }
        }
    }
}