using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Helpdesk.Pages.Base
{


    public class HelpdeskPageBase : PageModel
    {
        private const string BenutzerCookieKey = "Benutzer";
        private const string AdminCookieKey = "Berechtigung";

        public static int? GetUserIdFromRequest(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey(BenutzerCookieKey))
            {
                return null;
            }

            string cookie = request.Cookies[BenutzerCookieKey];

            if (string.IsNullOrEmpty(cookie))
            {
                return null;
            }

            byte[] bytes = Convert.FromBase64String(cookie);
            string decoded = Encoding.UTF8.GetString(bytes);

            return int.Parse(decoded);
        }

        public static bool GetIsAdminFromRequest(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey(AdminCookieKey))
            {
                return false;
            }

            string cookie = request.Cookies[AdminCookieKey];

            if (string.IsNullOrEmpty(cookie))
            {
                return false;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(cookie);
                string decoded = Encoding.UTF8.GetString(bytes);

                return bool.Parse(decoded);
            }
            catch
            {
                return false;
            }
        }

        public bool UserIsValidated
        {
            get
            {
                return UserId != null;
            }
        }
       
        public int? UserId
        {
            get
            {
                return GetUserIdFromRequest(Request);
            }
            set
            {
                if (value == null)
                {
                    Response.Cookies.Append(BenutzerCookieKey, "", new CookieOptions
                    {
                        Expires = DateTimeOffset.Now.AddSeconds(-10)
                    });

                    return;
                }

                byte[] bytes = Encoding.UTF8.GetBytes(value.ToString()!);
                string encoded = Convert.ToBase64String(bytes);

                Response.Cookies.Append(BenutzerCookieKey, encoded, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddHours(2)
                });
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (!Request.Cookies.ContainsKey(AdminCookieKey))
                {
                    return false;
                }

                string cookie = Request.Cookies[AdminCookieKey];

                if (string.IsNullOrEmpty(cookie))
                {
                    return false;
                }

                try
                {
                    byte[] bytes = Convert.FromBase64String(cookie);
                    string decoded = Encoding.UTF8.GetString(bytes);

                    return bool.Parse(decoded);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());
                string encoded = Convert.ToBase64String(bytes);

                Response.Cookies.Append(AdminCookieKey, encoded, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddHours(2)
                });
            }
        }

        public void ClearAllUserCookies()
        {
            Response.Cookies.Append(BenutzerCookieKey, "", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddSeconds(-10)
            });

            Response.Cookies.Append(AdminCookieKey, "", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddSeconds(-10)
            });
        }

        public IActionResult? RequireLogin()
        {
            if (UserId == null)
            {
                return RedirectToPage("/Log/LogIn");
            }

            return null;
        }

        public IActionResult? RequireAdmin()
        {
            IActionResult? login = RequireLogin();

            if (login != null)
            {
                return login;
            }

            if (!IsAdmin)
            {
                return RedirectToPage("/AccessDenied");
            }

            return null;
        }
    }
}

