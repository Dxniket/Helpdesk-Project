using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Benutzer.Model;
using HelpdeskAPI.Provider;
using HelpdeskAPI.Tickets.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HelpdeskAPI.Tickets
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TicketController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(CreateTicketResponse), (int)HttpStatusCode.OK)]
        public IActionResult CreateTicket(CreateTicketRequest request)
        {
            int ticketId = HelpdeskConfigurationProvider.CreateTicket(request);

            return Ok(new CreateTicketResponse
            {
                Succeeded = true,
                TicketId = ticketId
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(TicketListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetMeineTickets(int benutzerId)
        {
            List<TicketModel> tickets = HelpdeskConfigurationProvider.GetMeineTickets(benutzerId);

            return Ok(new TicketListResponse
            {
                Succeeded = true,
                Tickets = tickets
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminTicketListResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetAlleTickets()
        {
            List<AdminTicketModel> tickets = HelpdeskConfigurationProvider.GetAlleTickets();

            return Ok(new AdminTicketListResponse
            {
                Succeeded = true,
                Tickets = tickets
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(TicketResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetTicket(int ticketId, int benutzerId)
        {
            TicketModel ticket =
                HelpdeskConfigurationProvider.GetTicket(ticketId, benutzerId);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(new TicketResponse
            {
                Succeeded = true,
                Ticket = ticket
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(BenutzerStatistikResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetBenutzerStatistik(int benutzerId)
        {
            BenutzerStatistikModel statistik = HelpdeskConfigurationProvider.GetBenutzerStatistik(benutzerId);

            return Ok(new BenutzerStatistikResponse
            {
                Succeeded = true,
                Statistik = statistik
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminTicketResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetAdminTicket(int ticketId)
        {
            AdminTicketModel ticket = HelpdeskConfigurationProvider.GetAdminTicket(ticketId);

            return Ok(new AdminTicketResponse
            {
                Succeeded = ticket != null,
                Ticket = ticket
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult AssignBearbeiter(AssignBearbeiterRequest request)
        {
            bool ok = HelpdeskConfigurationProvider.AssignBearbeiter(request.TicketId, request.BearbeiterId);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult UpdateStatus(UpdateStatusRequest request)
        {
            bool ok = HelpdeskConfigurationProvider.UpdateStatus(request.TicketId, request.StatusId);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult UpdatePriority(UpdatePriorityRequest request)
        {
            bool ok = HelpdeskConfigurationProvider.UpdatePriority(request.TicketId, request.PriorityId);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }
        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult SaveAdminTicket(SaveAdminTicketRequest request)
        {
            bool ok = HelpdeskConfigurationProvider.SaveAdminTicket(request);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }


        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse),(int)HttpStatusCode.OK)]
        public IActionResult UpdateTicket(UpdateTicketRequest request)
        {
            bool ok = HelpdeskConfigurationProvider.UpdateTicket(request);

            return Ok(new SimpleResponse
            {
                Succeeded = ok
            });
        }


        [HttpGet]
        [ProducesResponseType(typeof(TicketLogListResponse),(int)HttpStatusCode.OK)]
        public IActionResult GetTicketLogs(int ticketId)
        {
            List<TicketLogModel> logs = HelpdeskConfigurationProvider.GetTicketLogs(ticketId);

            return Ok(new TicketLogListResponse
            {
                Succeeded = true,
                Logs = logs
            });
        }

    }
}