using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Benutzer.Model;
using HelpdeskAPI.Password;
using HelpdeskAPI.Provider;
using HelpdeskAPI.Tickets.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static HelpdeskAPI.Provider.HelpdeskConfigurationProvider;

namespace HelpdeskAPI.Admin
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        // Alle Benutzer laden
        [HttpGet]
        [ProducesResponseType(typeof(UserListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetAlleBenutzer()
        {
            List<BenutzerModel> benutzer = HelpdeskConfigurationProvider.GetAlleBenutzer();

            return Ok(new UserListResponse()
            {
                Succeeded = true,
                Benutzer = benutzer
            });
        }

        // Einen Benutzer laden
        [HttpGet]
        [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetBenutzer(int benutzerId)
        {
            BenutzerModel benutzer =
                HelpdeskConfigurationProvider.GetBenutzer(benutzerId);

            return Ok(new UserResponse()
            {
                Succeeded = benutzer != null,
                Benutzer = benutzer
            });
        }

        // Benutzer anlegen / bearbeiten / Nummer und Email prüfen
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult SaveBenutzer(SaveBenutzerRequest request)
        {
            DuplicateCheckResult dup =
                HelpdeskConfigurationProvider.CheckDuplicates(
                    request.Email,
                    request.Telefonnummer);

            if (dup.EmailExists)
            {
                return Ok(new SimpleResponse
                {
                    Succeeded = false,
                    ErrorMessage = "Diese E-Mail existiert bereits."
                });
            }

            if (dup.TelefonExists)
            {
                return Ok(new SimpleResponse
                {
                    Succeeded = false,
                    ErrorMessage = "Diese Telefonnummer existiert bereits."
                });
            }

            HelpdeskConfigurationProvider.SaveBenutzer(request);

            return Ok(new SimpleResponse
            {
                Succeeded = true
            });
        }

        // Benutzer löschen
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult DeleteBenutzer(UserIdRequest request)
        {
            bool ok =
                HelpdeskConfigurationProvider.DeleteBenutzer(request.BenutzerId);

            return Ok(new SimpleResponse()
            {
                Succeeded = ok
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminTicketListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetAlleTickets()
        {
            List<AdminTicketModel> tickets =
                HelpdeskConfigurationProvider.GetAlleTickets();

            return Ok(new AdminTicketListResponse
            {
                Succeeded = true,
                Tickets = tickets
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetAlleAdmins()
        {
            List<BenutzerModel> admins =
                HelpdeskConfigurationProvider.GetAlleAdmins();

            return Ok(new UserListResponse
            {
                Succeeded = true,
                Benutzer = admins
            });
        }
    }
}