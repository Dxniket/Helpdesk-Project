using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Dateien.Model;
using HelpdeskAPI.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HelpdeskAPI.Dateien
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DateiController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(DateiListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetDateien(int ticketId)
        {
            List<DateiModel> dateien = HelpdeskConfigurationProvider.GetDateien(ticketId);

            return Ok(new DateiListResponse
            {
                Dateien = dateien
            });
        }


        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult InsertDatei([FromForm] int ticketId, IFormFile datei)
        {
            // maximale erlaubte dateigröße (50 MB)
            const long MaxDateigroesse = 50 * 1024 * 1024;

            // prüft ob hochgeladene datei größer als 50 MB ist
            if (datei.Length > MaxDateigroesse)
            {
                // gibt fehlermeldung zurück wenn datei zu groß ist
                return BadRequest(new
                {
                    errorMessage = "Die Datei darf maximal 50 MB groß sein."
                });
            }

            // erstellt einen arbeitsspeicher in den die hochgeladene datei kopiert wird
            using MemoryStream ms = new MemoryStream();

            // kopiert den kompletten inhalt der hochgeladenen datei in den MemoryStream
            datei.CopyTo(ms);

            // wandelt Inhalt des MemoryStreams in ein Byte-Array um
            // damit die datei in der datenbank gespeichert werden kann
            byte[] bytes = ms.ToArray();


            HelpdeskConfigurationProvider.InsertDatei(
                ticketId,
                datei.FileName,
                datei.ContentType,
                bytes);


            return Ok(new SimpleResponse
            {
                Succeeded = true
            });
        }

        [HttpGet]
        public IActionResult DownloadDatei(int dateiId)
        {
            DateiDownloadModel datei = HelpdeskConfigurationProvider.DownloadDatei(dateiId);

            if (datei == null)
            {
                return NotFound();
            }

            return File(datei.Datei, datei.ContentType, datei.Dateiname);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult DeleteDatei(int dateiId)
        {
            bool ok = HelpdeskConfigurationProvider.DeleteDatei(dateiId);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }
    }
}