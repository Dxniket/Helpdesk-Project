using Helpdesk.Pages.Base;
using Helpdesk.Provider;
using HelpdeskAPIClient;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Pages.Tickets
{
    public class TicketDetailsModel : HelpdeskPageBase
    {
        public List<KommentarModel> Kommentare { get; set; } = new List<KommentarModel>();
        public TicketModel? Ticket { get; set; }
        public bool EditMode { get; set; }
        public string InfoMessage { get; set; } = "";

        //paging für Kommentare
        public int Seite { get; set; }

        public int SeitenAnzahl { get; set; }

        private const int KommentareProSeite = 4;
        //------------------------

        //DATEIEN

        public List<DateiModel> Dateien { get; set; } = new List<DateiModel>();

        //--------

        public IActionResult OnGet(int ticketId, string info = null, bool edit = false, int seite = 1)
        {
            IActionResult? result = RequireLogin();

            if (result != null)
            {
                return result;
            }

            InfoMessage = info;
            EditMode = edit;

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                try
                {
                    TicketResponse ticketResponse = api.GetTicketAsync(ticketId, UserId.Value).Result;

                    Ticket = ticketResponse.Ticket;
                }
                catch (AggregateException ex)
                {
                    //bei error 404 also ticket nicht gefunden -> catch Exeption und Redirect 
                    if (ex.InnerException is ApiException apiEx && apiEx.StatusCode == 404)
                    {
                        return RedirectToPage("/AccessDenied");
                    }

                    throw;
                }

                Task<KommentarListResponse> kommentarTask = api.GetKommentareAsync(ticketId);

                Task<DateiListResponse> dateiTask = api.GetDateienAsync(ticketId);

                Task.WaitAll(kommentarTask, dateiTask);

                Kommentare = kommentarTask.Result.Kommentare.ToList();

                Dateien = dateiTask.Result.Dateien.ToList();



                Seite = seite;

                SeitenAnzahl =
                    (int)Math.Ceiling((double)Kommentare.Count / KommentareProSeite);

                Kommentare = Kommentare
                    .Skip((Seite - 1) * KommentareProSeite)
                    .Take(KommentareProSeite)
                    .ToList();
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

            UpdateTicketRequest request = new UpdateTicketRequest
            {
                TicketId = Convert.ToInt32(Request.Form["TicketId"]),
                Titel = Request.Form["Titel"],
                Beschreibung = Request.Form["Beschreibung"]
            };

            using (HttpClient client = new HttpClient())
            {
                HelpdeskApiClient api = new HelpdeskApiClient(HelpdeskConfigProvider.APIBaseURL, client);

                SimpleResponse response = api.UpdateTicketAsync(request).Result;

                if (!response.Succeeded)
                {
                    return RedirectToPage("/Tickets/TicketDetails",
                        new
                        {
                            ticketId = request.TicketId,
                            info = "Ticket konnte nicht gespeichert werden."
                        });
                }
            }

            return RedirectToPage("/Tickets/TicketDetails",
                new
                {
                    ticketId = request.TicketId,
                    info = "Ticket wurde erfolgreich gespeichert."
                });
        }
    }
}