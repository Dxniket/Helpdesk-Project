using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Pages.Profile
{
    public class MeinProfilModel : HelpdeskPageBase
    {
        public ProfileDetails Daten { get; set; } = new ProfileDetails();

        public string InfoMessage { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            IActionResult result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            LoadProfile();

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                UpdateProfileRequest request = new UpdateProfileRequest();

                request.BenutzerId = GetUserIdFromRequest(Request).Value;

                request.Vorname = Request.Form["Vorname"];

                request.Nachname = Request.Form["Nachname"];

                request.Email = Request.Form["Email"];

                request.Telefonnummer = Request.Form["Telefonnummer"];

                Task<SimpleResponse> task = api.UpdateProfileAsync(request);

                Task.WaitAll(task);

                SimpleResponse response = task.Result;

                if (!response.Succeeded)
                {
                    ErrorMessage = response.ErrorMessage;
                }
                else
                {
                    InfoMessage = "Profil erfolgreich gespeichert.";
                }
            }

            LoadProfile();

            return Page();
        }

        public IActionResult OnPostPassword()
        {
            IActionResult result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                ChangePasswordRequest request = new ChangePasswordRequest();

                request.BenutzerId = GetUserIdFromRequest(Request).Value;

                request.NeuesPasswort = Request.Form["NeuesPasswort"];

                Task<SimpleResponse> task = api.ChangePasswordAsync(request);

                Task.WaitAll(task);

                SimpleResponse response = task.Result;

                if (!response.Succeeded)
                {
                    ErrorMessage = response.ErrorMessage;
                }
                else
                {
                    InfoMessage = "Passwort erfolgreich geändert.";
                }
            }

            LoadProfile();

            return Page();
        }

        public IActionResult OnPostDelete()
        {
            IActionResult result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            using (HttpClient client = new HttpClient())
            {


                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                UserIdRequest request = new UserIdRequest();


                request.BenutzerId = GetUserIdFromRequest(Request).Value;



                Task<SimpleResponse> task = api.DeleteAccountAsync(request);

                Task.WaitAll(task);

                SimpleResponse response = task.Result;

                if (response.Succeeded)
                {
                    Response.Cookies.Delete("Benutzer");
                    Response.Cookies.Delete("Berechtigung");

                    return RedirectToPage("/Index");
                }

                ErrorMessage = "Konto konnte nicht gelöscht werden.";
            }

            LoadProfile();

            return Page();
        }

        private void LoadProfile()
        {
            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<ProfileResponse> task = api.GetProfileAsync(GetUserIdFromRequest(Request).Value);

                Task.WaitAll(task);

                ProfileResponse response = task.Result;

                if (response != null && response.Details != null)
                {
                    Daten = response.Details;
                }
            }
        }
    }
}