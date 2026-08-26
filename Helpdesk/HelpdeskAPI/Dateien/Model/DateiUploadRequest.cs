using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpdeskAPI.Dateien.Model
{
    public class DateiUploadRequest
    {
        public int TicketId { get; set; }

        public IFormFile Datei { get; set; }
    }
}