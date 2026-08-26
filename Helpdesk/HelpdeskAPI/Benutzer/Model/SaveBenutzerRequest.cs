namespace HelpdeskAPI.Benutzer.Model
{
    public class SaveBenutzerRequest
    {
        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }

        public string Passwort { get; set; }

        public bool IsAdmin { get; set; }
    }
}
