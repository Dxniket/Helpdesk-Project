using HelpdeskAPI.Dashboard.Model;
using HelpdeskAPI.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HelpdeskAPI.Dashboard
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DashboardController : ControllerBase
    {
        [HttpGet]

        [ProducesResponseType(
            typeof(DashboardResponse),
            (int)HttpStatusCode.OK)]

        public IActionResult GetDashboard(
            int benutzerId)
        {
            DashboardResponse response =
                new DashboardResponse();

            response.Dashboard =
                HelpdeskConfigurationProvider.GetDashboard(
                    benutzerId);

            return Ok(response);
        }

        [HttpGet]

        [ProducesResponseType(
            typeof(DashboardTicketListResponse),
            (int)HttpStatusCode.OK)]

        public IActionResult GetDashboardTickets(
            int benutzerId)
        {
            DashboardTicketListResponse response =
                new DashboardTicketListResponse();

            response.Tickets =
                HelpdeskConfigurationProvider
                    .GetDashboardTickets(
                        benutzerId);

            return Ok(response);
        }
    }
}