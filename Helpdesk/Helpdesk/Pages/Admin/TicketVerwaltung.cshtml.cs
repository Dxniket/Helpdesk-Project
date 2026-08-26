using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Helpdesk.Pages.Admin
{
    public class TicketVerwaltungModel : HelpdeskPageBase
    {
        public List<AdminTicketModel> Tickets { get; set; } =
    new List<AdminTicketModel>();

        public string TicketsAsJson { get; set; } = "";

        public IActionResult OnGet()
        {
            IActionResult result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api =
                    new HelpdeskApiClient(
                        HelpdeskConfigProvider.APIBaseURL,
                        client);

                Task<AdminTicketListResponse> task =
     api.GetAlleTicketsAsync();

                Task.WaitAll(task);

                if (task.Result != null &&
                    task.Result.Tickets != null)
                {
                    Tickets = task.Result.Tickets.ToList();
                }

                TicketsAsJson =
                    JsonSerializer.Serialize(Tickets);
            }

            return Page();
        }
    }
}