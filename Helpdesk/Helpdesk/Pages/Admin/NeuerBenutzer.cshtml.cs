using Helpdesk.Provider;
using Helpdesk.Pages.Base;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Pages.Admin
{
    public class NeuerBenutzerModel : HelpdeskPageBase
    {
        [ViewData]
        public string ErrorMessage { get; set; }

        public SaveBenutzerRequest Benutzer { get; set; } = new SaveBenutzerRequest();

        public IActionResult OnGet()
        {
            IActionResult result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            Benutzer.BenutzerId = 0;
            Benutzer.Vorname = Request.Form["txtVorname"];
            Benutzer.Nachname = Request.Form["txtNachname"];
            Benutzer.Email = Request.Form["txtEmail"];
            Benutzer.Telefonnummer = Request.Form["txtTelefonnummer"];
            Benutzer.Passwort = Request.Form["txtPasswort"];
            Benutzer.IsAdmin = Request.Form["chkAdmin"] == "on";

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api =
                    new HelpdeskApiClient(
                        HelpdeskConfigProvider.APIBaseURL,
                        client);

                Task<SimpleResponse> task =
                    api.SaveBenutzerAsync(Benutzer);

                Task.WaitAll(task);

                if (!task.Result.Succeeded)
                {
                    ErrorMessage = task.Result.ErrorMessage;
                    return Page();
                }
            }

            return RedirectToPage("/Admin/AdminPortal");
        }
    }
}