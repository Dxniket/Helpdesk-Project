using Microsoft.Extensions.Configuration;

namespace HelpdeskAPI.Provider
{
    public static class GlobalConfigurationProvider
    {
        public static IConfiguration Configuration = null!;

        public static string DBConnString = @"Server=...";

        private static string SectionName
        {
            get
            {
#if DEBUG
                return "Settings_Debug";
#else
                return "Settings_Release";
#endif
            }
        }

        private static T GetSectionValue<T>(string keyName)
        {
            return Configuration.GetSection(SectionName).GetValue<T>(keyName);
        }

        public static T SectionValue<T>(this SectionValueKeys key)
        {
            return GetSectionValue<T>(key.GetKeyName());
        }

        public static string WebBaseURL => SectionValueKeys.WebBaseURL.SectionValue<string>();

        private static string GetKeyName(this SectionValueKeys secValue)
        {
            switch (secValue)
            {
                case SectionValueKeys.WebBaseURL:
                    return "WebBaseURL";

                default:
                    throw new Exception("Enum not defined");
            }
        }
    }

    public enum SectionValueKeys
    {
        WebBaseURL
    }
}