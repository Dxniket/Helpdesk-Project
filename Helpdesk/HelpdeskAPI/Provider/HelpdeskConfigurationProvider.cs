using Dapper;
using HelpdeskAPI.Admin.Model;
using HelpdeskAPI.Benutzer.Model;
using HelpdeskAPI.Dashboard.Model;
using HelpdeskAPI.Dateien.Model;
using HelpdeskAPI.Kommentare.Model;
using HelpdeskAPI.Login.Model;
using HelpdeskAPI.Password;
using HelpdeskAPI.Tickets.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpdeskAPI.Provider
{
    public static class HelpdeskConfigurationProvider
    {
        public class DuplicateCheckResult
        {
            public bool EmailExists { get; set; }
            public bool TelefonExists { get; set; }
        }


        // ================= LOGIN =================

        public static LoginResult ValidateLoginFull(string email, string passwortHash)
        {
            LoginResult result = new LoginResult();

            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);
            using SqlCommand cmd = new SqlCommand("pr_Logins_Check", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwortHash);

            cmd.Parameters.Add("@BenutzerId", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@IsAdmin", SqlDbType.Bit).Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@Vorname", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Nachname", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Telefonnummer", SqlDbType.NVarChar, 20).Direction = ParameterDirection.Output;

            conn.Open();
            cmd.ExecuteNonQuery();

            if (cmd.Parameters["@BenutzerId"].Value == DBNull.Value)
            {
                return null;
            }

            result.BenutzerId = Convert.ToInt32(cmd.Parameters["@BenutzerId"].Value);
            result.IsAdmin = Convert.ToBoolean(cmd.Parameters["@IsAdmin"].Value);
            result.Vorname = cmd.Parameters["@Vorname"].Value.ToString();
            result.Nachname = cmd.Parameters["@Nachname"].Value.ToString();
            result.Telefonnummer = cmd.Parameters["@Telefonnummer"].Value.ToString();
            result.Email = email;

            return result;
        }

        // ================= BENUTZER =================

        public static List<BenutzerModel> GetAlleBenutzer()
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            return conn.Query<BenutzerModel>(
                "pr_Benutzer_GET_Alle",
                commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public static BenutzerModel GetBenutzer(int benutzerId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();
            p.Add("@BenutzerId", benutzerId);

            return conn.Query<BenutzerModel>(
                "pr_Benutzer_GET",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public static bool SaveBenutzer(SaveBenutzerRequest benutzer)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);
            using SqlCommand cmd = new SqlCommand("pr_Benutzer_Upsert", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter id = new SqlParameter("@BenutzerId", SqlDbType.Int);

            if (benutzer.BenutzerId == 0)
            {
                id.Value = DBNull.Value;
            }
            else
            {
                id.Value = benutzer.BenutzerId;
            }

            id.Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add(id);

            cmd.Parameters.AddWithValue("@Vorname", benutzer.Vorname);
            cmd.Parameters.AddWithValue("@Nachname", benutzer.Nachname);
            cmd.Parameters.AddWithValue("@Email", benutzer.Email);
            cmd.Parameters.AddWithValue("@Telefonnummer", benutzer.Telefonnummer);
            string hash = PasswortEncryptor.ComputeHash(benutzer.Passwort);

            cmd.Parameters.AddWithValue("@PasswordHash", hash);
            cmd.Parameters.AddWithValue("@IsAdmin", benutzer.IsAdmin);

            conn.Open();
            cmd.ExecuteNonQuery();

            benutzer.BenutzerId = Convert.ToInt32(id.Value);

            return true;
        }

        public static bool DeleteBenutzer(int benutzerId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();
            p.Add("@BenutzerId", benutzerId);

            conn.Execute(
                "pr_Benutzer_Delete",
                p,
                commandType: CommandType.StoredProcedure);

            return true;
        }



        public static DuplicateCheckResult CheckDuplicates(string email, string telefonnummer)
        {
            DuplicateCheckResult result = new DuplicateCheckResult();

            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);
            using SqlCommand cmd = new SqlCommand("pr_Benutzer_CheckDuplicates", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Telefonnummer", telefonnummer);

            cmd.Parameters.Add("@ExistsEmail", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@ExistsTelefon", SqlDbType.Bit).Direction = ParameterDirection.Output;

            conn.Open();
            cmd.ExecuteNonQuery();

            result.EmailExists = Convert.ToBoolean(cmd.Parameters["@ExistsEmail"].Value);
            result.TelefonExists = Convert.ToBoolean(cmd.Parameters["@ExistsTelefon"].Value);

            return result;
        }

        public static bool UpdateBenutzer(BenutzerModel benutzer)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);
            using SqlCommand cmd = new SqlCommand("pr_Benutzer_Upsert", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter id = new SqlParameter("@BenutzerId", SqlDbType.Int);
            id.Value = benutzer.BenutzerId;
            id.Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add(id);

            cmd.Parameters.AddWithValue("@Vorname", benutzer.Vorname);
            cmd.Parameters.AddWithValue("@Nachname", benutzer.Nachname);
            cmd.Parameters.AddWithValue("@Email", benutzer.Email);
            cmd.Parameters.AddWithValue("@Telefonnummer", benutzer.Telefonnummer);

            cmd.Parameters.AddWithValue("@PasswordHash", "");
            cmd.Parameters.AddWithValue("@IsAdmin", benutzer.IsAdmin);

            conn.Open();
            cmd.ExecuteNonQuery();

            return true;
        }

        public static bool UpdatePasswort(int benutzerId, string passwortHash)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@BenutzerId", benutzerId);
            p.Add("@PasswordHash", passwortHash);

            conn.Execute(
                "pr_Logins_UpdatePassword",
                p,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public static int? GetBenutzerIdByEmail(string email)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@Email", email);

            return conn.Query<int?>(
                "pr_Benutzer_GET_BY_Email",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public static int? GetBenutzerIdByTelefon(string telefonnummer)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@Telefon", telefonnummer);

            return conn.Query<int?>(
                "pr_Benutzer_GET_BY_Telefon",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        //TICKETS JETZT:

        public static int CreateTicket(CreateTicketRequest ticket)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);
            using SqlCommand cmd = new SqlCommand("pr_Tickets_Insert", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@BenutzerId", ticket.BenutzerId);
            cmd.Parameters.AddWithValue("@Titel", ticket.Titel);
            cmd.Parameters.AddWithValue("@Beschreibung", ticket.Beschreibung);

            SqlParameter ticketId = new SqlParameter("@TicketId", SqlDbType.Int);

            ticketId.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(ticketId);

            conn.Open();

            cmd.ExecuteNonQuery();
            // gibt die neu erzeugte TicketId zurück
            return Convert.ToInt32(ticketId.Value);
        }

        public static List<TicketModel> GetMeineTickets(int benutzerId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@BenutzerId", benutzerId);

            return conn.Query<TicketModel>(
                "pr_Tickets_GET_ByBenutzer",
                p,
                commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public static List<AdminTicketModel> GetAlleTickets()
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            return conn.Query<AdminTicketModel>(
                "pr_Tickets_GET_Alle",
                commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public static TicketModel GetTicket(int ticketId, int benutzerId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);
            p.Add("@BenutzerId", benutzerId);

            return conn.Query<TicketModel>(
                "pr_Tickets_GET",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }


        public static BenutzerStatistikModel GetBenutzerStatistik(int benutzerId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@BenutzerId", benutzerId);

            return conn.Query<BenutzerStatistikModel>(
                "pr_Tickets_GET_Statistik",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }



        public static AdminTicketModel GetAdminTicket(int ticketId)
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);

            return conn.Query<AdminTicketModel>(
                "pr_Tickets_GET_Admin",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public static bool AssignBearbeiter(int ticketId, int bearbeiterId)
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);
            p.Add("@BearbeiterId", bearbeiterId);

            conn.Execute(
                "pr_Tickets_AssignBearbeiter",
                p,
                commandType: CommandType.StoredProcedure);

            return true;
        }


        public static bool UpdateStatus(int ticketId, int statusId)
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);
            p.Add("@StatusId", statusId);

            conn.Execute(
                "pr_Tickets_UpdateStatus",
                p,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        public static bool UpdatePriority(int ticketId, int priorityId)
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);
            p.Add("@PriorityId", priorityId);

            conn.Execute(
                "pr_Tickets_UpdatePriority",
                p,
                commandType: CommandType.StoredProcedure);

            return true;
        }

        //admins holen
        public static List<BenutzerModel> GetAlleAdmins()
        {
            using SqlConnection conn =
                new SqlConnection(GlobalConfigurationProvider.DBConnString);

            return conn.Query<BenutzerModel>(
                "pr_Benutzer_GET_AlleAdmins",
                commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public static bool SaveAdminTicket(SaveAdminTicketRequest request)
        {
            using SqlConnection connection = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            connection.Open();

            connection.Execute(
                "pr_Tickets_Update",
                new
                {
                    request.TicketId,
                    request.BearbeiterId,
                    request.StatusId,
                    request.PriorityId
                },
                commandType: CommandType.StoredProcedure);

            return true;
        }

        //TicketLogs und weitere Ticket Methoden

        public static bool UpdateTicket(UpdateTicketRequest request)
        {
            using SqlConnection conn =
                new SqlConnection(
                    GlobalConfigurationProvider.DBConnString);

            DynamicParameters p =
                new DynamicParameters();

            p.Add("@TicketId", request.TicketId);
            p.Add("@Titel", request.Titel);
            p.Add("@Beschreibung", request.Beschreibung);

            conn.Execute(
                "pr_Tickets_UpdateBenutzer",
                p,
                commandType:
                CommandType.StoredProcedure);

            return true;
        }

        public static List<TicketLogModel> GetTicketLogs(int ticketId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);

            return conn.Query<TicketLogModel>("pr_TicketLogs_GET_ByTicket", p, commandType: CommandType.StoredProcedure).ToList();
        }


        //DASHBOARD
        public static DashboardModel GetDashboard(int benutzerId)
        {
            using SqlConnection connection = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            return connection.QueryFirst<DashboardModel>(
                "pr_Dashboard_Get",
                new
                {
                    BenutzerId = benutzerId
                },
                commandType: CommandType.StoredProcedure);
        }

        public static DashboardTicketModel[] GetDashboardTickets(int benutzerId)
        {
            using SqlConnection connection = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            return connection.Query<DashboardTicketModel>(
                "pr_Dashboard_GetLastTickets",
                new
                {
                    BenutzerId = benutzerId
                },
                commandType: CommandType.StoredProcedure)
                .ToArray();
        }


        //Kommentare

        public static IEnumerable<KommentarModel> GetKommentare(int ticketId)
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfigurationProvider.DBConnString))
            {
                return connection.Query<KommentarModel>(
                    "pr_Kommentare_Get",
                    new
                    {
                        TicketId = ticketId
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public static void InsertKommentar(int ticketId, int benutzerId, string text)
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfigurationProvider.DBConnString))
            {
                connection.Execute(
                    "pr_Kommentare_Insert",
                    new
                    {
                        TicketId = ticketId,
                        BenutzerId = benutzerId,
                        Text = text
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }




        //datei upload und download funktion
        public static void InsertDatei(int ticketId, string dateiname, string contentType, byte[] datei)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);
            p.Add("@Dateiname", dateiname);
            p.Add("@ContentType", contentType);
            p.Add("@Datei", datei);

            conn.Execute(
                "pr_Dateien_Insert",
                p,
                commandType: CommandType.StoredProcedure);
        }

        public static List<DateiModel> GetDateien(int ticketId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@TicketId", ticketId);

            return conn.Query<DateiModel>(
                "pr_Dateien_Get",
                p,
                commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public static DateiDownloadModel DownloadDatei(int dateiId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            DynamicParameters p = new DynamicParameters();

            p.Add("@DateiId", dateiId);

            return conn.Query<DateiDownloadModel>(
                "pr_Datei_Download",
                p,
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public static bool DeleteDatei(int dateiId)
        {
            using SqlConnection conn = new SqlConnection(GlobalConfigurationProvider.DBConnString);

            using SqlCommand cmd = new SqlCommand("pr_Dateien_Delete", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DateiId", dateiId);

            conn.Open();

            cmd.ExecuteNonQuery();

            return true;
        }


    }
}