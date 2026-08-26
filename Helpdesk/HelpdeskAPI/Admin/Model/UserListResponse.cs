using HelpdeskAPI.Benutzer.Model;

namespace HelpdeskAPI.Admin.Model
{
    public class UserListResponse
    {
        public bool Succeeded { get; set; }

        public List<BenutzerModel> Benutzer { get; set; } = new List<BenutzerModel>();
    }
}