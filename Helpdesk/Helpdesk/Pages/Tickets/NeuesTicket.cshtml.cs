using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Helpdesk.Pages.Tickets
{
    public class NeuesTicketModel : HelpdeskPageBase
    {
        public string Titel { get; set; } = "";

        public string Beschreibung { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public List<IFormFile> Dateien { get; set; } = new List<IFormFile>();
        public IActionResult OnGet()
        {
            IActionResult? result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult? result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            Titel = Request.Form["Titel"];
            Beschreibung = Request.Form["Beschreibung"];
            Dateien = Request.Form.Files.ToList();
            if (Dateien.Count > 3)
            {
                ErrorMessage = "Es dürfen maximal 3 Dateien hochgeladen werden.";
                return Page();
            }

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api =
                    new HelpdeskApiClient(
                        HelpdeskConfigProvider.APIBaseURL,
                        client);

                CreateTicketRequest request =
                    new CreateTicketRequest();

                request.BenutzerId = UserId.Value;
                request.Titel = Titel;
                request.Beschreibung = Beschreibung;

                Task<CreateTicketResponse> task =
                    api.CreateTicketAsync(request);

                Task.WaitAll(task);

                CreateTicketResponse response =
    task.Result;

                if (!response.Succeeded)
                {
                    ErrorMessage = response.ErrorMessage;
                    return Page();
                }

                // Neue Ticket-ID merken
                int ticketId = response.TicketId;

                // Alle ausgewählten Dateien hochladen
                foreach (IFormFile datei in Dateien)
                {
                    using MemoryStream ms = new MemoryStream();

                    // Datei in den Arbeitsspeicher kopieren
                    datei.CopyTo(ms);

                    // Stream wieder an den Anfang setzen
                    ms.Position = 0;

                    FileParameter file = new FileParameter(
                        ms,
                        datei.FileName,
                        datei.ContentType);

                    api.InsertDateiAsync(ticketId, file).Wait();
                }
            }

            return RedirectToPage(
                "/Tickets/MeineTickets",
                new
                {
                    info = "Ticket wurde erfolgreich erstellt."
                });
        }
    }
}