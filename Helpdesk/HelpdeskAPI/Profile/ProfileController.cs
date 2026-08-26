using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Benutzer.Model;
using HelpdeskAPI.Password;
using HelpdeskAPI.Profile.Model;
using HelpdeskAPI.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static HelpdeskAPI.Provider.HelpdeskConfigurationProvider;

namespace HelpdeskAPI.Profile
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProfileResponse), (int)HttpStatusCode.OK)]
        public IActionResult GetProfile(int benutzerId)
        {
            BenutzerModel benutzer =
                HelpdeskConfigurationProvider.GetBenutzer(benutzerId);

            if (benutzer == null)
            {
                return Ok(new ProfileResponse()
                {
                    Succeeded = false
                });
            }

            ProfileDetails details = new ProfileDetails()
            {
                BenutzerId = benutzer.BenutzerId,
                Vorname = benutzer.Vorname,
                Nachname = benutzer.Nachname,
                Email = benutzer.Email,
                Telefonnummer = benutzer.Telefonnummer,
                IsAdmin = benutzer.IsAdmin
            };

            return Ok(new ProfileResponse()
            {
                Succeeded = true,
                Details = details
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProfile(UpdateProfileRequest request)
        {
            DuplicateCheckResult duplicate =
                HelpdeskConfigurationProvider.CheckDuplicates(
                    request.Email,
                    request.Telefonnummer);

            if (duplicate.EmailExists)
            {
                int? owner =
                    HelpdeskConfigurationProvider.GetBenutzerIdByEmail(request.Email);

                if (owner != null &&
                    owner.Value != request.BenutzerId)
                {
                    return Ok(new SimpleResponse()
                    {
                        Succeeded = false,
                        ErrorMessage = "Diese E-Mail wird bereits verwendet."
                    });
                }
            }

            if (duplicate.TelefonExists)
            {
                int? owner =
                    HelpdeskConfigurationProvider.GetBenutzerIdByTelefon(request.Telefonnummer);

                if (owner != null &&
                    owner.Value != request.BenutzerId)
                {
                    return Ok(new SimpleResponse()
                    {
                        Succeeded = false,
                        ErrorMessage = "Diese Telefonnummer wird bereits verwendet."
                    });
                }
            }

            BenutzerModel benutzer = new BenutzerModel();

            benutzer.BenutzerId = request.BenutzerId;
            benutzer.Vorname = request.Vorname;
            benutzer.Nachname = request.Nachname;
            benutzer.Email = request.Email;
            benutzer.Telefonnummer = request.Telefonnummer;

            bool ok =
                HelpdeskConfigurationProvider.UpdateBenutzer(benutzer);

            return Ok(new SimpleResponse()
            {
                Succeeded = ok
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            string hash =
                PasswortEncryptor.ComputeHash(request.NeuesPasswort);

            bool ok =
                HelpdeskConfigurationProvider.UpdatePasswort(
                    request.BenutzerId,
                    hash);

            return Ok(new SimpleResponse()
            {
                Succeeded = ok
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(SimpleResponse), (int)HttpStatusCode.OK)]
        public IActionResult DeleteAccount(UserIdRequest request)
        {
            bool ok =
                HelpdeskConfigurationProvider.DeleteBenutzer(
                    request.BenutzerId);

            return Ok(new SimpleResponse()
            {
                Succeeded = ok
            });
        }
    }
}