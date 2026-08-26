using HelpdeskAPI.Benutzer.Model;

namespace HelpdeskAPI.Admin.Model
{
    public class UserResponse
    {
        public bool Succeeded { get; set; }

        public BenutzerModel Benutzer { get; set; }
    }
}