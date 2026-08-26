namespace HelpdeskAPI.Profile.Model
{
    public class ProfileDetails
    {
        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class ProfileResponse
    {
        public bool Succeeded { get; set; }

        public ProfileDetails Details { get; set; }
    }

    public class UpdateProfileRequest
    {
        public int BenutzerId { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }
    }

    public class ChangePasswordRequest
    {
        public int BenutzerId { get; set; }

        public string NeuesPasswort { get; set; }
    }
}