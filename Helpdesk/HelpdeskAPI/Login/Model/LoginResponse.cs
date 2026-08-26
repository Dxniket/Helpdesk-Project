namespace HelpdeskAPI.Login.Model
{
    public class LoginResponse
    {
        public bool LoginSucceeded { get; set; }

        public string ErrorMessage { get; set; }

        public int BenutzerId { get; set; }

        public bool IsAdmin { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }
    }
}
