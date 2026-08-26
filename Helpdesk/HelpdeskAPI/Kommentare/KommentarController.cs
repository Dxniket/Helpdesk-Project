using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Kommentare.Model;
using HelpdeskAPI.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HelpdeskAPI.Kommentare
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class KommentarController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(KommentarListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetKommentare(int ticketId)
        {
            IEnumerable<KommentarModel> kommentare = HelpdeskConfigurationProvider.GetKommentare(ticketId);

            return Ok(new KommentarListResponse
            {
                Kommentare = kommentare
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult InsertKommentar(KommentarRequest request)
        {
            HelpdeskConfigurationProvider.InsertKommentar(
                request.TicketId,
                request.BenutzerId,
                request.Text);

            return Ok(new SimpleResponse
            {
                Succeeded = true
            });
        }
    }
}