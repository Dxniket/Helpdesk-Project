using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Helpdesk.Pages.Tickets
{
    public class MeineTicketsModel : HelpdeskPageBase
    {
        public List<TicketModel> Tickets { get; set; } =
            new List<TicketModel>();

        public string TicketsAsJson { get; set; } = "";

        public string InfoMessage { get; set; } = "";

        public IActionResult OnGet(string? info)
        {
            IActionResult? result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            InfoMessage = info;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api =
                    new HelpdeskApiClient(
                        HelpdeskConfigProvider.APIBaseURL,
                        client);

                Task<TicketListResponse> task =
                    api.GetMeineTicketsAsync(UserId.Value);

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