using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Helpdesk.Pages.Admin
{
    public class AdminPortalModel : HelpdeskPageBase
    {
        public List<BenutzerModel> Benutzer { get; set; } = new List<BenutzerModel>();
        public string InfoMessage { get; set; }
        public string UsersAsJson { get; set; } = "";

        public IActionResult OnGet(string info = null)
        {
            IActionResult result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            InfoMessage = info;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<UserListResponse> task = api.GetAlleBenutzerAsync();

                Task.WaitAll(task);
                
                Benutzer = task.Result.Benutzer.ToList();

                UsersAsJson = JsonSerializer.Serialize(Benutzer);
            }

            return Page();
        }
    }
}