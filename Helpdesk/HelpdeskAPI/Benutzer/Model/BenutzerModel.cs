namespace HelpdeskAPI.Benutzer.Model
{
    public class BenutzerModel
    {
        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }

        public bool IsAdmin { get; set; }
    }
}
