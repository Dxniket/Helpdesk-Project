using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Pages.Admin
{
    public class AdminTicketDetailsModel : HelpdeskPageBase
    {
        public AdminTicketModel Ticket { get; set; }

        public string InfoMessage { get; set; } = "";
        public List<KommentarModel> Kommentare { get; set; } = new List<KommentarModel>();
        public List<BenutzerModel> Benutzer { get; set; } = new List<BenutzerModel>();
        public List<DateiModel> Dateien { get; set; } = new List<DateiModel>();

        //KOMMENTAR PAGING:
        public int Seite { get; set; }

        public int SeitenAnzahl { get; set; }

        private const int KommentareProSeite = 4;

        //---------------------
        public IActionResult OnGet(int ticketId, string info = null, int seite = 1)
        {


            IActionResult? result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            InfoMessage = info;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<AdminTicketResponse> ticketTask = api.GetAdminTicketAsync(ticketId);

                Task<UserListResponse> userTask = api.GetAlleAdminsAsync();

                Task<KommentarListResponse> kommentarTask = api.GetKommentareAsync(ticketId);

                Task<DateiListResponse> dateiTask = api.GetDateienAsync(ticketId);

                Task.WaitAll(ticketTask, userTask, kommentarTask, dateiTask);

                Ticket = ticketTask.Result.Ticket;

                Benutzer = userTask.Result.Benutzer.ToList();

                Kommentare = kommentarTask.Result.Kommentare.ToList();
                Dateien = dateiTask.Result.Dateien.ToList();

                Seite = seite;
                // Berechnet, wie viele Seiten insgesamt benötigt werden.
                // Math.Ceiling rundet immer auf die nächste ganze Zahl auf.
                // Beispiel: 12 Kommentare / 5 pro Seite = 2,4 ? 3 Seiten.
                SeitenAnzahl = (int)Math.Ceiling((double)Kommentare.Count / KommentareProSeite);

                Kommentare = Kommentare
                    .Skip((Seite - 1) * KommentareProSeite)
                    .Take(KommentareProSeite)
                    .ToList();
            }

            if (Ticket == null)
            {
                return RedirectToPage("/Admin/TicketVerwaltung");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult? result = RequireAdmin();

            if (result != null)
            {
                return result;
            }

            SaveAdminTicketRequest request = new SaveAdminTicketRequest();

            request.TicketId = Convert.ToInt32(Request.Form["TicketId"]);



            // Geschlossene Tickets dürfen nicht mehr bearbeitet werden
            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<AdminTicketResponse> ticketTask = api.GetAdminTicketAsync(request.TicketId);

                Task.WaitAll(ticketTask);

                if (ticketTask.Result.Ticket.StatusId == 3)
                {
                    return RedirectToPage(
                        "/Admin/AdminTicketDetails",
                        new
                        {
                            ticketId = request.TicketId,
                            info = "Geschlossene Tickets können nicht mehr bearbeitet werden."
                        });
                }
            }

            if (!string.IsNullOrEmpty(Request.Form["BearbeiterId"]))
            {
                request.BearbeiterId = Convert.ToInt32(Request.Form["BearbeiterId"]);
            }

            request.StatusId = Convert.ToInt32(Request.Form["StatusId"]);

            request.PriorityId =Convert.ToInt32(Request.Form["PriorityId"]);

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                Task<SimpleResponse> task = api.SaveAdminTicketAsync(request);

                Task.WaitAll(task);
            }

            return RedirectToPage(
                "/Admin/AdminTicketDetails",
                new
                {
                    ticketId = request.TicketId,
                    info = "Änderungen wurden erfolgreich gespeichert."
                });
        }
    }
}