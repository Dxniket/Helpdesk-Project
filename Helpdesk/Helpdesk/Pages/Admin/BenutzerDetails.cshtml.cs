using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Pages.Admin
{
    public class BenutzerDetailsModel : HelpdeskPageBase
    {
        public BenutzerModel Benutzer { get; set; }

        public BenutzerStatistikModel Statistik { get; set; }

        public string InfoMessage { get; set; }

        public IActionResult OnGet(int benutzerId, string info = null)
        {
            IActionResult? result = RequireAdmin();

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

                Task<UserResponse> benutzerTask =
                    api.GetBenutzerAsync(benutzerId);

                Task<BenutzerStatistikResponse> statistikTask =
                    api.GetBenutzerStatistikAsync(benutzerId);

                Task.WaitAll(
                    benutzerTask,
                    statistikTask);

                Benutzer = benutzerTask.Result.Benutzer;

                Statistik = statistikTask.Result.Statistik;
            }

            if (Benutzer == null)
            {
                return RedirectToPage("/Admin/AdminPortal");
            }

            return Page();
        }

        public IActionResult OnPostDelete(int benutzerId)
        {
            bool succeeded;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api =
                    new HelpdeskApiClient(
                        HelpdeskConfigProvider.APIBaseURL,
                        client);

                UserIdRequest request = new UserIdRequest()
                {
                    BenutzerId = benutzerId
                };

                Task<SimpleResponse> task =
                    api.DeleteBenutzerAsync(request);

                Task.WaitAll(task);

                succeeded = task.Result.Succeeded;
            }

            return RedirectToPage("/Admin/AdminPortal", new
            {
                info = succeeded
                    ? "Benutzer wurde erfolgreich gelöscht."
                    : "Benutzer konnte nicht gelöscht werden."
            });
        }
    }
}