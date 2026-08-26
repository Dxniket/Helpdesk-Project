using HelpdeskAPI.Login.Model;
using HelpdeskAPI.Password;
using HelpdeskAPI.Provider;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HelpdeskAPI.Login
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public IActionResult ValidateLogin(LoginRequest request)
        {
            string hash = PasswortEncryptor.ComputeHash(request.Passwort);

            LoginResult result = HelpdeskConfigurationProvider.ValidateLoginFull(request.Email,hash);

            if (result == null)
            {
                return Ok(new LoginResponse
                {
                    LoginSucceeded = false,
                    ErrorMessage = "Benutzername oder Passwort falsch."
                });
            }

            return Ok(new LoginResponse
            {
                LoginSucceeded = true,
                BenutzerId = result.BenutzerId.Value,
                IsAdmin = result.IsAdmin,
                Vorname = result.Vorname,
                Nachname = result.Nachname,
                Email = result.Email,
                Telefonnummer = result.Telefonnummer
            });
        }
    }
}