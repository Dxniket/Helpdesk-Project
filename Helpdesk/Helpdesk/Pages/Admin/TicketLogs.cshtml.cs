using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Helpdesk.Pages.Admin
{
    public class TicketLogsModel : HelpdeskPageBase
    {
        public List<TicketLogModel> Logs { get; set; } = new List<TicketLogModel>();
        public int TicketId { get; set; }
        public string LogsAsJson { get; set; } = "";

        public IActionResult OnGet(int ticketId)
        {
            IActionResult result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            TicketId = ticketId;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<TicketLogListResponse> task = api.GetTicketLogsAsync(ticketId);

                Task.WaitAll(task);

                if (task.Result != null && task.Result.Logs != null)
                {
                    Logs = task.Result.Logs.ToList();
                }

                LogsAsJson = JsonSerializer.Serialize(Logs);
            }

            return Page();
        }
    }
}